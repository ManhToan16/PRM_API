using System;
using System.Collections.Generic;

namespace PRM_API.Models
{
    public partial class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Answer1 { get; set; } = null!;
        public int Status { get; set; }
        public bool IsCorrect { get; set; }

        public virtual Question Question { get; set; } = null!;
    }
}
