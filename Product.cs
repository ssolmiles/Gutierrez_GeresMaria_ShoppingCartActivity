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
        Console.WriteLine(
            $"{Id}. {Name} - {Category} - PHP {Price} - Stock: {RemainingStock}"
        );
    }

 
    // Checks if requested quantity is valid and available
    public bool HasEnoughStock(int quantity)
    {
        return quantity > 0 && quantity <= RemainingStock;
    }

    
    // Deducts stock AFTER successful checkout
    public void DeductStock(int quantity)
    {
        if (!HasEnoughStock(quantity))
        {
            throw new InvalidOperationException("Stock deduction failed.");
        }

        RemainingStock -= quantity;
    }

    // price calculation
    // Computes subtotal for a given quantity
    public double CalculateSubTotal(int quantity)
    {
        return Price * quantity;
    }

    // low stock alert: checks if remaining stock is at or below reorder level
    public bool IsLowStock(int reorderLevel)
    {
        return RemainingStock <= reorderLevel;
    }
}