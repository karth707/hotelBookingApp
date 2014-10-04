using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingApplication
{
    class BufferObject
    {
        private String hotel;
        private String encodedOrder;

        public BufferObject(String encodedOrder, String hotel)
        {
            this.encodedOrder = encodedOrder;
            this.hotel = hotel;
        }

        public String getHotel()
        {
            return hotel;
        }

        public String getEncodedOrder()
        {
            return encodedOrder;
        }
    }
}
