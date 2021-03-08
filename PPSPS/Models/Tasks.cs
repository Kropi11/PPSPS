using System;

namespace PPSPS.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateTime Date_start { get; set; }
        public DateTime Date_end { get; set; }

    }
}