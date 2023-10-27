using System;
using System.Collections.Generic;

namespace Programming_Examination_Platform.Model;

public partial class Booking
{
    public int BookingId { get; set; }

    public int StudentId { get; set; }

    public int ExamId { get; set; }

    public DateTime BookingDate { get; set; }

    public int? ExamScheduleId { get; set; }

    public virtual Exam Exam { get; set; } = null!;

    public virtual ExamSchedule? ExamSchedule { get; set; }

    public virtual Studenti Student { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
