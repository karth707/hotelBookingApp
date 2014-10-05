using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HotelBookingApplication
{
    class MultiCellBuffer                                                   //Multi cell buffer that has the orders placed by the agency for the suppliers to read
    {        
        const int BUFFER_SIZE = 3;                                          //buffer of size 3
        int head = 0, tail = 0, nElements = 0;
        BufferObject[] buffer = new BufferObject[BUFFER_SIZE];        
            
        Semaphore write = new Semaphore(3, 3);                              //Read and write semaphores
        Semaphore read = new Semaphore(3, 3);   
                
        public void setOneCell(String encodedOrder, String hotel)           //placing an order in the buffer
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
                nElements++;
                //Console.WriteLine("Thread : " + Thread.CurrentThread.Name + " Leaving Write");
                write.Release();
                Monitor.Pulse(this);                
            }
        }

        public String getOneCell()                                      //Retrieving an order from the buffer
        {
            read.WaitOne();
            
            lock (this)
            {
                String encodedOrder = null;
                while (nElements == 0)
                {
                    Monitor.Wait(this, 2000);
                }
                if (Thread.CurrentThread.Name.Equals(buffer[head].getHotel()))                          //Since multiple hotel suppliers read from here, 
                {                                                                                       //we check if the order is for that particular supplier
                    //Console.WriteLine("Thread : " + Thread.CurrentThread.Name + " Entred Read");
                    encodedOrder = buffer[head].getEncodedOrder();
                    head = (head + 1) % BUFFER_SIZE;
                    nElements--;                    
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
