using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Concurrent;

public delegate void priceCutEvent(Int32 price);
public delegate void priceChangeEvent(Int32 price);

namespace HotelBookingApplication{
public class Program{

    private static Int32 NUM_HOTELS = 2;
    private static Int32 NUM_AGENTS = 5;
    private static MultiCellBuffer mCellBuffer = new MultiCellBuffer();
    private static ConfirmationBuffer cBuffer = new ConfirmationBuffer();

    static void Main(string[] args){

        HotelSupplier[] suppliers = new HotelSupplier[NUM_HOTELS];
        Thread[] hotelSupplier = new Thread[NUM_HOTELS];
        for (Int32 i = 0; i < NUM_HOTELS; i++)
        {
            suppliers[i] = new HotelSupplier(mCellBuffer, cBuffer);            
            hotelSupplier[i] = new Thread(new ThreadStart(suppliers[i].HotelFunc));
            hotelSupplier[i].Name = "HSupplier:" + (i + 1).ToString();
            hotelSupplier[i].Start();
        }

        TravelAgency agency = new TravelAgency(mCellBuffer, cBuffer);
        HotelSupplier.priceCut += new priceCutEvent(agency.HotelRoomOnSale);
        HotelSupplier.priceChange += new priceChangeEvent(agency.HotelRoomPriceChange);

        Thread[] travelAgency = new Thread[NUM_AGENTS];
        for (Int32 i = 0; i < NUM_AGENTS; i++)
        {
            travelAgency[i] = new Thread(new ThreadStart(agency.AgencyFunc));
            travelAgency[i].Name = "TAgency:" + (i + 1).ToString();
            travelAgency[i].Start();
        }

        for (Int32 i = 0; i < NUM_HOTELS; i++)
        {
            hotelSupplier[i].Join();
        }

        for (Int32 i = 0; i < NUM_AGENTS; i++)
        {
            travelAgency[i].Abort();
        }
    }
}
}
