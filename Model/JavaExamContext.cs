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
            entity.Property(e => e.ProctorId).HasColumnName("ProctorID");
            entity.Property(e => e.SpecializationId).HasColumnName("SpecializationID");
            entity.Property(e => e.Year).HasMaxLength(50);

            entity.HasOne(d => d.Proctor).WithMany(p => p.Studentis)
                .HasForeignKey(d => d.ProctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Studenti_Proctor");

            entity.HasOne(d => d.Specialization).WithMany(p => p.Studentis)
                .HasForeignKey(d => d.SpecializationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Studenti_Specializations");
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
