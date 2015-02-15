using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Entities
{
    public interface IEntity
    {
        void ReadData(Windows.Data.Json.JsonObject jsonObj);
    }

    public abstract class Entity<TId> : IEntity
    {
        public virtual TId Id
        {
            get
            {
                return _id;
            }
            set { _id = value; }
        }
        private TId _id;

        public virtual void ReadData(Windows.Data.Json.JsonObject jsonObj)
        {

        }

        public int ReadValue(Windows.Data.Json.JsonObject jsonObj, string field)
        {
            int value = 0;

            var fieldType = jsonObj[field].ValueType;

            if (fieldType == Windows.Data.Json.JsonValueType.Number)
            {
                value = (int)jsonObj[field].GetNumber();
            }

            if (fieldType == Windows.Data.Json.JsonValueType.String)
            {
                value = int.Parse(jsonObj[field].GetString(), System.Globalization.CultureInfo.InvariantCulture);
            }

            return value;
        }

    }
}
