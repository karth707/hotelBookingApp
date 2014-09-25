using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

public delegate void priceCutEvent(Int32 p);
namespace HotelBookingApplication{
public class Program{

    private static Int32 NUM_HOTELS = 1;
    private static Int32 NUM_AGENTS = 3;

    static void Main(string[] args){

        HotelSupplier supplier = new HotelSupplier();
        Thread[] hotelSupplier = new Thread[NUM_HOTELS];
        for (Int32 i = 0; i < NUM_HOTELS; i++)
        {
            hotelSupplier[i] = new Thread(new ThreadStart(supplier.HotelFunc));
            hotelSupplier[i].Name = "HSupplier:" + (i + 1).ToString();
            hotelSupplier[i].Start();
        }
        
        TravelAgency agency = new TravelAgency();
        HotelSupplier.priceCut += new priceCutEvent(agency.HotelRoomOnSale);
        Thread[] travelAgency = new Thread[NUM_AGENTS];
        for (Int32 i = 0; i < NUM_AGENTS; i++)
        {
            travelAgency[i] = new Thread(new ThreadStart(agency.AgencyFunc));
            travelAgency[i].Name = "TAgency:" + (i + 1).ToString();
            travelAgency[i].Start();
        }
    }
}
}
