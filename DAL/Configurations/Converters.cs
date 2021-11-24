using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    public static class Converters
    {
        public static ValueConverter<T, string> ToBase64Converter<T>() where T : Enum
        {
            return new ValueConverter<T, string>(
                x => Convert.ToBase64String(Encoding.Default.GetBytes(x.ToString())),
                x => (T)Enum.Parse(typeof(T), Encoding.Default.GetString(Convert.FromBase64String(x))),
                new ConverterMappingHints(size: 20, unicode:false));
        }
    }
}
