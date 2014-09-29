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

        static Random rng = new Random();                       //To generate random prices
        public static event priceCutEvent priceCut;             //Event when discount 
        public static event priceChangeEvent priceChange;       //Event when regular price change 
        private Int32 roomPrice;
        private Int32 p;                                        //priceCut counter; if p=10, thread terminates                             

        public HotelSupplier()
        {
            roomPrice = 10;
            p = 0;
        }

        public Int32 getPrice()
        {                           //returns price
            return roomPrice;
        }

        public void changePrice(Int32 price)                //Changes the roomPrice to new price 
        {
            if (price < roomPrice)
            {
                p++;                                        //Increments p if there is a priceCut
                if (priceCut != null)
                    priceCut(price);
            }
            else
            {
                if (priceChange != null)
                    priceChange(price);
            }
            roomPrice = price;

        }

        public void HotelFunc()                             //Thread Start function.
        {                                                   //Changes the price of the room for this hotel                    
            for (Int32 i = 0; i < 50; i++)
            {
                if (p > 10)
                {
                    if (priceChange != null)
                        priceChange(0);
                    Thread.CurrentThread.Abort();
                }
                else
                {
                    Int32 newPrice = rng.Next(5, 10);
                    changePrice(newPrice);
                    Console.WriteLine("New price for Hotel:{0} is:{1}", Thread.CurrentThread.Name, newPrice);
                    Thread.Sleep(500);

                    //check buffer for orders
                    //decode orders
                    //spawn orderProcessing threads for processing order
                }
            }
        }
    }
}
