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
    
    public partial class Article
    {
        public int ArticleID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Nullable<int> ImageID { get; set; }
        public string Abstract { get; set; }
        public Nullable<int> TopicID { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public Nullable<int> Priority { get; set; }
        public Nullable<int> Visits { get; set; }
        public Nullable<int> Status { get; set; }
        public string Links { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Tags { get; set; }
        public string KeyWords { get; set; }
    }
}