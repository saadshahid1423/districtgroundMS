//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DistrictPlayGroundManagementSystem
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ground
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ground()
        {
            this.Feedbacks = new HashSet<Feedback>();
            this.Bookings = new HashSet<Booking>();
        }
    
        public int Id { get; set; }
        public Nullable<int> GroundTypeID { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public string Picture { get; set; }
        public Nullable<int> Price { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual Ground Ground1 { get; set; }
        public virtual Ground Ground2 { get; set; }
        public virtual GroundType GroundType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
