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
        ConcurrentDictionary<String, PriceObject> hotelPricesMap = new ConcurrentDictionary<String, PriceObject>();
        ConcurrentDictionary<String, byte> hotelsWithRoomsOnSale = new ConcurrentDictionary<String, byte>();

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
            Console.WriteLine("{0} will attept to book {1} having price {2}"
                   , agent, hotel, price);
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
