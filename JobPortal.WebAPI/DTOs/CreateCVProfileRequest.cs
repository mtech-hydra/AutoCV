namespace JobPortal.WebAPI.DTOs
{
    public class CreateCVProfileRequest
    {
        public string Title { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public string Skills { get; set; } = null!;
    }
}
