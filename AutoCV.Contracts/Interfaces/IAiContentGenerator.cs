using AutoCV.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCV.Contracts.Interfaces
{
    public interface IAiContentGenerator
    {
        /// <summary>
        /// Generate CV content for a candidate, optionally using job ad info
        /// </summary>
        Task<string> GenerateCvAsync(CandidateProfileDto profile, JobAdDto? jobAd = null);

        /// <summary>
        /// Generate cover letter for a candidate given a job ad
        /// </summary>
        Task<string> GenerateCoverLetterAsync(CandidateProfileDto profile, JobAdDto jobAd);
    }
}
