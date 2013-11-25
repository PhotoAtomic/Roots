using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;

namespace PhotoAtomic.AspNet.MediaFormatters
{
    public class MultiPartJson
    {
        private Stream json;
        private Encoding encoding;
        private const int maxDepth = 256;
        private readonly JsonSerializerSettings settings;


        public MultiPartJson(Stream json, Encoding encoding, JsonSerializerSettings settings)
        {
            this.json = json;
            this.encoding = encoding;
            this.settings = settings;                       
        }

        public T Get<T>()
        {
            using (JsonTextReader jsonTextReader = new JsonTextReader(new StreamReader(json, encoding))) { 
                jsonTextReader.CloseInput = false;
                jsonTextReader.MaxDepth = maxDepth;

                JsonSerializer jsonSerializer = JsonSerializer.Create(settings);
                Exception error;
                jsonSerializer.Error += (sender, e) =>
                {
                    error = e.ErrorContext.Error;
                };                
                return (T) jsonSerializer.Deserialize(jsonTextReader, typeof(T));
            }
        }        
    }
}
