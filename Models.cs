public class Product
{
     public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}

public class Customer
{
    public int Id { get; set; }
    public string? Name { get; set; } = string.Empty;
}
