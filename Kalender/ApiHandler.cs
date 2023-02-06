using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalender
{
    public static class ApiHandler
    {
        public static Dictionary<DateTime, string> GetData()
        {
            Dictionary<DateTime, string> data = new Dictionary<DateTime, string>();
            //Get API Data
            return data;
        }

        public static void FillData(DateTime dt, string titel)
        {
            //Fill API Data
        }

        //public static string[] GetPlaces(string plz)
        //{
        //    string[] places = new string[SessionData.plzdata.Count];
        //    for (int i = 0; i < SessionData.plzdata.Count; i++)
        //    {
        //        places[i] = SessionData.plzdata[plz];
        //    }
        //    //plzdata[plz]

        //    return places;
        //}
    }
}
