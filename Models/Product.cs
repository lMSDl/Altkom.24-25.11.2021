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
        //Konwencja nazewnictwa pól zapasowaych
        //private string displayName;
        //private string _displayName;
        private string m_displayName;


        //Konfigurujemy token współbieżności
        //[ConcurrencyCheck]
        public string Name { get; set; }
        public float Price { get; set; }

        public string DisplayName
        {
            get
            {
                return m_displayName;
            }

            set
            {
                m_displayName = value;
            }
        }

        public DateTime ExpirationDate { get; set; } = DateTime.Now.AddDays(5);
        //public int DaysToExpire { get; }
        //Field-only property
        private int _daysToExpire;

        //Konfigurujemy token współbieżności za pomocą sygnatury czasowej
        //[Timestamp]
        public byte[] Timestamp { get; set; }

    }
}
