namespace DatingApp_Api.Helpers
{
    public class PageinationHeader
    { 
         public PageinationHeader(int itemPerPage, int totalItems, int totalPage, int currenPge)
        {
            ItemPerPage = itemPerPage;
            TotalItems = totalItems;
            TotalPage = totalPage;
            CurrentPge = currenPge;
        }

        public int ItemPerPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPge { get; set; }
    }
}