using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTracNghiemOnline.Models
{
    public class QuizViewModel
    {
        public string Keys { get; set; }
        public string Title { get; set; }
        public DateTime Time_Start { get; set; }
        public DateTime Time_Finish { get; set; }
        public TimeSpan Time { get; set; }
        public int Allow_Attemp { get; set; }
        public int Creator { get; set; }
    }
}