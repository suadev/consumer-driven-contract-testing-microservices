namespace Data
{
    public class Product
    {
        public Product(int id, string name, int stockCount, bool isActive = true)
        {
            ID = id;
            Name = name;
            NormalizedName = name.ToUpperInvariant();
            StockCount = stockCount;
            IsActive = isActive;
        }

        public Product()
        { }

        public int ID { get; private set; }
        public string Name { get; private set; }
        public string NormalizedName { get; private set; }
        public int StockCount { get; private set; }
        public bool IsActive { get; private set; }
    }
}