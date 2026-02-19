using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdig;
using PuppeteerSharp;
using AutoCV.Contracts.Interfaces;

namespace AutoCV.Infrastructure.Pdf
{
    internal class PuppeteerPdfRenderer
    {
        private readonly MarkdownPipeline _pipeline;

        public PuppeteerPdfRenderer()
        {
            _pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        }

        public async Task<byte[]> RenderMarkdownToPdfAsync(string markdown)
        {
            return null;
        }
    }
}
