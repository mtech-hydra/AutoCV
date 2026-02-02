using AutoCV.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCV.Contracts.Interfaces
{
    public interface IJobAdSource
    {
        Task<JobAdDto> LoadAsync(string folderPath);
    }
}
