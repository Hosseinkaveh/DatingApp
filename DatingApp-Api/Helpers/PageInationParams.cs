namespace DatingApp_Api.Helpers
{
    public class PageInationParams
    {
        public int PageNumber { get; set; } = 1;
        private const int MaxPageSize = 30;
        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

    }
}