using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace HotelBookingApplication
{

    class TravelAgency
    {        
        private ConcurrentDictionary<String, PriceObject> hotelPricesMap = new ConcurrentDictionary<String, PriceObject>();     //HashMap which has hotel name and its current and old price
        private ConcurrentDictionary<String, byte> hotelsWithRoomsOnSale = new ConcurrentDictionary<String, byte>();            //HashSet containing hotels having price cuts at the moment
        private MultiCellBuffer mCellBuffer;                                                                                    //We use this set to pick up hotel prices from the HashMap
        private ConfirmationBuffer cBuffer;                         
        private Random rand = new Random();

        public TravelAgency(MultiCellBuffer mCellBuffer, ConfirmationBuffer cBuffer)                                        
        {
            this.mCellBuffer = mCellBuffer;
            this.cBuffer = cBuffer;
        }

        public void AgencyFunc()                                                                                                 //Travel Agency Thread
        {
            for (Int32 i = 0; i < 100; i++)
            {
                Thread.Sleep(500);
                if (hotelsWithRoomsOnSale.Count != 0)                                                                            //check If hotels have any price cuts at the moment
                {
                    List<String> saleHotels = hotelsWithRoomsOnSale.Keys.Cast<String>().ToList();                                
                    foreach (String hotelName in saleHotels)
                    {
                        Int32 currentPrice = hotelPricesMap[hotelName].getNewPrice();
                        Int32 oldPrice = hotelPricesMap[hotelName].getOldPrice();
                        if (currentPrice < oldPrice)
                        {
                            bookHotel(hotelName, Thread.CurrentThread.Name, currentPrice);                                      //Attempt Booking of a hotel with a price cut
                        }
                    }
                }
                
                String agent = Thread.CurrentThread.Name;
                int n = (int)Char.GetNumericValue(agent[agent.Length - 1]);
                if (cBuffer.buffer[n - 1] != null)                                                                               //Check if any booking confirmation is available
                {
                    String message = cBuffer.Get(n - 1);
                    Console.WriteLine(message);
                }
            }
        }

        public void bookHotel(String hotel, String agent, Int32 price)                                                          // Book Hotel by sending the order information to multi cell buffer
        {
            //attempt booking          
            OrderObject oo = new OrderObject();
            oo.setSenderId(agent);
            oo.setCardNo(rand.Next(2000,3000));
            oo.setNumberRooms(rand.Next(10, 50));
            oo.setReceiverId(hotel);
            oo.setPrice(price);
            oo.setBookingTimeStamp(System.DateTime.Now);
            String encodedOrder = Encoder.Encrypt(oo, "ABCDEFGHIJKLMNOP");
            Console.WriteLine("{0} will attempt to book {1} having price {2} at: {3}"
                  , agent, hotel, price, System.DateTime.Now);
            mCellBuffer.setOneCell(encodedOrder, oo.getReceiverId());            
        }

        public void HotelRoomOnSale(Int32 price)                                                                               // Price cut event subscription
        {
            Console.WriteLine("{0} rooms are on sale: as low as ${1} each", Thread.CurrentThread.Name, price);
            InsertIntoMap(Thread.CurrentThread.Name, price);
            hotelsWithRoomsOnSale.TryAdd(Thread.CurrentThread.Name, 0);                                                        //HashSet Implementation using dictionary with value for every key as a dummy byte variable

        }

        public void HotelRoomPriceChange(Int32 price)                                                                           //Regular price change event subscribtion
        {                                                                                                                       //We use this to update the hashmap having hotel names and prices at the moment
            InsertIntoMap(Thread.CurrentThread.Name, price);
            if (hotelsWithRoomsOnSale.Keys.Contains(Thread.CurrentThread.Name))
            {
                Byte dummy;                                                                                                     
                hotelsWithRoomsOnSale.TryRemove(Thread.CurrentThread.Name, out dummy); 
            }
        }

        public void InsertIntoMap(String hotelName, Int32 newPrice)                                                             // Build the HashMap containing hotel and its prices
        {
            if (!hotelPricesMap.Keys.Contains(hotelName))
            {
                PriceObject priceObject = new PriceObject();
                priceObject.UpdatePrices(newPrice);
                hotelPricesMap.TryAdd(hotelName, priceObject);
            }
            else
            {
                hotelPricesMap[hotelName].UpdatePrices(newPrice);
            }
        }
    }
}
