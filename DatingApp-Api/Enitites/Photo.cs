namespace DatingApp_Api.Enitites
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }

        public AppUser AppUser { get; set; }
        public int AppUserid { get; set; }
    }
}