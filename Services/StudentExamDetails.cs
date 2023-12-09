namespace Programming_Examination_Platform.Model
{
  public class StudentExamDetails
  {
    public int StudentId { get; set; } 
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string ExamName { get; set; }
    public DateTime BookingDate { get; set; }
    public int FinalGrade { get; set; }
    public string ExamStatus => FinalGrade >= 5 ? "Succeeded" : "Failed";
  }
}