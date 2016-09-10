using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace TaxiApp.Core.UWP.Common
{
    public class SuggestTextChangedParameterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var args = (AutoSuggestBoxTextChangedEventArgs)value;

            TaxiApp.Core.Common.SuggestTextChangedArgs coreArgs = new Core.Common.SuggestTextChangedArgs();
            coreArgs.ByUser = args.Reason == AutoSuggestionBoxTextChangeReason.UserInput;
            return coreArgs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
