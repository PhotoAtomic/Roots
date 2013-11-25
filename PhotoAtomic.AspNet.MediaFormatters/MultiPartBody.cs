using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace PhotoAtomic.AspNet.MediaFormatters
{
    public class MultiPartBody
    {
        private IDictionary<string, object> values = new Dictionary<string, object>();


        public void Add(string key, object value)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (value == null) throw new ArgumentNullException("value");

            values[key] = value;
                        
        }

        public T Get<T>(string key)
        {
            object value;
            values.TryGetValue(key, out value);

            if (value is MultiPartJson)
            {
                return ((MultiPartJson)value).Get<T>();
            }
            else if (value is T)
            {
                return (T)value;
            }
            throw new ArgumentOutOfRangeException("key",key);
            

        }

        public void ExplodeCall(ApiController controller)
        {
            throw new NotImplementedException();
        }
    }
}
