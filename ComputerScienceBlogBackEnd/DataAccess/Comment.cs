using System;

namespace ComputerScienceBlogBackEnd.DataAccess
{
    public class Comment
    {
        public string Text { get; set; }

        public DateTime Date { get; set; }

        public string CreatedUser { get; set; }
    }
}
