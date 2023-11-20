

namespace Efolio.Models
{
    public class Contacts
    {
       
        public string userid { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
        public DateTime datecreated { get; set; } = DateTime.Now;
    }
}