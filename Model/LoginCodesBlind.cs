using System;
using System.Collections.Generic;

namespace Programming_Examination_Platform.Model;

public partial class LoginCodesBlind
{
    public int CodeId { get; set; }

    public int StudentId { get; set; }

    public string LoginCode { get; set; } = null!;

    public virtual Studenti Student { get; set; } = null!;
}
