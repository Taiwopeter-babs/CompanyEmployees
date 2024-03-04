namespace Shared.RequestFeatures
{
    public class PagedList<T> : List<T>
    {
        public PageMetadata PageMetadata { get; set; }

        public PagedList(List<T> items, int itemsCount, int pageNumber, int pageSize) 
        {
            PageMetadata = new PageMetadata
            {
                TotalItemsCount = itemsCount,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(itemsCount / (double)pageSize),
            };

            // Append the items to the end of the list
            AddRange(items);
        }

        /// <summary>
        /// Returns a new paginated list of source
        /// </summary>
        /// <param name="source"></param>
        /// <param name="itemsCount"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList(
            IEnumerable<T> source, int itemsCount, int pageNumber, int pageSize)
        {
            var count = itemsCount;
            var items = source.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
