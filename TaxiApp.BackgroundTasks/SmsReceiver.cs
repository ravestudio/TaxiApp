using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.ApplicationModel.Background;

namespace TaxiApp.BackgroundTasks
{
    public sealed class SmsReceiver : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            Windows.Devices.sm
            SmsReceivedEventDetails smsDetails = (SmsReceivedEventDetails)taskInstance.TriggerDetails;
        }
    }
}
