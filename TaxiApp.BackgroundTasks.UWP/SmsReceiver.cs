using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Sms;

namespace TaxiApp.BackgroundTasks.UWP
{
    public sealed class SmsReceiver : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral _deferral = taskInstance.GetDeferral();

            SmsMessageReceivedTriggerDetails smsDetails = (SmsMessageReceivedTriggerDetails)taskInstance.TriggerDetails;

            string message = smsDetails.TextMessage.Body;

            _deferral.Complete();
        }
    }
}
