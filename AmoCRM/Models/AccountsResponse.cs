using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmoCRM.Models
{
    public class taskType
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class noteType
    {
        public int id { get; set; }
        public string code { get; set; }
        public bool is_editable { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
        public string method { get; set; }
    }

    //public class Links
    //{
    //    public Self self { get; set; }
    //}

    //public class DatePattern
    //{
    //    public string date { get; set; }
    //    public string time { get; set; }
    //    public string date_time { get; set; }
    //    public string time_full { get; set; }
    //}

    //public class Embedded
    //{
    //    public TaskTypes task_types { get; set; }
    //}

    //public class RootObject
    //{
    //    public Links _links { get; set; }
    //    public int id { get; set; }
    //    public string name { get; set; }
    //    public string subdomain { get; set; }
    //    public string currency { get; set; }
    //    public string timezone { get; set; }
    //    public string timezone_offset { get; set; }
    //    public string language { get; set; }
    //    public DatePattern date_pattern { get; set; }
    //    public int current_user { get; set; }
    //    public Embedded _embedded { get; set; }
    //}

    public class CallParams
    {
        public string UNIQ { get; set; }
        public string LINK { get; set; }
        public string PHONE { get; set; }
        public int DURATION { get; set; }
        public string @from { get; set; }
        public string extinfo { get; set; }
        public string SRC { get; set; }
    }

    public class CallNote
    {
        public int id { get; set; }
        public int responsible_user_id { get; set; }
        public int created_by { get; set; }
        public int created_at { get; set; }
        public int updated_at { get; set; }
        public int account_id { get; set; }
        public int group_id { get; set; }
        public bool is_editable { get; set; }
        public int element_id { get; set; }
        public int element_type { get; set; }
        public string attachment { get; set; }
        public int note_type { get; set; }
        public string text { get; set; }
        public CallParams @params { get; set; }
        //public Links2 _links { get; set; }
    }

    public class EmbeddedCalls
    {
        public List<CallNote> items { get; set; }
    }

    public class CallsNoteResponceRoot
    {
        //public Links _links { get; set; }
        public EmbeddedCalls _embedded { get; set; }
    }

    public class Call
    {
        public int id { get; set; }
        public string Phone { get; set; }
        public int Duration { get; set; }

        public int responsible_user_id { get; set; }
        public int created_at { get; set; }
        public int element_id { get; set; }
        public bool income { get; set; }
    }

}
