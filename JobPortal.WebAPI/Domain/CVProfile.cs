public class CVProfile : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Title { get; private set; }
    public string Summary { get; private set; }
    public string Skills { get; private set; }
    public string Experience { get; private set; }
    public string Education { get; private set; }

    public CVProfile() { }

    public CVProfile(CreateCVRequest request)
    {
        UserId = Guid.NewGuid();
        Title = request.Title;
        Summary = request.Summary;
        Skills = request.Skills;
        Experience = request.Experience;
        Education = request.Education;
    }

    public CVProfile(Guid userId, string title, string summary, string skills, string experience, string education)
    {
        UserId = userId;
        Title = title;
        Summary = summary;
        Skills = skills;
        Experience = experience;
        Education = education;
    }

    public void Update(string? title, string? summary, string? skills, string? experience, string? education)
    {
        if (!string.IsNullOrWhiteSpace(title)) Title = title;
        if (!string.IsNullOrWhiteSpace(summary)) Summary = summary;
        if (skills != null) Skills = skills;
        if (experience != null) Experience = experience;
        if (education != null) Education = education;
    }

    public void setDeleted()
    {
               IsDeleted = true;
    }
}
