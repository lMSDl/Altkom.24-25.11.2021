using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace Models
{
    public class Order : Entity
    {
        private ILazyLoader _lazyLoader;
        private ICollection<Product> products = new List<Product>();

        public Order()
        {
        }

        public Order(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        public DateTime DateTime { get; set; }

        public OrderTypes OrderType { get; set; }

        public Point DeliveryLocation { get; set; }

        //LazyLoading - wymaga właściwości wirtualnej
        //public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<Product> Products { get => _lazyLoader.Load(this, ref products); set => products = value; }
    }
}
