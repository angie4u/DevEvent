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
            client = new MobileServiceClient(Constants.ApplicationURL);
            var store = new MobileServiceSQLiteStore("localstore.db");
            store.DefineTable<MobileEvent>();            
            client.SyncContext.InitializeAsync(store);
            // 로컬 SQLite에서 데이터 가져온다. 
            // 아직은 로컬 데이터이고 SyncAsync() 를 해야만 서버측 데이터가 싱크
            eventTable = client.GetSyncTable<MobileEvent>();     
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
                    await SyncAsync();
                }

                var list = client.GetSyncTable<MobileEvent>();

                IEnumerable<MobileEvent> items = await eventTable
                                    .Where(x => x.PublishState == PublishState.Published)
                                    .ToEnumerableAsync();

                //var i = eventTable;
                //IEnumerable<MobileEvent> items2 = await eventTable.ToEnumerableAsync();

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
                await client.SyncContext.PushAsync();
                await eventTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allPublishedItems",
                    // 여기에서 Published 데이터만 가져오는 Query 작성 
                    eventTable.CreateQuery());
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

        /// <summary>
        /// Favorite 상태 변경
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isfavorite"></param>
        /// <returns></returns>
        public async Task ToggleFavoriteEvent(MobileEvent item)
        {
            // Favorite 설정 변경
            item.IsFavorite = !item.IsFavorite;
            // 로컬에 저장
            await SaveTaskAsync(item);
            // 즉시 씽크 
            await SyncAsync();
        }
    }
}
