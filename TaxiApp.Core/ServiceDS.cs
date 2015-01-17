using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core
{
    public class ServiceDS
    {
        public IList<string> Items { get; set; }

        public string Selected { get; set; }

        public ServiceDS()
        {
            this.Items = new List<string>();

            this.Items.Add("Audio");
            this.Items.Add("Smoking");
            this.Items.Add("Driver arn't smoking");
            this.Items.Add("Driver are Girl");
            this.Items.Add("Driver are men");
        }
    }
}
