using System;
using Xunit;
using Xunit.Abstractions;

using CSVSchema;

namespace CSVSchema.Test
{
    public class Data
    {
        public string Name { get; set; }
        public int Quantity { get; set; }

        public override string ToString()
        {
            return $"{Name} {Quantity}";
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 97;
                hash = hash * 89 + Name.GetHashCode();
                hash = hash * 89 + Quantity.GetHashCode();
                return hash;
            }
        }
    }

    public class CsvConverterTest
    {
        private readonly string data;
        private readonly Data[] dataObj;
        private readonly ITestOutputHelper _output;

        public CsvConverterTest(ITestOutputHelper output)
        {
            _output = output;
            data = $"|{Environment.NewLine}name|quantity{Environment.NewLine}fan|10{Environment.NewLine}light|22";
            dataObj = new Data[]
            {
                new Data(){Name="fan", Quantity=10 },
                new Data(){Name="light", Quantity=22}
            };
        }

        [Fact]
        public void ValidateCsvFile()
        {
            Assert.True(CSV.IsValidCsvSchemaFile(data));
        }

        [Fact]
        public void ReadSchema()
        {
            Schema schema = CSV.GetSchema(data);

            Assert.Equal("|", schema.Separator);
            Assert.Equal(2, schema.Properties.Length);
        }

        [Fact]
        public void CheckSchemaCompatibleWithType()
        {
            Schema schema = CSV.GetSchema(data);

            Assert.True(schema.IsCompatibleWith(typeof(Data)));
        }

        [Fact]
        public void Deserialize()
        {
            Data[] objs = CSV.Deserialize<Data>(data);

            Assert.Equal(2, objs.Length);
            foreach (var obj in objs)
            {
                _output.WriteLine(obj.ToString());
            }
        }

        [Fact]
        public void Serialize()
        {
            string serializedObjs = CSV.Serialize(dataObj);
        }
    }
}
