namespace AlbumTestTask.Services.Models
{
    public class PhotoModel : BaseModel
    {
        public byte[] Data { get; set; }

        public string ContentType { get; set; }

        public int LikeCounter { get; set; }

        public int DislikeCounter { get; set; }

        public int AlbumId { get; set; }

        public IList<LikeModel>? Likes { get; set; }
    }
}
