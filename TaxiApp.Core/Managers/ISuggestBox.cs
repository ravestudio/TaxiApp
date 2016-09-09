using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Managers
{
    public interface ISuggestBox
    {
        void Open();

        void SetListItems(IList<TaxiApp.Core.DataModel.LocationItem> items);

        void ClearListItems();
    }
}
