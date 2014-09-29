using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HotelBookingApplication
{
    class HotelSupplier
    {

        static Random rng = new Random();               // To generate random prices
        public static event priceCutEvent priceCut;     // Link event to delegate
        private static Int32 roomPrice = 10;
        private static Int32 p = 0;                     //priceCut counter; if p=10, thread terminates

        public Int32 getPrice() {                       //returns price
            return roomPrice; 
        }

        public static void changePrice(Int32 price)     //Changes the roomPrice to new price 
        {
            if (price < roomPrice)
            {
                p++;                                    //Increments p if there is a priceCut
                if (priceCut != null)  
                    priceCut(price);               
            }
            roomPrice = price;
        }

        public void HotelFunc()                         //Thread Start function.
        {                                               //Changes the price of the room for this hotel
            for (Int32 i = 0; i < 50; i++)
            {
                if (p == 10)
                {
                    break;
                }
                Thread.Sleep(500);
                Int32 newPrice = rng.Next(5, 10);
                HotelSupplier.changePrice(newPrice);
            }
        }
    }
}
