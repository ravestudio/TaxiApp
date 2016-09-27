using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Managers
{
    public interface IChatService
    {
        Task<ISMSStorage> GetStorage();
    }

    public interface ISMSStorage : IDisposable
    {
        Task<string> GetMessage();
    }
}
