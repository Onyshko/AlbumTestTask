namespace AlbumTestTask.Services.Models
{
    public class UserModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public IList<AlbumModel>? Albums { get; set; }
    }
}
