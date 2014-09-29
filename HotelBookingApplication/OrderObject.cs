using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingApplication
{
    [Serializable]
    class OrderObject
    {
        private String senderId;
        private int cardNo, numberRooms, price;
        private String receiverId;

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
    }
}
