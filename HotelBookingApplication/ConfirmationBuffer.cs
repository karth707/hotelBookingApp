using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBookingApplication
{
    class ConfirmationBuffer
    {
        const int BUFFER_SIZE = 5;
        public String[] buffer = new String[BUFFER_SIZE];
        int n = 0;

        public void Put(String message, int i)
        {

            lock (this)
            {
                if (buffer[i] != null)
                {
                    try
                    {
                        Monitor.Wait(this);
                    }
                    catch { Console.WriteLine("error"); }
                }
                buffer[i] = message;
                Monitor.Pulse(this);


                //Console.WriteLine("writing thread " + Thread.CurrentThread.Name + " " + message + " " + n);
            }
            }
        

        public String Get(int i)
        {
            lock (this)
            {
                if (buffer[i] == null)
                {
                    try
                    {
                        Monitor.Wait(this);
                    }
                    catch { Console.WriteLine("error"); }
                }


                //Console.WriteLine("reading thread entered");
                String message = buffer[i];
                buffer[i] = null;

               // Console.WriteLine("Reading thread " + Thread.CurrentThread.Name + " " + message + " " + n);
                Monitor.Pulse(this);

                return message;

            }
            }
        }
}
