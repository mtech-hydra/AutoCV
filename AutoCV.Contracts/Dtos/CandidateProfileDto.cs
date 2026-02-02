using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCV.Contracts.Dtos
{
    public class CandidateProfileDto
    {
        public HeaderDto Header { get; set; }
        public List<ExperienceDto> Experiences { get; set; }
        public List<ProjectDto> Projects { get; set; }
        public EducationDto Education { get; set; }
    }

}
