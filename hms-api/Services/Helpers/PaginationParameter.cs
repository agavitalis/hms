namespace HMS.Services.Helpers
{
    public class PaginationParameter
    {
        const int MaxPageSize = 500;
        public string SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        public string OrderBy { get; set; }
        public string Fields { get; set; }

        private int _pageSize = 100;
        public int PageSize
        {
            get => _pageSize;

            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;   
        }

      

    }
}
