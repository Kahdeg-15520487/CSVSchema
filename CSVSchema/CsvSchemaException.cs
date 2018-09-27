using System;
using System.Runtime.Serialization;

namespace CSVSchema
{
    [Serializable]
    public class CsvSchemaException : Exception
    {
        public CsvSchemaException()
        {
        }

        public CsvSchemaException(string message) : base(message)
        {
        }

        public CsvSchemaException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CsvSchemaException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}