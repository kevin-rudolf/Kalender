﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kalender.Models
{
    public class CalendarModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string eid { get; set; }
        public string titel { get; set; }
        public int importance { get; set; }
        public string plz { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string note { get; set; }
        public int repeat { get; set; }
        public DateTime date { get; set; }
    }
}
