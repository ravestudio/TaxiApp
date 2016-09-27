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

        public SMSStorage(ChatMessageStore store)
        {
            _store = store;

        }

        TypedEventHandler<ChatMessageStore, ChatMessageStoreChangedEventArgs> handler = null;

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }

        public Task<string> GetMessage()
        {
            TaskCompletionSource<string> TCS = new TaskCompletionSource<string>();

            handler = (sender, args) => {

                if (args.Kind == ChatStoreChangedEventKind.MessageCreated)
                {
                    var reader = _store.GetMessageReader();

                    IReadOnlyList<ChatMessage> msgList = reader.ReadBatchAsync().AsTask().Result;

                    ChatMessage msg = msgList.FirstOrDefault();

                    TCS.SetResult(msg == null ? null : msg.Body);
                }
            };

            _store.StoreChanged += handler;

            return TCS.Task;
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
