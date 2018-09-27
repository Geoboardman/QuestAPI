using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestAPI.Models
{
    public partial class MapObjects
    {
        public int Id { get; set; }
        public decimal Lat { get; set; }
        public decimal Lon { get; set; }
        public string Region { get; set; }
        public string Type { get; set; }
        public DateTime TimeStamp { get; set; }
        [NotMapped]
        public Dictionary<string,string> AttributesDict { get; set; }

        public string Attributes
        {
            get
            {
                return ToJSON(AttributesDict);
            }
            set
            {
                AttributesDict = FromJSON(value);
            }
        }

        private string ToJSON(Dictionary<string,string> dict)
        {
            return JsonConvert.SerializeObject(dict);
        }

        private Dictionary<string,string> FromJSON(string val)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(val);
        }
    }
}
