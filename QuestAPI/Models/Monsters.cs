using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuestAPI.Models
{
    public partial class Monsters
    {
        public int Id { get; set; }
        [NotMapped]
        public Dictionary<string, string> AttributesDict { get; set; }

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

        private string ToJSON(Dictionary<string, string> dict)
        {
            return JsonConvert.SerializeObject(dict);
        }

        private Dictionary<string, string> FromJSON(string val)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(val);
        }
    }
}
