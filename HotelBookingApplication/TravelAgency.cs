using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HotelBookingApplication
{
    class TravelAgency
    {
        public void AgencyFunc()
        {
            HotelSupplier supplier = new HotelSupplier();
            for (Int32 i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);
                Int32 p = supplier.getPrice();
            }
        }

        public void HotelRoomOnSale(Int32 p)
        {

        }
    }
}
