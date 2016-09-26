using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Managers
{
    public class BackgroundTaskManager
    {
        IBackgroundService _backgroundService = null;
        public BackgroundTaskManager(IBackgroundService backgroundService)
        {
            this._backgroundService = backgroundService;
        }

        public void Register(string TaskName)
        {
            this._backgroundService.CreateTask();
        }
    }
}
