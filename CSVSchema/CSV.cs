using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CSVSchema
{
    public static class CSV
    {
        public static T[] Deserialize<T>(string data) where T : new()
        {
            Schema schema = GetSchema(data);

            if (!schema.IsCompatibleWith(typeof(T)))
            {
                throw new CsvSchemaException(schema.ToString());
            }

            List<T> result = new List<T>();
            using (var sr = new StringReader(data))
            {
                sr.ReadLine();
                sr.ReadLine();

                while (sr.Peek() > 0)
                {
                    var values = sr.ReadLine().Split(schema.Separator);
                    T obj = new T();
                    var properties = obj.GetType().GetProperties();
                    for (int i = 0; i < schema.Properties.Length; i++)
                    {
                        //PropertyInfo prop = objType.GetProperty(schema.Properties[i]);
                        PropertyInfo prop = properties.First(pinfo => pinfo.Name.Equals(schema.Properties[i], StringComparison.OrdinalIgnoreCase));
                        Type propType = prop.PropertyType;
                        prop.SetValue(obj, Convert.ChangeType(values[i], propType), null);
                    }
                    result.Add(obj);
                }
            }

            return result.ToArray();
        }

        public static Schema GetSchema(string data)
        {
            using (var sr = new StringReader(data))
            {
                var separator = sr.ReadLine();
                var properties = sr.ReadLine().Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
                return new Schema(properties, separator);
            }
        }

        public static bool IsValidCsvSchemaFile(string data)
        {
            using (var sr = new StringReader(data))
            {
                sr.ReadLine();
                sr.ReadLine();
                if (sr.Peek() <= 0)
                {
                    return false;
                }
                return true;
            }
        }

        //public static string Serialize(object dataObj, string separator = ",")
        //{
        //    StringBuilder sb = new StringBuilder();
        //    using (var sw = new StringWriter(sb))
        //    {
        //        Schema schema = new Schema(dataObjs[0].GetType(), separator);
        //        sw.WriteLine(separator);
        //        sw.WriteLine(string.Join(separator, schema.Properties));
        //        foreach (var dataObj in dataObjs)
        //        {
        //            sw.WriteLine(Serialize(dataObj, schema));
        //        }
        //    }
        //    return sb.ToString();
        //}

        public static string Serialize(object[] dataObjs, string separator = ",")
        {
            StringBuilder sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                Schema schema = new Schema(dataObjs[0].GetType(), separator);
                sw.WriteLine(separator);
                sw.WriteLine(string.Join(separator, schema.Properties));
                foreach (var dataObj in dataObjs)
                {
                    sw.WriteLine(Serialize(dataObj, schema));
                }
            }
            return sb.ToString();
        }

        public static string Serialize(object dataObj, Schema schema)
        {
            throw new NotImplementedException();
        }
    }
}
