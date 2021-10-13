using Newtonsoft.Json;
using System;

namespace Jg.wpf.core.Utility
{
    public static class JsonNewtonsoft
    {
        public static string ToJson(this object o)
        {
            if (o == null)
            {
                return null;
            }
            return JsonConvert.SerializeObject(o, Formatting.Indented);
        }

        public static T FromJson<T>(this string input)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(input);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static object FromJson(this string input, Type type)
        {
            try
            {
                var deserialized = JsonConvert.DeserializeObject(input, type);
                return deserialized;
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
