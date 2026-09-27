using Microsoft.EntityFrameworkCore;
using SkillMentor.entities;
using Session = SkillMentor.entities.Session;
namespace SkillMentor.respositories;
public class SkillMentorDbContext(DbContextOptions<SkillMentorDbContext> options) : DbContext(options)
{
    public DbSet<Mentor> Mentors => Set<Mentor>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<Session> Sessions => Set<Session>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        var m = model.Entity<Mentor>();
        m.ToTable("mentor"); m.HasKey(x => x.Id); m.Property(x => x.Id).HasColumnName("id").UseIdentityByDefaultColumn();
        m.Property(x => x.MentorId).HasColumnName("mentor_id").HasMaxLength(100).IsRequired();
        m.Property(x => x.FirstName).HasColumnName("first_name").HasMaxLength(50).IsRequired();
        m.Property(x => x.LastName).HasColumnName("last_name").HasMaxLength(50).IsRequired();
        m.Property(x => x.Email).HasColumnName("email").HasMaxLength(100).IsRequired(); m.HasIndex(x => x.Email).IsUnique();
        m.Property(x => x.PhoneNumber).HasColumnName("phone_number").HasMaxLength(20);
        m.Property(x => x.Title).HasColumnName("title"); m.Property(x => x.Profession).HasColumnName("profession"); m.Property(x => x.Company).HasColumnName("company");
        m.Property(x => x.ExperienceYears).HasColumnName("experience_years"); m.Property(x => x.Bio).HasColumnName("bio").HasColumnType("text");
        m.Property(x => x.ProfileImageUrl).HasColumnName("profile_image_url"); m.Property(x => x.PositiveReviews).HasColumnName("positive_reviews");
        m.Property(x => x.TotalEnrollments).HasColumnName("total_enrollments"); m.Property(x => x.IsCertified).HasColumnName("is_certified");
        m.Property(x => x.StartYear).HasColumnName("start_year").HasMaxLength(10);
        m.Property(x => x.CreatedAt).HasColumnName("created_at"); m.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        var st = model.Entity<Student>();
        st.ToTable("student"); st.HasKey(x => x.Id); st.Property(x => x.Id).HasColumnName("id").UseIdentityByDefaultColumn();
        st.Property(x => x.StudentId).HasColumnName("student_id").HasMaxLength(100).IsRequired();
        st.Property(x => x.Email).HasColumnName("email").HasMaxLength(100).IsRequired(); st.HasIndex(x => x.Email).IsUnique();
        st.Property(x => x.FirstName).HasColumnName("first_name").HasMaxLength(50).IsRequired();
        st.Property(x => x.LastName).HasColumnName("last_name").HasMaxLength(50).IsRequired();
        st.Property(x => x.LearningGoals).HasColumnName("learning_goals").HasColumnType("text");
        st.Property(x => x.CreatedAt).HasColumnName("created_at"); st.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        var sub = model.Entity<Subject>();
        sub.ToTable("subject"); sub.HasKey(x => x.Id); sub.Property(x => x.Id).HasColumnName("id").UseIdentityByDefaultColumn();
        sub.Property(x => x.SubjectName).HasColumnName("subject_name").HasMaxLength(255).IsRequired();
        sub.Property(x => x.Description).HasColumnName("description").IsRequired(); sub.Property(x => x.CourseImageUrl).HasColumnName("course_image_url");
        sub.Property(x => x.MentorDbId).HasColumnName("mentor_id"); sub.HasOne(x => x.Mentor).WithMany(x => x.Subjects).HasForeignKey(x => x.MentorDbId);
        sub.Property(x => x.CreatedAt).HasColumnName("created_at"); sub.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        var s = model.Entity<Session>();
        s.ToTable("session"); s.HasKey(x => x.Id); s.Property(x => x.Id).HasColumnName("id").UseIdentityByDefaultColumn();
        s.Property(x => x.StudentDbId).HasColumnName("student_id"); s.HasOne(x => x.Student).WithMany(x => x.Sessions).HasForeignKey(x => x.StudentDbId);
        s.Property(x => x.MentorDbId).HasColumnName("mentor_id"); s.HasOne(x => x.Mentor).WithMany(x => x.Sessions).HasForeignKey(x => x.MentorDbId);
        s.Property(x => x.SubjectDbId).HasColumnName("subject_id"); s.HasOne(x => x.Subject).WithMany().HasForeignKey(x => x.SubjectDbId);
        s.Property(x => x.SessionAt).HasColumnName("session_at"); s.Property(x => x.DurationMinutes).HasColumnName("duration_minutes");
        s.Property(x => x.SessionStatus).HasColumnName("session_status").HasMaxLength(50);
        s.Property(x => x.MeetingLink).HasColumnName("meeting_link"); s.Property(x => x.SessionNotes).HasColumnName("session_notes").HasColumnType("text");
        s.Property(x => x.StudentReview).HasColumnName("student_review").HasColumnType("text"); s.Property(x => x.StudentRating).HasColumnName("student_rating");
        s.Property(x => x.PaymentStatus).HasColumnName("payment_status").HasMaxLength(20);
        s.Property(x => x.CreatedAt).HasColumnName("created_at"); s.Property(x => x.UpdatedAt).HasColumnName("updated_at");
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries().Where(x => x.Entity is Mentor or Student or Subject or Session))
        {
            if (entry.State == EntityState.Added) entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
            if (entry.State == EntityState.Modified) entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
}
