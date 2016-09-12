using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Core.DataModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace TaxiApp.Core.UWP.Common
{
    public class SuggestionChosenParameterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var args = (AutoSuggestBoxSuggestionChosenEventArgs)value;

            TaxiApp.Core.Common.SuggestionChosenArgs coreArgs = new TaxiApp.Core.Common.SuggestionChosenArgs();
            coreArgs.Selectedlocation = (LocationItem)args.SelectedItem;
            return coreArgs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
