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
        private ConcurrentDictionary<String, PriceObject> hotelPricesMap = new ConcurrentDictionary<String, PriceObject>();
        private ConcurrentDictionary<String, byte> hotelsWithRoomsOnSale = new ConcurrentDictionary<String, byte>();
        private MultiCellBuffer mCellBuffer;
        private Random rand = new Random();

        public TravelAgency(MultiCellBuffer mCellBuffer)
        {
            this.mCellBuffer = mCellBuffer;          
        }

        public void AgencyFunc()
        {
            for (Int32 i = 0; i < 50; i++)
            {
                Thread.Sleep(500);
                if (hotelsWithRoomsOnSale.Count != 0)
                {
                    List<String> saleHotels = hotelsWithRoomsOnSale.Keys.Cast<String>().ToList();
                    foreach (String hotelName in saleHotels)
                    {
                        Int32 currentPrice = hotelPricesMap[hotelName].getNewPrice();
                        Int32 oldPrice = hotelPricesMap[hotelName].getOldPrice();
                        if (currentPrice < oldPrice)
                        {
                            bookHotel(hotelName, Thread.CurrentThread.Name, currentPrice);
                        }
                    }
                }
            }
        }

        public void bookHotel(String hotel, String agent, Int32 price)
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

        public void HotelRoomOnSale(Int32 price)
        {
            Console.WriteLine("{0} rooms are on sale: as low as ${1} each", Thread.CurrentThread.Name, price);
            InsertIntoMap(Thread.CurrentThread.Name, price);
            hotelsWithRoomsOnSale.TryAdd(Thread.CurrentThread.Name, 0);

        }

        public void HotelRoomPriceChange(Int32 price)
        {
            InsertIntoMap(Thread.CurrentThread.Name, price);
            if (hotelsWithRoomsOnSale.Keys.Contains(Thread.CurrentThread.Name))
            {
                Byte dummy;
                hotelsWithRoomsOnSale.TryRemove(Thread.CurrentThread.Name, out dummy);
            }
        }

        public void InsertIntoMap(String hotelName, Int32 newPrice)
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
