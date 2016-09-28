using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Core.Managers;
using Windows.ApplicationModel.Chat;
using Windows.Foundation;
//using Windows.UI.Xaml.Input;

namespace TaxiApp.Core.UWP.Managers
{
    public class ChatService : IChatService
    {
        public Task<ISMSStorage> GetStorage()
        {
            TaskCompletionSource<ISMSStorage> TCS = new TaskCompletionSource<ISMSStorage>();

            ChatMessageManager.RequestStoreAsync().AsTask().ContinueWith(t =>
            {
                TCS.SetResult(new SMSStorage(t.Result));
            });

            return TCS.Task;
        }
    }

    public class SMSStorage : ISMSStorage
    {
        private ChatMessageStore _store = null;

        private bool disposed = false;
        private bool completed = false;

        TaskCompletionSource<string> TCS = null;

        public SMSStorage(ChatMessageStore store)
        {
            _store = store;

            TCS = new TaskCompletionSource<string>();

        }

        TypedEventHandler<ChatMessageStore, ChatMessageStoreChangedEventArgs> handler = null;

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }

        public Task<string> GetMessage()
        {
            handler = (sender, args) => {

                if (args.Kind == ChatStoreChangedEventKind.MessageCreated)
                {
                    var reader = _store.GetMessageReader();

                    IReadOnlyList<ChatMessage> msgList = reader.ReadBatchAsync().AsTask().Result;

                    ChatMessage msg = msgList.FirstOrDefault();

                    SetResult(msg);
                }
            };

            _store.StoreChanged += handler;

            Task.Delay(10000).ContinueWith(t => { SetResult(null); });

            return TCS.Task;
        }

        private void SetResult(ChatMessage msg)
        {
            if (!completed)
            {
                completed = true;
                TCS.SetResult(msg == null ? null : msg.Body);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            // cleanup
            _store.StoreChanged -= handler;
            _store = null;

           disposed = true;
        }


    }
}
