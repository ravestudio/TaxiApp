using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Core.Managers;
using Windows.UI.Xaml;

namespace TaxiApp.Core.Common
{
    public class MenuItemTemplateSelector : Windows.UI.Xaml.Controls.DataTemplateSelector
    {
        public DataTemplate SimpleDataTemplate { get; set; }
        public DataTemplate PersonalDataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            DataTemplate template = null;
            template = SimpleDataTemplate;

            if (item as IMenuItemPersonal != null)
            {
                template = PersonalDataTemplate;
            }

            return template;
        }
    }
}
