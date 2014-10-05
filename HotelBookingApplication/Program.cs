/*
 * Hotel Booking Application Fall - 2014 CSE 445/598
 * Authors: Kartheek Nallepalli, Ayush Kumar Maheshwari
 * October 2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Concurrent;

public delegate void priceCutEvent(Int32 price);                            //delegate for a price cut event
public delegate void priceChangeEvent(Int32 price);                         //delegate for a regular price change event

namespace HotelBookingApplication{
public class Program{

    public static Int32 NUM_HOTELS = 3;                                    //Number of Hotel suppliers
    public static Int32 NUM_AGENTS = 5;                                    //Number of Travel Agents
    private static MultiCellBuffer mCellBuffer = new MultiCellBuffer();     //Multicell buffer
    private static ConfirmationBuffer cBuffer = new ConfirmationBuffer();   //Confirmation buffer for the travel agents to know when an order is confirmed

    static void Main(string[] args){

        HotelSupplier[] suppliers = new HotelSupplier[NUM_HOTELS];
        Thread[] hotelSupplier = new Thread[NUM_HOTELS];
        for (Int32 i = 0; i < NUM_HOTELS; i++)
        {
            suppliers[i] = new HotelSupplier(mCellBuffer, cBuffer);            
            hotelSupplier[i] = new Thread(new ThreadStart(suppliers[i].HotelFunc));         //Start the Hotel Supplier threads
            hotelSupplier[i].Name = "HSupplier:" + (i + 1).ToString();
            hotelSupplier[i].Start();
        }

        TravelAgency agency = new TravelAgency(mCellBuffer, cBuffer);
        HotelSupplier.priceCut += new priceCutEvent(agency.HotelRoomOnSale);                //event subscribtion for a price cut
        HotelSupplier.priceChange += new priceChangeEvent(agency.HotelRoomPriceChange);     //event subscribtion for a regular prie change

        Thread[] travelAgency = new Thread[NUM_AGENTS];
        for (Int32 i = 0; i < NUM_AGENTS; i++)
        {
            travelAgency[i] = new Thread(new ThreadStart(agency.AgencyFunc));               //Start travel agency threads
            travelAgency[i].Name = "TAgency:" + (i + 1).ToString();
            travelAgency[i].Start();
        }

        for (Int32 i = 0; i < NUM_HOTELS; i++)
        {
            hotelSupplier[i].Join();                                                        //join hotel suppliers
        }

        for (Int32 i = 0; i < NUM_AGENTS; i++)
        {
            travelAgency[i].Abort();                                                        //Terminate the agency threads once all the hotel suppliers finish
        }
    }
}
}
