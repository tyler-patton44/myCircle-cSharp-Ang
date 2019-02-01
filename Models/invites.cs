using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace myCircle.Models
{
    public class invites
    {
        [Key]
        public int inviteId{get;set;}
        public int userId{get;set;}
        [NotMapped]
        public users User{get;set;}
        public int circleId{get;set;}
        public circles circle{get;set;}
    }
}