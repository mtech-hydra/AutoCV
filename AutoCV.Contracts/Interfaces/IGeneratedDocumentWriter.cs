using AutoCV.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCV.Contracts.Interfaces
{
    public interface IGeneratedDocumentWriter
    {
        Task WriteAsync(
            string jobAdId,
            IReadOnlyList<GeneratedDocumentDto> documents);
    }
}
