using Microsoft.EntityFrameworkCore;
using Programming_Examination_Platform.Model;
using Task = System.Threading.Tasks.Task;

namespace Programming_Examination_Platform.Services;

public class ProctorService
{
  private readonly JavaExamContext _context;

  public ProctorService(JavaExamContext context)
  {
    _context = context;
  }

  public async Task<Proctor?> GetProctorById(int id)
  {
    return await _context.Proctors.FirstOrDefaultAsync(proctor => proctor.ProctorId == id);
  }

  public async Task<List<Studenti>> GetStudentsByProctorId(int proctorId)
  {
    return await _context.Studentis
      .Where(student => student.ProctorId == proctorId)
      .ToListAsync();
  }

  public async Task<List<ExamSchedule>> GetExamScheduleByProctorId(int proctorId)
  {
    return await _context.ExamSchedules
      .Where(schedule => schedule.ProctorId == proctorId)
      .ToListAsync();
  }

  public async Task<bool> DeleteExamScheduleById(int examScheduleId)
  {
    // 1. Find all associated bookings.
    var associatedBookings = _context.Bookings.Where(b => b.ExamScheduleId == examScheduleId).ToList();

    // 2. For each booking, find and delete all associated tasks.
    foreach (var booking in associatedBookings)
    {
      var associatedTasks = _context.Tasks.Where(t => t.BookingId == booking.BookingId).ToList();
      if (associatedTasks.Any())
      {
        _context.Tasks.RemoveRange(associatedTasks);
      }
    }

    // 3. Delete the bookings.
    if (associatedBookings.Any())
    {
      _context.Bookings.RemoveRange(associatedBookings);
    }

    // 4. Find and delete the ExamSchedule.
    var examSchedule = await _context.ExamSchedules.FindAsync(examScheduleId);
    if (examSchedule == null)
    {
      return false; // or handle it differently
    }

    _context.ExamSchedules.Remove(examSchedule);

    // 5. Save changes.
    try
    {
      await _context.SaveChangesAsync();
      return true;
    }
    catch (Exception e)
    {
      // Log the exception or handle it appropriately.
      return false;
    }
  }

  public async Task<List<StudentExamDetails>> GetStudentExamDetails(int proctorId)
  {
    var tasksForProctor = await _context.Tasks.Where(t => t.ProctorId == proctorId).ToListAsync();
    var detailsList = new List<StudentExamDetails>();

    foreach (var task in tasksForProctor)
    {
      var student = await _context.Studentis.FirstOrDefaultAsync(s => s.StudnetId == task.StudentId);
      var exam = await _context.Exams.FirstOrDefaultAsync(e => e.ExamId == task.ExamId);
      var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == task.BookingId);
      
      var details = new StudentExamDetails
      {
        FirstName = student.FirstName,
        LastName = student.LastName,
        Email = student.Email,
        ExamName = exam.ExamName,
        BookingDate = booking.BookingDate,
        FinalGrade = task.FinalGrade ?? 0
      };
        
      detailsList.Add(details);
    }

    return detailsList;
  }








  private bool IsStudentLoggedIn(int studentId)
  {
    if (studentId != null)
    {
      return false;
    }
    else return false;
  }

  public async Task<bool> UpdateExamSchedule(int bookingId, string roomName, int addedValue)
  {
    var existingSchedule = await _context.ExamSchedules.FindAsync(bookingId);
    if (existingSchedule == null)
    {
      return false; // or you can handle it differently, such as throwing an exception
    }

    // Update only the specified fields:
    existingSchedule.RoomName = roomName;
    existingSchedule.TotalPlaces += addedValue;
    existingSchedule.AvailablePlaces += addedValue;

    try
    {
      await _context.SaveChangesAsync();
      return true;
    }
    catch (Exception e)
    {
      // Log the exception or handle it appropriately
      return false;
    }
  }


  public async Task<List<Studenti>> GetStudentsByExamScheduleId(int examScheduleId)
  {
    // Fetch all bookings with the provided ExamScheduleId
    var bookings = await _context.Bookings
      .Include(b => b.Student) // Include the associated student
      .Where(b => b.ExamScheduleId == examScheduleId)
      .ToListAsync();

    // Extract students from the bookings
    var students = bookings.Select(b => b.Student).ToList();

    return students;
  }

  public async Task<bool> AddExamSchedule(ExamSchedule examSchedule)
  {
    try
    {
      _context.ExamSchedules.Add(examSchedule);
      await _context.SaveChangesAsync();
      return true;
    }
    catch (Exception e)
    {
      // Log the exception or handle it appropriately
      return false;
    }
  }

  public async Task<string?> GetSpecializationNameByStudentId(int studentId)
  {
    var student = await _context.Studentis
      .Include(s => s.Specialization)
      .FirstOrDefaultAsync(s => s.StudnetId == studentId);

    return student?.Specialization?.SpecializationName;
  }
}