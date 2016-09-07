using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Core.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TaxiApp.Core.UWP.ViewModel
{
    public class InitializationFrameStrategy : IInitializationFrameStrategy
    {
        public Frame GetFrame()
        {
            Frame frame = null;

            SplitView split = TaxiApp.Core.Managers.ChildFinder.FindChild<SplitView>(Window.Current.Content, "panel_splitter");            

            if (split != null)
            {
                Grid grid = split.Content as Grid;
                frame = TaxiApp.Core.Managers.ChildFinder.FindChild<Frame>(grid, "mainFrame");
            }

            return frame;
        }
    }
}
