using System;
using System.Collections.Generic;

namespace Programming_Examination_Platform.Model;

public partial class Task
{
    public int Id { get; set; }

    public int ProctorId { get; set; }

    public int BookingId { get; set; }

    public int ExamId { get; set; }

    public int StudentId { get; set; }

    public string? Task1Content { get; set; }

    public string? Task1State { get; set; }

    public string? Task1Explanation { get; set; }

    public string? Task2Content { get; set; }

    public string? Task2State { get; set; }

    public string? Task2Explanation { get; set; }

    public string? Task3Content { get; set; }

    public string? Task3State { get; set; }

    public string? Task3Explanation { get; set; }

    public string? Task4Content { get; set; }

    public string? Task4State { get; set; }

    public string? Task4Explanation { get; set; }

    public int? FinalGrade { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Exam Exam { get; set; } = null!;

    public virtual Proctor Proctor { get; set; } = null!;

    public virtual Studenti Student { get; set; } = null!;
}
