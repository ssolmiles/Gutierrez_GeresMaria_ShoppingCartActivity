using System;

class Product
{
    public int Id;
    public string Name;
    public string Category;
    public double Price;
    public int RemainingStock;

    public void DisplayProduct()
    {
        Console.WriteLine($"{Id}. {Name} - {Category} - PHP {Price} - Stock: {RemainingStock}");
    }

    // search by name
    public bool MatchesName(string keyword)
    {
        return Name.ToLower().Contains(keyword.ToLower());
    }

    // category filter
    public bool MatchesCategory(string category)
    {
        return Category == category;
    }

    // stock check
    public bool HasEnoughStock(int quantity)
    {
        return quantity > 0 && quantity <= RemainingStock;
    }

    // stock deduction
    public void DeductStock(int quantity)
    {
        if (HasEnoughStock(quantity))
            RemainingStock -= quantity;
    }

    //subtotal
    public double CalculateSubTotal(int quantity)
    {
        return Price * quantity;
    }

    //low stock check
    public bool IsLowStock(int level)
    {
        return RemainingStock <= level;
    }
}