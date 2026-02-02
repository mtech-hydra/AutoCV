using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCV.Contracts.Dtos
{
    public class ExperienceDto
    {
        public string Company { get; set; }
        public string Role { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
        public List<string> BulletPoints { get; set; }
    }

}
