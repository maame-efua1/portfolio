﻿


namespace EFolio1.Models
{
    public class SignUp
    {
        
        public string userid { get; set; }
        
        public string firstname { get; set; }
        
        public string lastname { get; set; }
       
        public string username { get; set; }
       
        public string password { get; set; }
        
        public string confirmpassword { get; set; }

        public DateTime datecreated { get; set; } = DateTime.Now;

        public int usercount { get; set; }

    }
}
