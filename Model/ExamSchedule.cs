using System;
using System.Collections.Generic;

namespace Programming_Examination_Platform.Model;

public partial class ExamSchedule
{
    public int ExamScheduleId { get; set; }

    public int ProctorId { get; set; }

    public int? ExamId { get; set; }

    public DateTime? Date { get; set; }

    public string? RoomName { get; set; }

    public int? TotalPlaces { get; set; }

    public int? AvailablePlaces { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Exam? Exam { get; set; }

    public virtual Proctor Proctor { get; set; } = null!;
}
