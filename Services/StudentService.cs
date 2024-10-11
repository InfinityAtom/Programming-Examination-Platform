using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Programming_Examination_Platform.Model;
using Task = System.Threading.Tasks.Task;

namespace Programming_Examination_Platform.Services;

public class StudentService
{
    private readonly JavaExamContext _context;

    public StudentService(JavaExamContext context)
    {
        _context = context;
    }

    public async Task SaveBooking(Booking booking)
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

    private const string DateTimeFormat = "yyyyMMddTHHmmssZ";
    
public async Task SendConfirmationEmail(Studenti student, string examName, DateTime examDate)
{
    try
    {
        var fromAddress = new MailAddress("java.exam@infinity-atom.ro", "Programming Examination Platform");
        var toAddress = new MailAddress(student.Email, $"{student.FirstName} {student.LastName}");
        string subject = $"You are confirmed to take the {examName}";

        // Load the header image and convert to base64 string
        string headerImageBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes("wwwroot/images/header.png"));
        string headerImageSrc = $"data:image/png;base64,{headerImageBase64}";

        // Construct the email body with HTML template
        string body = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        body {{
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f4f4f4;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            padding: 20px;
            border: 1px solid #dddddd;
            border-radius: 5px;
        }}
        .header {{
            background-color: rgba(89,74,226,1);
            padding: 10px;
            text-align: center;
            color: white;
        }}
        .header img {{
            max-width: 320px;
            height: auto;
        }}
        .content {{
            padding: 20px;
            line-height: 1.6;
        }}
        .content h1 {{
            color: #333333;
        }}
        .content p {{
            color: #666666;
        }}
        .rules {{
            margin-top: 20px;
            padding: 15px;
            background-color: #f9f9f9;
            border: 1px solid #dddddd;
            border-radius: 5px;
        }}
        .footer {{
            background-color: #333333;
            padding: 10px;
            text-align: center;
            color: white;
            font-size: 12px;
        }}
        .footer a {{
            color: #ffffff;
            text-decoration: none;
        }}
        .footer a:hover {{
            text-decoration: underline;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <img src='{headerImageSrc}' alt='Company Logo'>
        </div>
        <div class='content'>
            <h1>Exam Confirmation</h1>
            <p>Hello {student.FirstName} {student.LastName},</p>
            <p>Welcome to the Programming Examination Platform. You are confirmed to take the <strong>{examName}</strong> exam on <strong>{examDate:dd/MM/yyyy HH:mm}</strong>. Please be at the location provided 20 minutes before the exam to proceed with login and setup.</p>
            <h2>Exam Instructions</h2>
            <ul>
                <li>You will work on a Java project, executing four key requirements such as reading from a file into a Java collection, sorting objects, and working with threads.</li>
                <li>The exam environment is set up with IntelliJ IDEA (Process: idea64.exe). Ensure you are familiar with this IDE.</li>
                <li>Remember to frequently press <b>Save (CTRL+S)</b> to avoid any loss of progress.</li>
                <li>Only use <b>Restart Project</b> if absolutely necessary, as it will reset your entire project, leaving only the original files. This option should be used with caution.</li>
                <li>After completing your exam, save your project and close the IntelliJ window before pressing the Submit Project button. This is crucial to ensure your work is correctly submitted.</li>
            </ul>
            <h2>Exam Rules</h2>
            <div class='rules'>
                <h3>Prohibited During the Exam:</h3>
                <ul>
                    <li>Bringing in:
                        <ul>
                            <li>Cell phones</li>
                            <li>Laptops</li>
                            <li>Smart Watches</li>
                            <li>Headphones (wired or wireless, any type)</li>
                            <li>Papers</li>
                            <li>Writing devices (Pencils/Pens)</li>
                            <li>Tablets</li>
                        </ul>
                    </li>
                    <li>Accessing communication and social networks</li>
                    <li>Using any artificial intelligence programs that facilitate instant code writing</li>
                    <li>Communication between student and teacher or between students</li>
                    <li>Leaving the examination room before the expiry of the 45-minute time limit</li>
                </ul>
                <h3>Allowed During the Exam:</h3>
                <ul>
                    <li>Using the internet to search for useful information in solving the tasks</li>
                </ul>
                <p><strong>Failure to comply with any of these rules will result in disqualification from the examination, resulting in the awarding of the minimum mark in the Java exam. Retaking the exam will only be possible in autumn!</strong></p>
            </div>
            <p>Good luck with your exam! We are here to support you every step of the way.</p>
        </div>
        <div class='footer'>
            <p>&copy; 2024 Infinity Atom. All rights reserved.</p>
            <p><a href='#'>Privacy Policy</a> | <a href='#'>Terms of Service</a></p>
        </div>
    </div>
</body>
</html>";

        var smtp = new SmtpClient
        {
            Host = "mail.infinity-atom.ro",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, "p@SSWORD20caractereabc1")
        };

        using (var message = new MailMessage(fromAddress, toAddress))
        {
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true; // Important: Set the body as HTML

            // Manually calculate the UTC time by applying the offset for Romania
            TimeSpan utcOffset = new TimeSpan(3, 0, 0); // UTC+3 for daylight saving time
            DateTime examStartUtc = examDate - utcOffset;
            DateTime examEndDate = examDate.AddMinutes(90);
            DateTime examEndUtc = examEndDate - utcOffset;

            string icsContent = string.Format(IcsContentTemplate,
                Guid.NewGuid(),
                DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ"),
                examStartUtc.ToString("yyyyMMddTHHmmssZ"),
                examEndUtc.ToString("yyyyMMddTHHmmssZ"),
                $"Exam: {examName}",
                $"You are scheduled to take the {examName} exam.",
                "Exam location: UniTBv, IESC, Politehnicii Street Number 1, N Building, Floor: 2, Class: 3 (NII3)");

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
        Console.WriteLine($"Error sending email: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
        }
    }
}

public async Task SendSorryEmail(Studenti student, string examName)
{
    try
    {
        var fromAddress = new MailAddress("java.exam@infinity-atom.ro", "Programming Examination Platform");
        var toAddress = new MailAddress(student.Email, $"{student.FirstName} {student.LastName}");
        string subject = $"We noticed you've canceled your {examName} booking";

        // Load the header image and convert to base64 string
        string headerImageBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes("wwwroot/images/header.png"));
        string headerImageSrc = $"data:image/png;base64,{headerImageBase64}";

        // Construct the email body with HTML template
        string body = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        body {{
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f4f4f4;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            padding: 20px;
            border: 1px solid #dddddd;
            border-radius: 5px;
        }}
        .header {{
            background-color: rgba(89,74,226,1);
            padding: 10px;
            text-align: center;
            color: white;
        }}
        .header img {{
            max-width: 320px;
            height: auto;
        }}
        .content {{
            padding: 20px;
            line-height: 1.6;
        }}
        .content h1 {{
            color: #333333;
        }}
        .content p {{
            color: #666666;
        }}
        .rules {{
            margin-top: 20px;
            padding: 15px;
            background-color: #f9f9f9;
            border: 1px solid #dddddd;
            border-radius: 5px;
        }}
        .footer {{
            background-color: #333333;
            padding: 10px;
            text-align: center;
            color: white;
            font-size: 12px;
        }}
        .footer a {{
            color: #ffffff;
            text-decoration: none;
        }}
        .footer a:hover {{
            text-decoration: underline;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <img src='{headerImageSrc}' alt='Company Logo'>
        </div>
        <div class='content'>
            <h1>We're Here for You!</h1>
            <p>Hello {student.FirstName} {student.LastName},</p>
            <p>We noticed that you recently canceled your booking for the <strong>{examName}</strong> exam. We understand that sometimes unexpected things come up or plans change, and that's perfectly okay!</p>
            <p>If you couldn't make it to your exam this time, know that we're here to support you. Our goal is to help you succeed, and we’re more than happy to assist you in rescheduling. Your growth and progress matter to us, and we believe you have what it takes to excel.</p>
            <p>If you canceled by mistake or just had a change of heart, don’t worry! We encourage you to rebook at your earliest convenience. It’s never too late to get back on track and continue your journey toward achieving your goals.</p>
            <p>Remember, every step you take brings you closer to mastering your skills and achieving your dreams. We look forward to seeing you back soon!</p>
            <p><strong>Click the button below to easily rebook your exam and continue your learning journey with us:</strong></p>
            <p style='text-align: center;'>
                <a href='https://localhost:7037/booking/selectexam' style='background-color: #594ae2; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Rebook Your Exam</a>
            </p>
        </div>
        <div class='footer'>
            <p>&copy; 2024 Infinity Atom. All rights reserved.</p>
            <p><a href='#'>Privacy Policy</a> | <a href='#'>Terms of Service</a></p>
        </div>
    </div>
</body>
</html>";

        var smtp = new SmtpClient
        {
            Host = "mail.infinity-atom.ro",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, "p@SSWORD20caractereabc1")
        };

        using (var message = new MailMessage(fromAddress, toAddress))
        {
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true; // Important: Set the body as HTML

            // Send the email
            await smtp.SendMailAsync(message);  // This line was missing
        }
    }
    catch (Exception ex)
    {
        // Handle exceptions appropriately in your project, for example, log the exception.
        Console.WriteLine($"Error sending email: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
        }
    }
}




// ICS content template remains the same as defined earlier
private const string IcsContentTemplate = @"BEGIN:VCALENDAR
PRODID:-//Infinity Atom//Programming Examination Platform//EN
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
    public async Task<List<Specialization>> GetAllSpecializations()
    {
        return await _context.Specializations.ToListAsync();
    }
    public async Task UpdateStudentPassword(int studentId, string hashedNewPassword)
    {
        // Find the student in the database by their ID
        var student = await _context.Studentis.FindAsync(studentId);

        if (student != null)
        {
            // Update the password field with the new hashed password
            student.Password = hashedNewPassword;

            // Save changes to the database
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new Exception("Student not found.");
        }
    }
    public async Task UpdateStudent(Studenti student)
    {
        _context.Studentis.Update(student);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> DeleteBooking(int bookingId)
    {
        try
        {
            // Fetch the booking by ID
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == bookingId);
            if (booking != null)
            {
                // Check if there's an associated exam schedule
                var examSchedule = await _context.ExamSchedules.FirstOrDefaultAsync(es => es.ExamScheduleId == booking.ExamScheduleId);
                if (examSchedule != null)
                {
                    // Increment the number of available places
                    examSchedule.AvailablePlaces += 1;
                    _context.ExamSchedules.Update(examSchedule);
                }

                // Delete the booking
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
                return true;
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., log error)
            Console.WriteLine($"Error deleting booking: {ex.Message}");
        }

        return false;
    }

    
    public async Task<Studenti?> GetStudentByEmail(string email)
    {
        return await _context.Studentis.FirstOrDefaultAsync(s => s.Email == email);
    }
    public async Task<Specialization?> GetSpecializationByStudentId(int studentId)
    {
        // Retrieve the student by ID
        var student = await _context.Studentis.FirstOrDefaultAsync(s => s.StudnetId == studentId);
    
        // If student is found, retrieve the specialization
        if (student != null)
        {
            return await _context.Specializations
                .FirstOrDefaultAsync(sp => sp.SpecializationId == student.SpecializationId);
        }
    
        return null;
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
            .Select(es => es.Date!.Value)
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
    public async Task UpdateExamSchedule(ExamSchedule examSchedule)
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
    public async Task DeleteTestRecords(int testId)
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
    public async Task DeleteTestQuestionsByTestId(int testId)
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
            .FirstOrDefaultAsync() ?? throw new InvalidOperationException();
    }
    
    public async Task UpdateStudentAnswer(int testQuestionId, string studentAnswer)
    {
        var testQuestion = await _context.TestQuestions.FindAsync(testQuestionId);
    
        if (testQuestion != null)
        {
            testQuestion.StudentAnswer = studentAnswer;
            await _context.SaveChangesAsync();
        }
    }
}