using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTracNghiemOnline.Models
{
    public class QuestionViewModel
    {
        public int ID_Question { get; set; }
        public string Content_of_Question { get; set; }
        public int Type_of_Question { get; set; }
        public double Score { get; set; }
    }
}