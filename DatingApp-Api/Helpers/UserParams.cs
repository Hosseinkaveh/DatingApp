namespace DatingApp_Api.Helpers
{
    public class UserParams
    {
         public int PageNumber { get; set; } = 1;
        private const int MaxPageSize = 30;
        private int _pageSize  = 10;

        public int PageSize {
             get => _pageSize; 
             set => _pageSize =  (value > MaxPageSize) ? MaxPageSize : value; 
             }

             public string CurrentUserName { get; set; }
             public string Gender { get; set; }

             public int MinAge { get; set; } = 18;
             public int MaxAge { get; set; } = 99;

             public string OrderBy { get; set; } ="lastActive";

       



        
    }
}