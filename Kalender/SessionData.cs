using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalender
{
    public static class SessionData
    {
        public static int editorid { get; set; }
        public static string editorname { get; set; }
        public static string editorsurname { get; set; }
        public static int monthcounter { get; set; }
        public static int currentday { get; set; }
        public static int currentmonth { get; set; }
        public static string titel { get; set; }
        public static string assdate { get; set; }
        
        //temp solution until API func --ApiHandler-- is working
        public static Dictionary<DateTime, string> datadic = new Dictionary<DateTime, string>();
    }
}
