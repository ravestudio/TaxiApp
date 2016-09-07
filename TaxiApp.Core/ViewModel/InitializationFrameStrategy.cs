using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TaxiApp.Core.ViewModel
{
    public class InitializationFrameStrategy : IInitializationFrameStrategy
    {
        public Frame GetFrame()
        {
            return Window.Current.Content as Frame;
        }
    }
}
