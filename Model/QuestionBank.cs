using System;
using System.Collections.Generic;

namespace Programming_Examination_Platform.Model;

public partial class QuestionBank
{
    public int QuestionId { get; set; }

    public string? QuestionText { get; set; }

    public string? AnswerA { get; set; }

    public string? AnswerB { get; set; }

    public string? AnswerC { get; set; }

    public string? AnswerD { get; set; }

    public string? CorrectAnswerLetter { get; set; }

    public virtual ICollection<TestQuestion> TestQuestions { get; set; } = new List<TestQuestion>();
}
