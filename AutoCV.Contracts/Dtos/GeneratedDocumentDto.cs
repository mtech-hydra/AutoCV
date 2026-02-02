using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCV.Contracts.Dtos
{
    public class GeneratedDocumentDto
    {
        public string FileName { get; set; }   // cv.md, resume.md, cover.md
        public string Content { get; set; }
    }

}
