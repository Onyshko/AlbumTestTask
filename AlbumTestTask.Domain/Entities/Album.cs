namespace AlbumTestTask.Domain.Entities
{
    public class Album : BaseEntity
    {
        public string Name { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }
    }
}
