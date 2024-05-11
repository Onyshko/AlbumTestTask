using Microsoft.AspNetCore.Identity;

namespace AlbumTestTask.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Album> Albums { get; set; }

        public virtual ICollection<Like> Likes { get; set; }
    }
}
