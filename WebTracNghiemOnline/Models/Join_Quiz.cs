//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebTracNghiemOnline.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Join_Quiz
    {
        public int ID_User { get; set; }
        public int ID_Quiz { get; set; }
        public Nullable<int> Number_of_Times { get; set; }
        public Nullable<double> Score { get; set; }
    
        public virtual Quiz Quiz { get; set; }
        public virtual User User { get; set; }
    }
}