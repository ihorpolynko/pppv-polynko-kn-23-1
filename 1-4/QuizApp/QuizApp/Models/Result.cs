using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Models
{
    public class Result
    {
        public int ResultId { get; set; }
        public int UserId { get; set; }
        public int QuizId { get; set; }
        public int Score { get; set; }
        public DateTime DateTaken { get; set; }

        public string UserLogin { get; set; }
        public string QuizTitle { get; set; }
    }
}
