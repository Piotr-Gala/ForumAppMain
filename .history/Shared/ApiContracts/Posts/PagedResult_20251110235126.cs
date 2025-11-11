namespace ApiContracts.Posts
{
    // Generyczny wynik stronowany: Items + całkowita liczba rekordów
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
