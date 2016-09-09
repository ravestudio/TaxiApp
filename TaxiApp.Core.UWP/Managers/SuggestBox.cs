using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TaxiApp.Core.Managers;
using TaxiApp.Core.DataModel;

namespace TaxiApp.Core.UWP.Managers
{
    public class SuggestBox : ISuggestBox
    {
        private AutoSuggestBox _autoSuggestBox = null;

        public void ClearListItems()
        {
            this.GetSuggestBox().ItemsSource = null;
        }

        public void Open()
        {

            this.GetSuggestBox().IsSuggestionListOpen = true;
        }

        public void SetListItems(IList<LocationItem> items)
        {
            this.GetSuggestBox().ItemsSource = items;
        }

        private AutoSuggestBox GetSuggestBox()
        {
            if (this._autoSuggestBox == null)
            {
                this._autoSuggestBox = ChildFinder.FindChild<AutoSuggestBox>(Window.Current.Content, "suggest_box");
            }

            return this._autoSuggestBox;
        }
    }
}
