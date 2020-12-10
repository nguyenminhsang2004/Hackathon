using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebTracNghiemOnline.Models
{
    public class AnswerViewModel
    {
        public int ID { get; set; }
        public string Answer { get; set; }
        public bool Correct_Answer { get; set; }
    }
}