using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace myCircle.Models
{
    public class messagelikes
    {
        [Key]
        public int likeId{get;set;}
        public int userId{get;set;}
        [NotMapped]
        public users User{get;set;}
        public int messageId{get;set;}
        [NotMapped]
        public messages message{get;set;}
    }
}