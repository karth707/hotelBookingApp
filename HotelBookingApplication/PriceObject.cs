using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingApplication
{
    class PriceObject
    {
        Int32 oldPrice;
        Int32 newPrice;

        public PriceObject()
        {
            oldPrice = 0;
            newPrice = 0;
        }

        public void UpdatePrices(Int32 newPrice)
        {
            this.oldPrice = getNewPrice();
            this.newPrice = newPrice;
        }

        public Int32 getNewPrice()
        {
            return newPrice;
        }

        public Int32 getOldPrice()
        {
            return oldPrice;
        }
    }
}
