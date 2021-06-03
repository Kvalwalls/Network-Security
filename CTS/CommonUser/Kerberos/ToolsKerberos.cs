using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUser.Kerberos
{
    class ToolsKerberos
    {
        public const int LIFE_TIME = 60;
        public static long GenerateTS()
        {
            TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(timeSpan.TotalMilliseconds) / 1000;
        }

        public static bool VerifyTS(long ts, long lifetime)
        {
            return (GenerateTS() - ts < lifetime);
        }
    }
}
