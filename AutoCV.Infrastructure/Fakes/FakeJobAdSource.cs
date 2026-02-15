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
                Description = "# Senior Software Engineer\n\nWe need C# and Docker."
            });
        }
    }

}
