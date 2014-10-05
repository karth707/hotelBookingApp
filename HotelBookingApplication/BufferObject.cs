using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingApplication
{
    class BufferObject                              //the object that is a single cell in the multicell buffer
    {
        private String hotel;                       //Has the data for the hotel that the booking was for and the encoded order
        private String encodedOrder;                //We store the hotel details along with the order so that we know which supplier should pick up and process the order

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
