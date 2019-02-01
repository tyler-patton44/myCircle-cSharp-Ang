using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace myCircle.Models{

    public class channels{
        [Key]
        public int channelId{get;set;}

        [Required(ErrorMessage = "This field is required")]
        public string name{get;set;}
        public DateTime createdAt{get;set;}
        public int circleId{get;set;}
        [NotMapped]
        public circles circle{get;set;}
        public List<messages> messages{get;set;}

        
        public channels(){
            createdAt = DateTime.Now;
            messages = new List<messages>();
        }
    }
}