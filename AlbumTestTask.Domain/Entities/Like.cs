using System.ComponentModel.DataAnnotations.Schema;

namespace AlbumTestTask.Domain.Entities
{
    public class Like : BaseEntity
    {
        public string UserId { get; set; }

        public int PhotoId { get; set; }

        public string ActionType { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("PhotoId")]
        public virtual Photo Photo { get; set; }
    }
}
