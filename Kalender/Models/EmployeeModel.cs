using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kalender.Models
{
    public class EmployeeModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string surname { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string plz { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public DateTime birthday { get; set; }
    }
}
