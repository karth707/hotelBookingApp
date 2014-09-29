using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingApplication
{
    class Decoder
    {

        public static OrderObject Decrypt(String input, string key)
        {
            Byte[] inputArray = Convert.FromBase64String(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Decoder.ByteArrayToObject(resultArray);
        }

        public static OrderObject ByteArrayToObject(byte[] b)
        {
            MemoryStream memStream = new MemoryStream(b);
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Read(b, 0, b.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            return (OrderObject)binForm.Deserialize(memStream);
        }
            
        
    }
}
