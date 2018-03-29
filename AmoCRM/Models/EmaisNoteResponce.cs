using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmoCRM.Models
{
    public class EmailParams
    {
        public string thread_id { get; set; }
        public string message_id { get; set; }
        public bool @private { get; set; }
        public bool income { get; set; }
        public From from { get; set; }
        public To to { get; set; }
        public int version { get; set; }
        public string subject { get; set; }
        public string content_summary { get; set; }
        public string contentSummary { get; set; }
        public int attach_cnt { get; set; }
        public Delivery delivery { get; set; }
    }

    public class From
    {
        public string email { get; set; }
        public string name { get; set; }
    }

    public class To
    {
        public string email { get; set; }
        public string name { get; set; }
    }

    public class Delivery
    {
        public string status { get; set; }
        public int time { get; set; }
    }

    public class EmailNote
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
        public EmailParams @params { get; set; }
        //public Links2 _links { get; set; }
    }

    public class EmbeddedEmails
    {
        public List<EmailNote> items { get; set; }
    }

    public class EmailsNoteResponceRoot
    {
        //public Links _links { get; set; }
        public EmbeddedEmails _embedded { get; set; }
    }

    public class Email
    {
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }

        public int id { get; set; }
        public int responsible_user_id { get; set; }
        public int created_at { get; set; }
        public int element_id { get; set; }
        public bool income { get; set; }
    }
}
