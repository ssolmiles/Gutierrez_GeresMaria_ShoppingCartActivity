using System;

class Product
{
    public int Id;
    public string Name;
    public double Price;
    public int RemainingStock;///

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

class Program
{
    static void Main()
    {
        // Step 1: Create products array
        Product[] Store = new Product[5];

        Store[0] = new Product { Id = 1, Name = "Boots", Price = 3000, RemainingStock = 5 };
        Store[1] = new Product { Id = 2, Name = "Loafers ", Price = 2500, RemainingStock = 10 };
        Store[2] = new Product { Id = 3, Name = "Rubber Shoes", Price = 1900, RemainingStock = 8 };
        Store[3] = new Product { Id = 4, Name = "Kitten Heels", Price = 1400, RemainingStock = 3 };
        Store[4] = new Product { Id = 5, Name = "Slides", Price = 1500, RemainingStock = 15 };


        // Step 2: Create cart array
        CartItem[] Cart = new CartItem[10];
        int CartCount = 0;

        string AddMore = "Y";

        while (AddMore.ToUpper() == "Y")
        {
            Console.WriteLine("\n--- Store Menu ---");

            for (int i = 0; i < Store.Length; i++)
            {
                Store[i].DisplayProduct();
            }

            // Step 3: Get product number
            Console.Write("Enter product number: ");
            string InputProduct = Console.ReadLine();
            int ProductNumber;

            if (!int.TryParse(InputProduct, out ProductNumber) || ProductNumber < 1 || ProductNumber > Store.Length)
            {
                Console.WriteLine("Invalid product number!");
                continue;
            }

            Product SelectedProduct = Store[ProductNumber - 1];

            if (SelectedProduct.RemainingStock == 0)
            {
                Console.WriteLine("Sorry, this product is out of stock!");
                continue;
            }

            // Step 3: Get quantity
            Console.Write("Enter quantity: ");
            string InputQuantity = Console.ReadLine();
            int Quantity;

            if (!int.TryParse(InputQuantity, out Quantity) || Quantity < 1)
            {
                Console.WriteLine("Invalid quantity!");
                continue;
            }

            if (Quantity > SelectedProduct.RemainingStock)
            {
                Console.WriteLine("Not enough stock available.");
                continue;
            }

            // Step 6: Check for duplicate cart entries
          