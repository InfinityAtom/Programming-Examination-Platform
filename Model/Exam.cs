using System;
using System.Collections.Generic;

namespace Programming_Examination_Platform.Model;

public partial class Exam
{
    public int ExamId { get; set; }

    public string? ExamName { get; set; }

    public string? ExamVersion { get; set; }

    public string? ExamLength { get; set; }

    public int? ExamTasks { get; set; }

    public int? ExamMaximumGrade { get; set; }

    public int? ExamPointsPerQuestion { get; set; }

    public string? ExamProducer { get; set; }

    public string? ExamAuthor { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<ExamSchedule> ExamSchedules { get; set; } = new List<ExamSchedule>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
