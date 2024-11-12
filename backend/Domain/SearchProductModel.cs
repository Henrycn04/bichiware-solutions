namespace backend.Domain
{
    public class SearcProductModel
    {
        public int ID { get; set; }
    }

    public class SearchProductListModel
    {
        public List<int> ProductIDs { get; set; } = new List<int>();
    }
}
