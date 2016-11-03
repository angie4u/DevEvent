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
            // true 일때 서버와 싱크하고 로컬에 저장
            //  Unauth 같은 오류가 발생하면 호출하는 곳에서 처리
            if (syncItems)
            {
                await SyncAsync();
            }

            // false 일때는 로컬에서만 가져옴. 
            var list = client.GetSyncTable<MobileEvent>();

            IEnumerable<MobileEvent> items = await eventTable
                                .Where(x => x.PublishState == PublishState.Published)
                                .ToEnumerableAsync();

            //var i = eventTable;
            //IEnumerable<MobileEvent> items2 = await eventTable.ToEnumerableAsync();

            return new ObservableCollection<MobileEvent>(items);
        }

        public async Task SaveTaskAsync(MobileEvent item)
        {
            try
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
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message); 
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
                    eventTable.CreateQuery());
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                // 서버측에서 인증이 먼저 필요한 Method 의 경우 401 을 반환하게 된다. 
                if (msioe.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("Unauth access", msioe);
                }
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    // Authentication Error when push 
                    if (exc.PushResult.Status == MobileServicePushStatus.CancelledByAuthenticationError)
                    {
                        throw new UnauthorizedAccessException("Unauth access when push", exc);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("error: {0}",ex.ToString());
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
