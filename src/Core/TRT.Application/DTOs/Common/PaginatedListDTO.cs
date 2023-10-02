namespace TRT.Application.DTOs.Common
{
    public class PaginatedListDTO<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int PageNumber { get; }
        public int TotalPages { get; }
        public int TotalRecordCount { get; }

        public PaginatedListDTO(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalRecordCount = count;
            Items = items;
        }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;


    }
}
