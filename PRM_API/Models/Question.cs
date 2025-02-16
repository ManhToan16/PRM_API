using System;
using System.Collections.Generic;

namespace PRM_API.Models
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        public int Id { get; set; }
        public string Question1 { get; set; } = null!;
        public int Status { get; set; }
        public int ExerciseId { get; set; }
        public int Mark { get; set; }

        public virtual Exercise Exercise { get; set; } = null!;
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
