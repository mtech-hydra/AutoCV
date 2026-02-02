using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCV.Contracts.Interfaces;

using AutoCV.Contracts.Dtos;

public interface ICvGenerator
{
    Task<CvGenerationResultDto> GenerateAsync(
        JobAdDto jobAd,
        CandidateProfileDto profile);
}
