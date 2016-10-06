using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Managers
{
    public interface IMenu
    {
        void Open();
        IList<IMenuItem> GetMenuItems();
    }

    public interface IMenuItem
    {
        string Key { get; set; }
        string Text { get; set; }
    }

    public interface IMenuItemPersonal: IMenuItem
    {
        string PhoneNumber { get; set; }
    }
}
