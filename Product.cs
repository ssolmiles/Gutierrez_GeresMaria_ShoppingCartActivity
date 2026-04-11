using System;

class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int RemainingStock;

    // Display product info
    public void DisplayProduct()
    {
        Console.WriteLine($"{Id}. {Name} - Price: {Price} - Stock: {RemainingStock}");
    }
    //
    // Calculate total for a given quantity
    public double GetItemTotal(int Quantity)
    {
        //
        return Price * Quantity;
    }
}

class CartItem
{
    public Product Product;
    public int Quantity;
    public double SubTotal;
}
