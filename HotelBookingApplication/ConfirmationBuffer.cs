using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBookingApplication
{
    class ConfirmationBuffer                                //This buffer holds the confirmation for the orders that were processed by the hotel suppliers
    {                                                       //We place the order confirmation for the ith agency in the ith ocation of this buffer
        const int BUFFER_SIZE = Program.NUM_AGENTS;
        public String[] buffer = new String[BUFFER_SIZE];

        public void Put(String message, int i)              //Place the order confirmation in one of the cells
        {

            lock (this)
            {
                while (buffer[i] != null)
                {
                    try
                    {
                        Monitor.Wait(this, 2000);
                    }
                    catch { Console.WriteLine("error"); }
                }
                buffer[i] = message;
                Monitor.Pulse(this);
            }
            }
        

        public String Get(int i)                            //Retrieve an order confirmation
        {
            lock (this)
            {
                while (buffer[i] == null)
                {
                    try
                    {
                        Monitor.Wait(this, 2000);
                    }
                    catch { Console.WriteLine("error"); }
                }

                String message = buffer[i];
                buffer[i] = null;
                Monitor.Pulse(this);

                return message;

            }
            }
        }
}
