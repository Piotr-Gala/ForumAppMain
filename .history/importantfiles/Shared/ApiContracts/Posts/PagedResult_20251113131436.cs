namespace ApiContracts.Posts
{
    // Generic paged result: Items + total count
    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
