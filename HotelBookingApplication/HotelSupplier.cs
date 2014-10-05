using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace HotelBookingApplication
{
    class HotelSupplier
    {

        static Random rng = new Random();                       //To generate random prices
        public static event priceCutEvent priceCut;             //Event when discount 
        public static event priceChangeEvent priceChange;       //Event when regular price change 
        private Int32 roomPrice;
        private Int32 p;                                        //priceCut counter; if p=10, thread terminates                             
        private MultiCellBuffer mCellBuffer;
        private ConfirmationBuffer cBuffer;
        public Int32 locationCharge;

        public HotelSupplier(MultiCellBuffer mCellBuffer, ConfirmationBuffer cBuffer)
        {
            roomPrice = 1000;                                   //Initialize variables
            p = 0;
            this.mCellBuffer = mCellBuffer;
            this.cBuffer = cBuffer;
            this.locationCharge = rng.Next(15, 20);             //Each  hotel Supplier has its own location charge
        }

        public Int32 getPrice() {                           //returns price
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
            for (Int32 i = 0; i < 100; i++)
            {
                if (p > 10)                                 //If 10 price cuts have finished, the threads terminates
                {
                    if (priceChange != null)
                        priceChange(0);
                    Thread.CurrentThread.Abort();
                }
                else
                {
                    Int32 newPrice = rng.Next(500, 1000);       
                    changePrice(newPrice);                  //Changes price  
                    Thread.Sleep(500);
                    
                    if (mCellBuffer.getNElement() != 0)
                    {
                        String encodedOrder = mCellBuffer.getOneCell();                                     //Checking the multicell buffer for any orders
                        if (encodedOrder != null)
                        {
                            OrderObject decodedOrder = Decoder.Decrypt(encodedOrder, "ABCDEFGHIJKLMNOP");   //Decrypts the order    
                            Thread orderThread = new Thread(() => this.orderProcessing(decodedOrder));      //Starts order processing parameterized thread with the argument as the order
                            orderThread.Start();
                        }
                    }                    
                }
            }
        }
                
        public void orderProcessing(OrderObject decodedOrder)                                                           //Order Processing thread
        {
            String agent = decodedOrder.getSenderId();
            int n = (int)Char.GetNumericValue(agent[agent.Length - 1]);
            if(decodedOrder.getCardNo() < 2000 && decodedOrder.getCardNo() > 3000)                                      //Checks card information
            {
                String failedBooking = "Booking failed!! Details: Processed for "+ decodedOrder.getNumberRooms()        //booking failed message
                +" rooms at $"+decodedOrder.getPrice()
                +" per room for "+ decodedOrder.getSenderId() 
                +" by "+ decodedOrder.getReceiverId();

                cBuffer.Put(failedBooking, n - 1);                                          //Place failed order in the order confirmation buffer
                return;                 
            }            
            double tax = 0.08;                                                              //tax
            double finalPrice = decodedOrder.getNumberRooms() * decodedOrder.getPrice();    //final price
            finalPrice += finalPrice * tax;
            finalPrice += this.locationCharge;                                              //adding location charges

            TimeSpan timeSpan = System.DateTime.Now - decodedOrder.getBookingTimeStamp();
                        
            String confirmedBooking = "Booking confirmed at "+System.DateTime.Now           //Booking confirmed message
                +" in "+timeSpan
                +" seconds!! Details: Processed for "+ decodedOrder.getNumberRooms()
                +" rooms at $"+decodedOrder.getPrice()
                +" per room for "+ decodedOrder.getSenderId() 
                +" by "+ decodedOrder.getReceiverId()
                +" at total cost $"+ finalPrice
                +" including Location Charge $" + this.locationCharge;

                cBuffer.Put(confirmedBooking, n - 1);                                       //Place confirmed booking in the confirmation buffer
        }
    }
}
