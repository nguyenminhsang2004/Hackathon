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
    
    public partial class Quiz
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Quiz()
        {
            this.Join_Quiz = new HashSet<Join_Quiz>();
            this.Questions = new HashSet<Question>();
        }
    
        public int ID_Quiz { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> Time_Start { get; set; }
        public Nullable<System.DateTime> Time_Finish { get; set; }
        public Nullable<System.TimeSpan> Time { get; set; }
        public Nullable<int> Allow_Attemp { get; set; }
        public string Keys { get; set; }
        public Nullable<int> Creator { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Join_Quiz> Join_Quiz { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Question> Questions { get; set; }
        public virtual User_Ask User_Ask { get; set; }
    }
}
