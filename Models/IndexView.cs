using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace myCircle.Models{
    public class IndexView{
        public users user{get;set;}
        public List<usercircles> usercircles = new List<usercircles>();


    }
}