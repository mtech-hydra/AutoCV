using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCV.Contracts.Dtos
{
    public class ProjectMatchDto
    {
        public string ProjectName { get; set; }
        public double Score { get; set; }
        public List<string> MatchingKeywords { get; set; }
    }

}
