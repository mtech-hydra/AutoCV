namespace AutoCV.Web.Fakes
{
    using AutoCV.Contracts.Dtos;
    using AutoCV.Contracts.Interfaces;

    public class FakeCvGenerator : ICvGenerator
    {
        public Task<CvGenerationResultDto> GenerateAsync(
            JobAdDto jobAd,
            CandidateProfileDto profile)
        {
            return Task.FromResult(new CvGenerationResultDto
            {
                JobAdId = jobAd.Id,
                Documents =
            {
                new GeneratedDocumentDto
                {
                    FileName = "cv.md",
                    Content = $"# CV for {jobAd.Title}\n\nCandidate: {profile.Header}"
                }
            }
            });
        }
    }

}
