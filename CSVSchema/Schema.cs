using System;
using System.Linq;

namespace CSVSchema
{
    public class Schema
    {
        public readonly string Separator;
        public readonly string[] Properties;

        public Schema(string[] properties, string separator = ",")
        {
            Properties = properties;
            Separator = separator;
        }

        public Schema(Type type, string separator = ",")
        {
            Separator = separator;
            Properties = type.GetProperties().Select(prop => prop.Name).ToArray();
        }

        public bool IsCompatibleWith(Type type)
        {
            var props = type.GetProperties();

            return Properties.All(p => props.FirstOrDefault(propInfo => string.Equals(propInfo.Name, p, StringComparison.OrdinalIgnoreCase)) != null);
        }

        public override string ToString()
        {
            return string.Join(Separator, Properties);
        }
    }
}