using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace TaxiApp.Core.ViewModel
{
    public interface IInitializationFrameStrategy
    {
        Frame GetFrame();
    }
}
