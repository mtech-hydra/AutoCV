namespace AutoCV.Web.Fakes
{
    using AutoCV.Contracts.Dtos;
    using AutoCV.Contracts.Interfaces;

    public class FakeDocumentWriter : IGeneratedDocumentWriter
    {
        public Task WriteAsync(
            string jobAdId,
            IReadOnlyList<GeneratedDocumentDto> documents)
        {
            foreach (var doc in documents)
            {
                Console.WriteLine($"=== {doc.FileName} ===");
                Console.WriteLine(doc.Content);
            }

            return Task.CompletedTask;
        }
    }

}
