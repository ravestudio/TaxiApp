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
        public Task<string> GetMessage()
        {
            TaskCompletionSource<string> TCS = new TaskCompletionSource<string>();

            ChatMessageManager.RequestStoreAsync().AsTask().ContinueWith(t =>
            {
                ChatMessageStore store = t.Result;

                TypedEventHandler<ChatMessageStore, ChatMessageStoreChangedEventArgs> handler = null;

                handler += (sender, args) => {
                    var reader = store.GetMessageReader();

                    IReadOnlyList<ChatMessage> msgList = reader.ReadBatchAsync().AsTask().Result;

                    ChatMessage msg = msgList.FirstOrDefault();

                    TCS.SetResult(msg == null ? null : msg.Body);

                    store.StoreChanged -= handler;
                };

                store.StoreChanged += handler;

            });

            return TCS.Task;
        }

    }
}
