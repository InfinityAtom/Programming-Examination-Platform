using System;
using System.Collections.Generic;

namespace Programming_Examination_Platform.Model;

public partial class Test
{
    public int TestId { get; set; }

    public int? StudentId { get; set; }

    public DateTime? DateTaken { get; set; }

    public int? Score { get; set; }

    public virtual Studenti? Student { get; set; }

    public virtual ICollection<TestQuestion> TestQuestions { get; set; } = new List<TestQuestion>();
}
