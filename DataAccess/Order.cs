//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public int OrderID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsSeen { get; set; }
        public string MaxStartTime { get; set; }
        public string DeadLine { get; set; }
        public string State { get; set; }
        public string CIty { get; set; }
        public string Address { get; set; }
    }
}
