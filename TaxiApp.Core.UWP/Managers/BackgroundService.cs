using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace TaxiApp.Core.UWP.Managers
{
    public class BackgroundService : TaxiApp.Core.Managers.IBackgroundService
    {
        public void CreateTask()
        {

            try
            {
                var task = new BackgroundTaskBuilder();
                task.Name = "SMSReceiver";
                task.TaskEntryPoint = typeof(TaxiApp.BackgroundTasks.UWP.SmsReceiver).ToString();
                var smsTrigger = new SystemTrigger(SystemTriggerType.SmsReceived, false);
                task.SetTrigger(smsTrigger);

                var backgroundTask = task.Register();
            }
            catch(Exception ex)
            {

            }

            
        }
    }
}
