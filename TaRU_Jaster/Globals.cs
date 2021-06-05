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
}
