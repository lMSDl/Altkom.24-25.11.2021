using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Product : Entity
    {
        //Konfigurujemy token współbieżności
        //[ConcurrencyCheck]
        public string Name { get; set;}
        public float Price { get; set; }

        public string DisplayName { get; }
        public DateTime ExpirationDate { get; set; } = DateTime.Now.AddDays(5);
        public int DaysToExpire { get; }

        //Konfigurujemy token współbieżności za pomocą sygnatury czasowej
        //[Timestamp]
        public byte[] Timestamp { get; set; }

    }
}
