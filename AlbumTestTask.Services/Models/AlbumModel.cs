namespace AlbumTestTask.Services.Models
{
    public class AlbumModel : BaseModel
    {
        public string Name { get; set; }

        public string UserId { get; set; }

        public IList<PhotoModel>? Photos { get; set; }
    }
}