using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCV.Contracts.Interfaces
{
    using AutoCV.Contracts.Interfaces;

    public class HappyPathRunner
    {
        private readonly IJobAdSource _jobAdSource;
        private readonly ICandidateProfileSource _profileSource;
        private readonly ICvGenerator _cvGenerator;
        private readonly IGeneratedDocumentWriter _writer;

        public HappyPathRunner(
            IJobAdSource jobAdSource,
            ICandidateProfileSource profileSource,
            ICvGenerator cvGenerator,
            IGeneratedDocumentWriter writer)
        {
            _jobAdSource = jobAdSource;
            _profileSource = profileSource;
            _cvGenerator = cvGenerator;
            _writer = writer;
        }

        public async Task RunAsync(string jobAdId)
        {
            var jobAd = await _jobAdSource.LoadAsync(jobAdId);
            var profile = await _profileSource.LoadAsync();

            var result = await _cvGenerator.GenerateAsync(jobAd, profile);

            await _writer.WriteAsync(jobAdId, result.Documents);
        }
    }

}
