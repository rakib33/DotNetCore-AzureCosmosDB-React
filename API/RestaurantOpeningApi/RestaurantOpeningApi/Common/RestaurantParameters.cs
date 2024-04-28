namespace RestaurantOpeningApi.Common
{
    public class Pagination
    {
            const int maxPageSize = 100;
            public int PageNumber { get; set; } = 1;

            private int _pageSize = 10;
            public int PageSize
            {
                get
                {
                    return _pageSize;
                }
                set
                {
                    _pageSize = (value > maxPageSize) ? maxPageSize : value;
                }
            }
        }

    public class RestaurantParameters
    {
        public Pagination Pagination { get; set; }
        public string name { get; set; }
        public string day { get; set; }
        public TimeSpan time { get; set; }
    }

}
