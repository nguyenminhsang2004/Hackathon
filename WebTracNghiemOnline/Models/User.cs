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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.Join_Quiz = new HashSet<Join_Quiz>();
        }
    
        public int ID_User { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> Date_of_Birth { get; set; }
        public Nullable<bool> Sex { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Nullable<int> Permisson { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Join_Quiz> Join_Quiz { get; set; }
        public virtual User_Admin User_Admin { get; set; }
        public virtual User_Answer User_Answer { get; set; }
        public virtual User_Ask User_Ask { get; set; }
    }
}