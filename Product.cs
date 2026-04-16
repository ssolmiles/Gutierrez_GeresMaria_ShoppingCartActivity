class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int RemainingStock;
    public void DisplayProduct()
    {
        Console.WriteLine($"{Id}. {Name} - Price: {Price} - Stock: {RemainingStock}");
    }
    public double GetItemTotal(int quantity)
    {
        return Price * quantity;
    }
}