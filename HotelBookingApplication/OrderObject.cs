using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingApplication
{
    [Serializable]
    class OrderObject                               //Order object having all details of the order ordered by the travel agent
    {
        private String senderId;
        private int cardNo, numberRooms, price;
        private String receiverId;
        private DateTime bookingTimeStamp;

        public OrderObject()
        {
            senderId = null;
            cardNo = 0;
            numberRooms = 0;
            receiverId = null;
        }

        public void setSenderId(String senderId)
        {
            this.senderId = senderId;
        }

        public String getSenderId()
        {
            return senderId;
        }

        public void setCardNo(int cardNo)
        {
            this.cardNo = cardNo;
        }

        public int getCardNo()
        {
            return cardNo;
        }

        public void setNumberRooms(int numberRooms)
        {
            this.numberRooms = numberRooms;
        }

        public int getNumberRooms()
        {
            return numberRooms;
        }

        public void setReceiverId(String receiverId)
        {
            this.receiverId = receiverId;
        }

        public String getReceiverId()
        {
            return receiverId;
        }

        public void setPrice(int price)
        {
            this.price = price;
        }

        public int getPrice()
        {
            return price;
        }

        public void setBookingTimeStamp(DateTime bookingTimeStamp)
        {
            this.bookingTimeStamp = bookingTimeStamp;
        }

        public DateTime getBookingTimeStamp()
        {
            return bookingTimeStamp;
        }
    }
}
