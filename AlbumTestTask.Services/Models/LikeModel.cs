namespace AlbumTestTask.Services.Models
{
    public class LikeModel : BaseModel
    {
        public string? ActionType { get; set; }

        public string? UserId { get; set; }

        public int? PhotoId { get; set; }
    }
}
