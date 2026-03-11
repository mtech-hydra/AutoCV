public class JobAd : BaseEntity
{
    public Guid UserId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string CompanyName { get; private set; }
    public string Location { get; private set; }
    public string SalaryRange { get; private set; }
    public bool IsActive { get; private set; }

    public JobAd() { }

    public JobAd(Guid userId, string title, string description, string companyName, string location, string salaryRange)
    {
        UserId = userId;
        Title = title;
        Description = description;
        CompanyName = companyName;
        Location = location;
        SalaryRange = salaryRange;
        IsActive = true;
    }

    public void Update(string? title, string? description, string? companyName, string? location, string? salaryRange, bool? isActive)
    {
        if (!string.IsNullOrWhiteSpace(title)) Title = title;
        if (!string.IsNullOrWhiteSpace(description)) Description = description;
        if (!string.IsNullOrWhiteSpace(companyName)) CompanyName = companyName;
        if (!string.IsNullOrWhiteSpace(location)) Location = location;
        if (!string.IsNullOrWhiteSpace(salaryRange)) SalaryRange = salaryRange;
        if (isActive.HasValue) IsActive = isActive.Value;
    }
    public void setDeleted()
    {
        IsDeleted = true;
        SetUpdated();
    }
}