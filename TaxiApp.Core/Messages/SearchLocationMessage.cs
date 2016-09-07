using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Messages
{
    /// <summary>
    /// Найти локацию по текстовому запросу
    /// </summary>
    public class SearchLocationMessage
    {
        public string Text { get; set; }
    }
}
