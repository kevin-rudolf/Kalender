using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalender
{
    public static class SessionData
    {
        public static string editorname { get; set; }
        public static string editorsurname { get; set; }
        public static string username { get; set; }
        public static int monthcounter { get; set; }
        public static DateTime CurrentDate { get; set; }
        public static string DragTemp { get; set; }

        public static Dictionary<DateTime, string> datadic = new Dictionary<DateTime, string>();
        public static Dictionary<DateTime, int> dataimp = new Dictionary<DateTime, int>();
        public static Dictionary<string, string> plzdata = new Dictionary<string, string>();
    }
}
