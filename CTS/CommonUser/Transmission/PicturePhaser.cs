using System;
using System.IO;

namespace CommonUser.Transmission
{
    class PicturePhaser
    {
        /// <summary>
        /// 图片转换Base64方法
        /// </summary>
        /// <param name="picName">图片名称</param>
        /// <returns>Base64格式字符串</returns>
        public static string PictureToBase64(string picName)
        {
            FileStream fileStream = new FileStream(picName, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, buffer.Length);
            fileStream.Close();
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// Base64转换图片方法
        /// </summary>
        /// <param name="Base64Str">Base64格式字符串</param>
        /// <param name="Mid">影片号</param>
        public static void Base64ToPicture(string Base64Str, string Mid)
        {
            string picName = "..\\..\\MoviePictures\\" + Mid + ".jpg";
            FileStream fs = new FileStream(picName, FileMode.Create);
            byte[] picBytes = Convert.FromBase64String(Base64Str);
            fs.Write(picBytes, 0, picBytes.Length);
            fs.Flush();
            fs.Close();
        }
    }
}