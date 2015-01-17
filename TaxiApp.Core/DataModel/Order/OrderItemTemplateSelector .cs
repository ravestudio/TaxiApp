using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TaxiApp.Core.DataModel.Order
{
    //public class OrderItemTemplateSelector : Windows.UI.Xaml.Controls.DataTemplateSelector
    //{
    //    public DataTemplate OrderPointDataTemplate { get; set; }

    //    public DataTemplate OrderServiceDataTemplate { get; set; }

    //    public DataTemplate OrderDateTimeTemplate { get; set; }

    //    protected override Windows.UI.Xaml.DataTemplate SelectTemplateCore(object item, Windows.UI.Xaml.DependencyObject container)
    //    {
    //        Windows.UI.Xaml.DataTemplate template = null;

    //        if (item as TaxiApp.Core.DataModel.Order.OrderPoint != null)
    //        {
    //            template = this.OrderPointDataTemplate;
    //        }

    //        if (item as TaxiApp.Core.DataModel.Order.OrderService != null)
    //        {
    //            template = this.OrderServiceDataTemplate;
    //        }

    //        if (item as TaxiApp.Core.DataModel.Order.OrderDateTime != null)
    //        {
    //            template = this.OrderDateTimeTemplate;
    //        }

    //        return template;
    //    }
    //}
}
