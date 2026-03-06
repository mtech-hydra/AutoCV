public class JobApplication : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid JobAdId { get; private set; }
    public Guid CVProfileId { get; private set; }
    public Guid CoverLetterId { get; private set; }
    public JobApplicationStatus Status { get; set; }

    public JobApplication() { }

    public JobApplication(Guid userId, Guid jobAdId, Guid cvProfileId, Guid coverLetterId)
    {
        UserId = userId;
        JobAdId = jobAdId;
        CVProfileId = cvProfileId;
        CoverLetterId = coverLetterId;
        Status = JobApplicationStatus.Pending;
    }
}