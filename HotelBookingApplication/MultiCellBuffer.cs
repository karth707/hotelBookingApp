using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HotelBookingApplication
{
    class MultiCellBuffer
    {        
        const int BUFFER_SIZE = 3;
        int head = 0, tail = 0, nElements = 0;
        BufferObject[] buffer = new BufferObject[BUFFER_SIZE];        

        Semaphore write = new Semaphore(3, 3);
        Semaphore read = new Semaphore(3, 3);

        public void setOneCell(String encodedOrder, String hotel)
        {
            write.WaitOne();
            //Console.WriteLine("Thread : " + Thread.CurrentThread.Name + " Entred Write");
            lock (this)
            {
                while (nElements == BUFFER_SIZE)
                {
                    Monitor.Wait(this, 2000);
                }
                BufferObject bufferObject = new BufferObject(encodedOrder, hotel);
                buffer[tail] = bufferObject;                
                tail = (tail + 1) % BUFFER_SIZE;
                //Console.WriteLine("Write to the buffer: {0}, {1}, {2}", encodedOrder, DateTime.Now, nElements);
                nElements++;
                //Console.WriteLine("Thread : " + Thread.CurrentThread.Name + " Leaving Write");
                write.Release();
                Monitor.Pulse(this);                
            }
        }

        public String getOneCell()
        {
            read.WaitOne();
            
            lock (this)
            {
                String encodedOrder = null;
                while (nElements == 0)
                {
                    Monitor.Wait(this, 2000);
                }
                if (Thread.CurrentThread.Name.Equals(buffer[head].getHotel()))
                {
                    //Console.WriteLine("Thread : " + Thread.CurrentThread.Name + " Entred Read");
                    encodedOrder = buffer[head].getEncodedOrder();
                    head = (head + 1) % BUFFER_SIZE;
                    nElements--;
                    //Console.WriteLine("Read from the buffer: {0} , {1}, {2}", encodedOrder, DateTime.Now, nElements);
                    //Console.WriteLine("Thread : " + Thread.CurrentThread.Name + " leaving Read");
                }
                read.Release();
                Monitor.Pulse(this);
                return encodedOrder;
            }             
        }

        public int getNElement()
        {
            return nElements;
        }
    }
}
