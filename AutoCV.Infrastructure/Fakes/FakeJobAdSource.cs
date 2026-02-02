namespace AutoCV.Web.Fakes
{
    using AutoCV.Contracts.Dtos;
    using AutoCV.Contracts.Interfaces;

    public class FakeJobAdSource : IJobAdSource
    {
        public Task<JobAdDto> LoadAsync(string jobAdId)
        {
            return Task.FromResult(new JobAdDto
            {
                Id = jobAdId,
                Title = "Senior Software Engineer",
                Company = "Acme Corp",
                DescriptionMarkdown = "# Senior Software Engineer\n\nWe need C# and Docker."
            });
        }
    }

}
