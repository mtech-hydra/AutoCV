namespace JobPortal.WebAPI.DTOs
{
    public class CvProfileResponse
    {
        public System.Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public string Skills { get; set; } = null!;
    }
}
