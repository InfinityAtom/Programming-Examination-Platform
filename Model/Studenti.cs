using System;
using System.Collections.Generic;

namespace Programming_Examination_Platform.Model;

public partial class Studenti
{
    public int StudnetId { get; set; }

    public int ProctorId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Faculty { get; set; } = null!;

    public int SpecializationId { get; set; }

    public string Year { get; set; } = null!;

    public string Groupa { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Proctor Proctor { get; set; } = null!;

    public virtual Specialization Specialization { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
