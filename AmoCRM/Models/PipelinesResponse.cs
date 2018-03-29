using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmoCRM.Models
{
    public class Status
    {
        public int id { get; set; }
        public string name { get; set; }
        public string color { get; set; }
        public int sort { get; set; }
        public string editable { get; set; }
        public int pipeline_id { get; set; }
    }

    public class Statuses
    {
        public List<Status> status { get; set; }
    }

    public class Pipeline
    {
        public int id { get; set; }
        public int value { get; set; }
        public string label { get; set; }
        public string name { get; set; }
        public int sort { get; set; }
        public bool is_main { get; set; }
        public Statuses statuses { get; set; }
        public int leads { get; set; }
    }
}
