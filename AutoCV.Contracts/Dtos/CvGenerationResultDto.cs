using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCV.Contracts.Dtos
{
    public class CvGenerationResultDto
    {
        public string JobAdId { get; set; }
        public List<GeneratedDocumentDto> Documents { get; set; }
        public List<string> ProjectSelectionRationale { get; set; }
    }

}
