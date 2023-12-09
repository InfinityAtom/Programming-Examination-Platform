using System.Text;

namespace Programming_Examination_Platform.Services;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Programming_Examination_Platform.Model;
using System.Net;
using System.Net.Mail;

public class StudentService
{
    private readonly JavaExamContext _context;

    public StudentService(JavaExamContext context)
    {
        _context = context;
    }

    public async System.Threading.Tasks.Task SaveBooking(Booking booking)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
    }

    public async Task<Booking> GetBookingByUserId(int userId)
    {
        return await _context.Bookings.FirstOrDefaultAsync(b => b.StudentId == userId);
    }

    public async Task<Model.Task> GetTaskByUserId(int userId)
    {
        return await _context.Tasks.FirstOrDefaultAsync(t => t.StudentId == userId);
    }
    
    public async Task<Studenti?> GetStudentById(int id)
    {
        return await _context.Studentis.FirstOrDefaultAsync(student => student.StudnetId == id);
    }
    public async Task<List<Model.Task>> GetStudentsWithTasksAboveGrade(int grade)
    {
        return await _context.Tasks.Include(t => t.Student)
            .Where(t => t.FinalGrade.HasValue && t.FinalGrade > grade)
            .ToListAsync();
    }
    private const string ICSContentTemplate = @"BEGIN:VCALENDAR
PRODID:-//YourCompanyName//YourProductName//EN
VERSION:2.0
BEGIN:VEVENT
UID:{0}
DTSTAMP:{1}
DTSTART:{2}
DTEND:{3}
SUMMARY:{4}
DESCRIPTION:{5}
LOCATION:{6}
END:VEVENT
END:VCALENDAR";

    private const string DateTimeFormat = "yyyyMMddTHHmmssZ";
    
    public async System.Threading.Tasks.Task SendConfirmationEmail(Studenti student, string examName, DateTime examDate)
    {
        try
        {
            var fromAddress = new MailAddress("java.exam@infinity-atom.ro", "Programming Examination Platform");
            var toAddress = new MailAddress(student.Email, $"{student.FirstName} {student.LastName}");
            string subject = $"You are confirmed to take the {examName}";
            string body = $"Hello, {student.FirstName} {student.LastName}\n\n" +
                          $"You selected to take your {examName} exam on {examDate:dd/MM/yyyy}.\n\n" +
                          "Please note that is your professor's responsibility to inform you about the hour.\n" +
                          "We are not responsible for any inconveniences that may appear in the process of coming to your exam.\n\n" +
                          "Thank you for testing with us,\n" +
                          "--\nThe Infinity Atom Team\n" +
                          "Programming Examination Platform\n" +
                          "Developer: Velicea Fabian Pavel\n" +
                          "fabian.velicea@student.unitbv.ro\n" +
                          "--\n" +
                          "Universitatea Transilvania din Brașov,\n" +
                          "Facultatea de Inginerie Electrică și Știința Calculatoarelor";

            var smtp = new SmtpClient
            {
                Host = "mail.infinity-atom.ro",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, "p@SSWORD20caractereabc1")
            };

            using (var message = new MailMessage(fromAddress, toAddress)
                   {
                       Subject = subject,
                       Body = body
                   })
            {
                // Generate ICS content
                DateTime examEndDate = examDate.AddHours(1); // Assuming 1 hour for exam duration. Adjust as needed.
                
                string icsContent = string.Format(ICSContentTemplate,
                    Guid.NewGuid(),
                    DateTime.UtcNow.ToString(DateTimeFormat),
                    examDate.ToString(DateTimeFormat),
                    examEndDate.ToString(DateTimeFormat),
                    $"Exam: {examName}",
                    $"You are scheduled to take the {examName}.",
                    "Exam location: UniTBv, IESC, Politehnicii Street Number 1, N Building, Floor: 2, Class: 3 (NII3)" ); // Update with actual location if available

                // Attach ICS content as file to email
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(icsContent)))
                {
                    var attachment = new Attachment(stream, "exam-schedule.ics", "text/calendar");
                    message.Attachments.Add(attachment);

                    await smtp.SendMailAsync(message);
                }
            }
        }
        
        catch (Exception ex)
        {
            // Handle exceptions appropriately in your project, for example, log the exception.
            Console.WriteLine(ex.Message);
        }
        
    }
    public async Task<List<DateTime>> GetAvailableExamDatesForStudent(int studentId)
    {
        // Fetch the ProctorId for the student
        var proctorId = _context.Studentis.FirstOrDefault(s => s.StudnetId == studentId)?.ProctorId;

        // Return an empty list if ProctorId is null
        if (proctorId == null) return new List<DateTime>();

        // Fetch the available dates based on the ProctorId and AvailablePlaces
        return await _context.ExamSchedules
            .Where(es => es.AvailablePlaces >= 1 && es.ProctorId == proctorId.Value)
            .OrderBy(es => es.Date)  // Order by date for getting the earliest date first
            .Select(es => es.Date.Value)
            .ToListAsync();
    }
    public async Task<int?> GetAvailablePlacesByExamScheduleId(int examScheduleId)
    {
        var examSchedule = await _context.ExamSchedules.FirstOrDefaultAsync(es => es.ExamScheduleId == examScheduleId);
        Console.WriteLine($"Exam Schedule: {examSchedule?.ExamScheduleId}, Available Places: {examSchedule?.AvailablePlaces}");
        return examSchedule?.AvailablePlaces;
    }
    public async Task<string?> GetRoomNameByExamScheduleId(int examScheduleId)
    {
        var examSchedule = await _context.ExamSchedules.FirstOrDefaultAsync(es => es.ExamScheduleId == examScheduleId);
        Console.WriteLine($"Exam Schedule: {examSchedule?.ExamScheduleId}, Available Places: {examSchedule?.AvailablePlaces}");
        return examSchedule?.RoomName;
    }
    public async System.Threading.Tasks.Task UpdateExamSchedule(ExamSchedule examSchedule)
    {
        _context.Entry(examSchedule).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    public async Task<ExamSchedule?> GetExamScheduleById(int examScheduleId)
    {
        return await _context.ExamSchedules.FirstOrDefaultAsync(es => es.ExamScheduleId == examScheduleId);
    }

    public async Task<ExamSchedule?> GetExamScheduleByDateAndProctorId(DateTime date, int proctorId)
    {
        Console.WriteLine($"Fetching ExamSchedule for Date: {date} and ProctorId: {proctorId}");
        var result = await _context.ExamSchedules
            .FirstOrDefaultAsync(es => es.Date == date 
                                       && es.ProctorId == proctorId 
                                       && es.AvailablePlaces >= 1);
        Console.WriteLine($"Resulting ExamSchedule: {result?.ExamScheduleId}, Available Places: {result?.AvailablePlaces}");
        return result;
        
        
    }
    public async System.Threading.Tasks.Task DeleteTestRecords(int testId)
    {
        // Find the test with the given TestId
        var test = await _context.Tests.FindAsync(testId);
        if (test != null)
        {
            // Remove the test and its related test questions from the context
            _context.Tests.Remove(test);
            var testQuestions = _context.TestQuestions.Where(tq => tq.TestId == testId);
            _context.TestQuestions.RemoveRange(testQuestions);

            // Save the changes to the database
            await _context.SaveChangesAsync();
        }
    }
    public async System.Threading.Tasks.Task DeleteTestQuestionsByTestId(int testId)
    {
        var testQuestions = _context.TestQuestions.Where(q => q.TestId == testId);
        _context.TestQuestions.RemoveRange(testQuestions);
        await _context.SaveChangesAsync();
    }
    public async Task<List<ExamSchedule>> GetExamScheduleByProctorId(int proctorId)
    {
        return await _context.ExamSchedules
            .Where(schedule => schedule.ProctorId == proctorId)
            .ToListAsync();
    }

    public async Task<Test> CreateTestWithQuestions(int studentId)
    {
        // Create and save a new test
        var test = new Test
        {
            StudentId = studentId,
            DateTaken = DateTime.Now
        };

        _context.Tests.Add(test);
        await _context.SaveChangesAsync();

        // Get 15 random questions from the QuestionBank
        var randomQuestions = await _context.QuestionBanks
            .OrderBy(q => Guid.NewGuid())
            .Take(30)
            .ToListAsync();

        // Create and save new test questions
        foreach (var question in randomQuestions)
        {
            var testQuestion = new TestQuestion
            {
                TestId = test.TestId,
                QuestionId = question.QuestionId,
                StudentAnswer = null // Initially null
            };
            _context.TestQuestions.Add(testQuestion);
        }

        await _context.SaveChangesAsync();

        return test;
    }

    public async Task<TestQuestion> GetNextTestQuestion(int testId, int currentQuestionIndex)
    {
        // Assuming TestQuestions are ordered by their ID or another sequential identifier
        return await _context.TestQuestions
            .Where(tq => tq.TestId == testId)
            .OrderBy(tq => tq.TestQuestionId)
            .Skip(currentQuestionIndex)
            .FirstOrDefaultAsync();
    }
    public async System.Threading.Tasks.Task UpdateStudentAnswer(int testQuestionId, string studentAnswer)
    {
        var testQuestion = await _context.TestQuestions.FindAsync(testQuestionId);
    
        if (testQuestion != null)
        {
            testQuestion.StudentAnswer = studentAnswer;
            await _context.SaveChangesAsync();
        }
    }


}