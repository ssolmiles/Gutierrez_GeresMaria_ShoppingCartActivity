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

    public bool MatchesName(string keyword)
    {
        return Name.ToLower().Contains(keyword.ToLower());
    }

    public bool MatchesCategory(string category)
    {
        return Category.ToLower() == category.ToLower();
    }

    public bool HasEnoughStock(int quantity)
    {
        return quantity > 0 && quantity <= RemainingStock;
    }

    public void DeductStock(int quantity)
    {
        if (HasEnoughStock(quantity))
            RemainingStock -= quantity;
    }

    public double CalculateSubTotal(int quantity)
    {
        return Price * quantity;
    }

    public bool IsLowStock(int level)
    {
        return RemainingStock <= level;
    }
}
