using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Core.DataModel.Order;

namespace TaxiApp.Core.Managers
{
    public interface IEditOrderControls
    {
        void OpenServicePicker(IList<OrderOption> serviceList);

        IList<OrderOption> GetServices();
    }
}
