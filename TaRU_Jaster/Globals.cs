using System.Text;

namespace TaRU_Jaster
{

    class Global
    {
        public static Form1 g_form1;
    }

    class Utils
    {
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("0x{0:x2} ", b);
            return hex.ToString();
        }
    }

    static class Logger
    {
        public const int DEBUG =  0;
        public const int INFO  =  1;
        public const int WARN  =  2;
        public const int ERR   =  3;
        public const int CRIT  =  4;
        public const int OUT   =  5;
        public const int EMERG  = 6;

        public static readonly string[] _error_desc = {
            "DEBUG:    ", 
            "INFO:     ",
            "WARNING:  ",
            "ERROR:    ",
            "CRITICAL: ",
            "EXECUTOR: ",
            "EMERG:    "
        };

        public static void LOG(string w_msg, int w_level = DEBUG)
        {
            Global.g_form1.log_msg(w_msg, w_level);
        }
    }
}
