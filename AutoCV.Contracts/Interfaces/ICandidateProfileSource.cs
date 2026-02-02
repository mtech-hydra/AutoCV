using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCV.Contracts.Interfaces;

using AutoCV.Contracts.Dtos;

public interface ICandidateProfileSource
{
    Task<CandidateProfileDto> LoadAsync();
}

