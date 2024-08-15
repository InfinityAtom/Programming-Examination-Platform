using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Programming_Examination_Platform.Model;

public partial class JavaExamContext : DbContext
{
    public JavaExamContext()
    {
    }

    public JavaExamContext(DbContextOptions<JavaExamContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<ExamSchedule> ExamSchedules { get; set; }

    public virtual DbSet<LoginCodesBlind> LoginCodesBlinds { get; set; }

    public virtual DbSet<Proctor> Proctors { get; set; }

    public virtual DbSet<QuestionBank> QuestionBanks { get; set; }

    public virtual DbSet<QuestionsBlind> QuestionsBlinds { get; set; }

    public virtual DbSet<Specialization> Specializations { get; set; }

    public virtual DbSet<Studenti> Studentis { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<TestQuestion> TestQuestions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\HotelMgmSystem;Database=db;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.BookingDate).HasColumnType("datetime");
            entity.Property(e => e.ExamId).HasColumnName("ExamID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Exam).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bookings_Exams");

            entity.HasOne(d => d.ExamSchedule).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ExamScheduleId)
                .HasConstraintName("FK__Bookings__ExamSc__22401542");

            entity.HasOne(d => d.Student).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bookings_Studenti");
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasIndex(e => e.ExamName, "IX_Exams").IsUnique();

            entity.Property(e => e.ExamId).HasColumnName("ExamID");
            entity.Property(e => e.ExamAuthor).HasMaxLength(50);
            entity.Property(e => e.ExamLength).HasMaxLength(50);
            entity.Property(e => e.ExamName).HasMaxLength(50);
            entity.Property(e => e.ExamProducer).HasMaxLength(50);
            entity.Property(e => e.ExamVersion).HasMaxLength(50);
        });

        modelBuilder.Entity<ExamSchedule>(entity =>
        {
            entity.HasKey(e => e.ExamScheduleId).HasName("PK__ExamSche__D03AF2C2314BA673");

            entity.ToTable("ExamSchedule");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.RoomName).HasMaxLength(255);

            entity.HasOne(d => d.Exam).WithMany(p => p.ExamSchedules)
                .HasForeignKey(d => d.ExamId)
                .HasConstraintName("FK__ExamSched__ExamI__1F63A897");

            entity.HasOne(d => d.Proctor).WithMany(p => p.ExamSchedules)
                .HasForeignKey(d => d.ProctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExamSchedule_Proctor");
        });

        modelBuilder.Entity<LoginCodesBlind>(entity =>
        {
            entity.HasKey(e => e.CodeId);

            entity.ToTable("LoginCodesBlind");

            entity.Property(e => e.CodeId).HasColumnName("CodeID");
            entity.Property(e => e.LoginCode)
                .HasMaxLength(64)
                .IsUnicode(false);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Student).WithMany(p => p.LoginCodesBlinds)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LoginCodesBlind_Studenti");
        });

        modelBuilder.Entity<Proctor>(entity =>
        {
            entity.ToTable("Proctor");

            entity.HasIndex(e => e.LastName, "IX_Proctor").IsUnique();

            entity.Property(e => e.ProctorId).HasColumnName("ProctorID");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(255);
        });

        modelBuilder.Entity<QuestionBank>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06F8C647BEE70");

            entity.ToTable("QuestionBank");

            entity.Property(e => e.QuestionId)
                .ValueGeneratedNever()
                .HasColumnName("QuestionID");
            entity.Property(e => e.AnswerA)
                .HasMaxLength(1024)
                .IsUnicode(false);
            entity.Property(e => e.AnswerB)
                .HasMaxLength(1024)
                .IsUnicode(false);
            entity.Property(e => e.AnswerC)
                .HasMaxLength(1024)
                .IsUnicode(false);
            entity.Property(e => e.AnswerD)
                .HasMaxLength(1024)
                .IsUnicode(false);
            entity.Property(e => e.CorrectAnswerLetter)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.QuestionText)
                .HasMaxLength(1024)
                .IsUnicode(false);
        });

        modelBuilder.Entity<QuestionsBlind>(entity =>
        {
            entity.HasKey(e => e.QuestionBlindId);

            entity.ToTable("QuestionsBlind");

            entity.Property(e => e.QuestionBlindId).HasColumnName("QuestionBlindID");
            entity.Property(e => e.Atext1).HasColumnName("AText1");
            entity.Property(e => e.Atext10).HasColumnName("AText10");
            entity.Property(e => e.Atext11).HasColumnName("AText11");
            entity.Property(e => e.Atext12).HasColumnName("AText12");
            entity.Property(e => e.Atext13).HasColumnName("AText13");
            entity.Property(e => e.Atext14).HasColumnName("AText14");
            entity.Property(e => e.Atext15).HasColumnName("AText15");
            entity.Property(e => e.Atext16).HasColumnName("AText16");
            entity.Property(e => e.Atext17).HasColumnName("AText17");
            entity.Property(e => e.Atext18).HasColumnName("AText18");
            entity.Property(e => e.Atext19).HasColumnName("AText19");
            entity.Property(e => e.Atext2).HasColumnName("AText2");
            entity.Property(e => e.Atext20).HasColumnName("AText20");
            entity.Property(e => e.Atext3).HasColumnName("AText3");
            entity.Property(e => e.Atext4).HasColumnName("AText4");
            entity.Property(e => e.Atext5).HasColumnName("AText5");
            entity.Property(e => e.Atext6).HasColumnName("AText6");
            entity.Property(e => e.Atext7).HasColumnName("AText7");
            entity.Property(e => e.Atext8).HasColumnName("AText8");
            entity.Property(e => e.Atext9).HasColumnName("AText9");
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.Btext1).HasColumnName("BText1");
            entity.Property(e => e.Btext10).HasColumnName("BText10");
            entity.Property(e => e.Btext11).HasColumnName("BText11");
            entity.Property(e => e.Btext12).HasColumnName("BText12");
            entity.Property(e => e.Btext13).HasColumnName("BText13");
            entity.Property(e => e.Btext14).HasColumnName("BText14");
            entity.Property(e => e.Btext15).HasColumnName("BText15");
            entity.Property(e => e.Btext16).HasColumnName("BText16");
            entity.Property(e => e.Btext17).HasColumnName("BText17");
            entity.Property(e => e.Btext18).HasColumnName("BText18");
            entity.Property(e => e.Btext19).HasColumnName("BText19");
            entity.Property(e => e.Btext2).HasColumnName("BText2");
            entity.Property(e => e.Btext20).HasColumnName("BText20");
            entity.Property(e => e.Btext3).HasColumnName("BText3");
            entity.Property(e => e.Btext4).HasColumnName("BText4");
            entity.Property(e => e.Btext5).HasColumnName("BText5");
            entity.Property(e => e.Btext6).HasColumnName("BText6");
            entity.Property(e => e.Btext7).HasColumnName("BText7");
            entity.Property(e => e.Btext8).HasColumnName("BText8");
            entity.Property(e => e.Btext9).HasColumnName("BText9");
            entity.Property(e => e.CorrectAnswer1)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CorrectAnswer10)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CorrectAnswer11)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CorrectAnswer12)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CorrectAnswer13)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CorrectAnswer14)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CorrectAnswer15)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CorrectAnswer16)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CorrectAnswer17)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CorrectAnswer18)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CorrectAnswer19)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CorrectAnswer2)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CorrectAnswer20)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CorrectAnswer3)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CorrectAnswer4)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CorrectAnswer5)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CorrectAnswer6)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CorrectAnswer7)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CorrectAnswer8)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CorrectAnswer9)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Ctext1).HasColumnName("CText1");
            entity.Property(e => e.Ctext10).HasColumnName("CText10");
            entity.Property(e => e.Ctext11).HasColumnName("CText11");
            entity.Property(e => e.Ctext12).HasColumnName("CText12");
            entity.Property(e => e.Ctext13).HasColumnName("CText13");
            entity.Property(e => e.Ctext14).HasColumnName("CText14");
            entity.Property(e => e.Ctext15).HasColumnName("CText15");
            entity.Property(e => e.Ctext16).HasColumnName("CText16");
            entity.Property(e => e.Ctext17).HasColumnName("CText17");
            entity.Property(e => e.Ctext18).HasColumnName("CText18");
            entity.Property(e => e.Ctext19).HasColumnName("CText19");
            entity.Property(e => e.Ctext2).HasColumnName("CText2");
            entity.Property(e => e.Ctext20).HasColumnName("CText20");
            entity.Property(e => e.Ctext3).HasColumnName("CText3");
            entity.Property(e => e.Ctext4).HasColumnName("CText4");
            entity.Property(e => e.Ctext5).HasColumnName("CText5");
            entity.Property(e => e.Ctext6).HasColumnName("CText6");
            entity.Property(e => e.Ctext7).HasColumnName("CText7");
            entity.Property(e => e.Ctext8).HasColumnName("CText8");
            entity.Property(e => e.Ctext9).HasColumnName("CText9");
            entity.Property(e => e.Dtext1).HasColumnName("DText1");
            entity.Property(e => e.Dtext10).HasColumnName("DText10");
            entity.Property(e => e.Dtext11).HasColumnName("DText11");
            entity.Property(e => e.Dtext12).HasColumnName("DText12");
            entity.Property(e => e.Dtext13).HasColumnName("DText13");
            entity.Property(e => e.Dtext14).HasColumnName("DText14");
            entity.Property(e => e.Dtext15).HasColumnName("DText15");
            entity.Property(e => e.Dtext16).HasColumnName("DText16");
            entity.Property(e => e.Dtext17).HasColumnName("DText17");
            entity.Property(e => e.Dtext18).HasColumnName("DText18");
            entity.Property(e => e.Dtext19).HasColumnName("DText19");
            entity.Property(e => e.Dtext2).HasColumnName("DText2");
            entity.Property(e => e.Dtext20).HasColumnName("DText20");
            entity.Property(e => e.Dtext3).HasColumnName("DText3");
            entity.Property(e => e.Dtext4).HasColumnName("DText4");
            entity.Property(e => e.Dtext5).HasColumnName("DText5");
            entity.Property(e => e.Dtext6).HasColumnName("DText6");
            entity.Property(e => e.Dtext7).HasColumnName("DText7");
            entity.Property(e => e.Dtext8).HasColumnName("DText8");
            entity.Property(e => e.Dtext9).HasColumnName("DText9");
            entity.Property(e => e.ExamId).HasColumnName("ExamID");
            entity.Property(e => e.ProctorId).HasColumnName("ProctorID");
            entity.Property(e => e.SelectedAnswer1)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.SelectedAnswer10)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.SelectedAnswer11)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.SelectedAnswer12)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.SelectedAnswer13)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.SelectedAnswer14)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.SelectedAnswer15)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.SelectedAnswer16)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.SelectedAnswer17)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.SelectedAnswer18)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.SelectedAnswer19)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.SelectedAnswer2)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.SelectedAnswer20)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.SelectedAnswer3)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.SelectedAnswer4)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.SelectedAnswer5)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.SelectedAnswer6)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.SelectedAnswer7)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.SelectedAnswer8)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.SelectedAnswer9)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Booking).WithMany(p => p.QuestionsBlinds)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionsBlind_Bookings");

            entity.HasOne(d => d.Exam).WithMany(p => p.QuestionsBlinds)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionsBlind_Exams");

            entity.HasOne(d => d.Proctor).WithMany(p => p.QuestionsBlinds)
                .HasForeignKey(d => d.ProctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionsBlind_Proctor");

            entity.HasOne(d => d.Student).WithMany(p => p.QuestionsBlinds)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionsBlind_Studenti");
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasIndex(e => e.SpecializationName, "IX_Specializations").IsUnique();

            entity.Property(e => e.SpecializationId).HasColumnName("SpecializationID");
            entity.Property(e => e.SpecializationName).HasMaxLength(50);
        });

        modelBuilder.Entity<Studenti>(entity =>
        {
            entity.HasKey(e => e.StudnetId);

            entity.ToTable("Studenti");

            entity.HasIndex(e => e.Email, "IX_Studenti").IsUnique();

            entity.Property(e => e.StudnetId).HasColumnName("StudnetID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Faculty).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Groupa).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(64);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.PhoneNumberVerified).HasMaxLength(50);
            entity.Property(e => e.ProctorId).HasColumnName("ProctorID");
            entity.Property(e => e.SpecializationId).HasColumnName("SpecializationID");
            entity.Property(e => e.Year).HasMaxLength(50);
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookingId).HasColumnName("BookingID");
            entity.Property(e => e.ExamId).HasColumnName("ExamID");
            entity.Property(e => e.ProctorId).HasColumnName("ProctorID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.Task1State).HasMaxLength(50);
            entity.Property(e => e.Task2State).HasMaxLength(50);
            entity.Property(e => e.Task3State).HasMaxLength(50);
            entity.Property(e => e.Task4State).HasMaxLength(50);

            entity.HasOne(d => d.Booking).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_Bookings");

            entity.HasOne(d => d.Exam).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_Exams");

            entity.HasOne(d => d.Proctor).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ProctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_Proctor");

            entity.HasOne(d => d.Student).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_Studenti");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.TestId).HasName("PK__Test__8CC3310041D7D969");

            entity.ToTable("Test");

            entity.Property(e => e.TestId).HasColumnName("TestID");
            entity.Property(e => e.DateTaken).HasColumnType("datetime");
            entity.Property(e => e.Score).HasDefaultValueSql("((0))");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Student).WithMany(p => p.Tests)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Test__StudentID__44FF419A");
        });

        modelBuilder.Entity<TestQuestion>(entity =>
        {
            entity.HasKey(e => e.TestQuestionId).HasName("PK__TestQues__4C589E693011FCEE");

            entity.Property(e => e.TestQuestionId).HasColumnName("TestQuestionID");
            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.StudentAnswer)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TestId).HasColumnName("TestID");

            entity.HasOne(d => d.Question).WithMany(p => p.TestQuestions)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK__TestQuest__Quest__2FCF1A8A");

            entity.HasOne(d => d.Test).WithMany(p => p.TestQuestions)
                .HasForeignKey(d => d.TestId)
                .HasConstraintName("FK__TestQuest__TestI__46E78A0C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
