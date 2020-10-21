using System;
using System.Collections.Generic;


namespace JobsAppAndroid.Models
{
    public class Job
    {

        public string Id { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public string Source { set; get; }
        public string Type { set; get; }
        public string Location { set; get; }
        public string Company { set; get; }
        public DateTime Closing { set; get; }
        public DateTime Posted { set; get; }
        public bool Liked { set; get; }
        public List<string> Contacts { set; get; }
        public List<string> Competencies { set; get; }
        public List<string> Qualifications { set; get; }
    }
}
       
