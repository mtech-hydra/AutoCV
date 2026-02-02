namespace AutoCV.Web.Fakes
{
    using AutoCV.Contracts.Dtos;
    using AutoCV.Contracts.Interfaces;

    public class FakeCandidateProfileSource : ICandidateProfileSource
    {
        public Task<CandidateProfileDto> LoadAsync()
        {
            return Task.FromResult(new CandidateProfileDto
            {
                Header = new HeaderDto
                {
                    FullName = "Your Name",
                    Email = "you@example.com"
                },
                Education = new EducationDto
                {
                    University = "Your University",
                    Degree = "Computer Science",
                    GraduationYear = 2015
                },
                Experiences = new(),
                Projects = new()
            });
        }
    }

}
