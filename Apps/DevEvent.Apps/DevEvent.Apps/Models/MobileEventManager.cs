using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace DevEvent.Apps.Models
{
    public partial class MobileEventManager
    {
        static MobileEventManager defaultInstance = new MobileEventManager();
        IMobileServiceClient client;
        IMobileServiceSyncTable<MobileEvent> eventTable;

        private MobileEventManager()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);
            var store = new MobileServiceSQLiteStore("localstore.db");
            store.DefineTable<MobileEvent>();
            this.client.SyncContext.InitializeAsync(store);
            this.eventTable = client.GetSyncTable<MobileEvent>();
        }

        public static MobileEventManager DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        public IMobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public bool IsOfflineEnabled
        {
            get { return eventTable is IMobileServiceSyncTable<MobileEvent>; }
        }

        public async Task<ObservableCollection<MobileEvent>> GetEventItemsAsync(bool syncItems = false)
        {
            try
            {
                if (syncItems)
                {
                    await this.SyncAsync();
                }
                IEnumerable<MobileEvent> items = await eventTable
                                    .Where(x => x.PublishState == PublishState.Published)
                                    .ToEnumerableAsync();
                return new ObservableCollection<MobileEvent>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
            }
            return null;
        }

        public async Task SaveTaskAsync(MobileEvent item)
        {
            if (item.Id == null)
            {
                await eventTable.InsertAsync(item);
            }
            else
            {
                await eventTable.UpdateAsync(item);
            }
        }

        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();
                await this.eventTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allPublishedItems",
                    this.eventTable.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }
                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }
    }
}
