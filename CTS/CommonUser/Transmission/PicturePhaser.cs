using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUser.Transmission
{
    class PicturePhaser
    {
        public static byte[] PictureToBytes(String filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);

            byte[] buffer = new byte[fs.Length];

            fs.Read(buffer, 0, buffer.Length);
            fs.Close();

            return buffer;
        }
        public static void BytesToPicture(byte[] bytes)
        {
            FileStream fs = new FileStream("D:\\1\\M.jpg", FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Flush();
            fs.Close();
        }
    }
}
