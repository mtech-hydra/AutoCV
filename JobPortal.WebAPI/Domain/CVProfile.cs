public class CVProfile : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Title { get; private set; }
    public string Summary { get; private set; }
    public string Skills { get; private set; }
    public string Experience { get; private set; }
    public string Education { get; private set; }

    public CVProfile() { }

    public CVProfile(Guid userId, string title, string summary, string skills, string experience, string education)
    {
        UserId = userId;
        Title = title;
        Summary = summary;
        Skills = skills;
        Experience = experience;
        Education = education;
    }
}