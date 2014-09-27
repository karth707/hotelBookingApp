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
        Int32 price = 0;       
        String HotelName= null;
        ReaderWriterLock rw = new ReaderWriterLock();

        public void AgencyFunc()
        {            
            for (Int32 i = 0; i < 50; i++)
            {                                
                Thread.Sleep(200);                                
                if (getPrice() != 0 && getHotelName() != null)
                {
                    bookHotel(getHotelName(), Thread.CurrentThread.Name, getPrice());
                }                                                        
            }
        }

        public void bookHotel(String hotel, String agent, Int32 price)
        {
            //attempt booking
            Console.WriteLine("{0} will attept to book {1} having price {2}"
                   , agent, hotel, price);   
        }

        public void HotelRoomOnSale(Int32 price)
        {            
                    Console.WriteLine("{0} rooms are on sale: as low as ${1} each", Thread.CurrentThread.Name, price);
                    setPrice(price);
                    setHotelName(Thread.CurrentThread.Name);                              
        }

        public void HotelRoomPriceChange(Int32 price)
        {           
                    setPrice(0);
                    setHotelName(null);
        }

        private void setPrice(Int32 price)
        {
            this.price = price;
        }

        private void setHotelName(String HotelName)
        {
            this.HotelName = HotelName;
            Console.WriteLine("hotel set to:{0}",this.HotelName);
        }

        private Int32 getPrice()
        {
            return this.price;
        }

        private String getHotelName()
        {
            return this.HotelName;
        }
    }
}
