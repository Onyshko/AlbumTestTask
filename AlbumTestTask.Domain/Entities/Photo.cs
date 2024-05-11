namespace AlbumTestTask.Domain.Entities
{
    public class Photo : BaseEntity
    {
        public byte[] Data { get; set; }

        public string ContentType { get; set; }

        public int LikeCounter { get; set; }

        public int DislikeCounter { get; set; }

        public int AlbumId { get; set; }

        public Album Album { get; set; }

        public virtual ICollection<Like> Likes { get; set; }
    }
}
