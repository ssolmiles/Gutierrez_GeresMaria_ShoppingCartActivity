using System;
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
        Product[] Store = new Product[5];

        Store[0] = new Product { Id = 1, Name = "Boots", Price = 3000, RemainingStock = 5 };
        Store[1] = new Product { Id = 2, Name = "Loafers", Price = 2500, RemainingStock = 10 };
        Store[2] = new Product { Id = 3, Name = "Rubber Shoes", Price = 1900, RemainingStock = 8 };
        Store[3] = new Product { Id = 4, Name = "Kitten Heels", Price = 1400, RemainingStock = 3 };
        Store[4] = new Product { Id = 5, Name = "Slides", Price = 1500, RemainingStock = 15 };

        CartItem[] Cart = new CartItem[10];
        int CartCount = 0;
        int totalQuantity = 0;

        string AddMore = "Y";

        while (AddMore == "Y")
        {
            Console.WriteLine("\n Store Menu ");
            for (int i = 0; i < Store.Length; i++)
            {
                Store[i].DisplayProduct();
            }
            Console.WriteLine("\nCart limit: 10 items total");

            Console.Write("Enter product number: ");
            if (!int.TryParse(Console.ReadLine(), out int ProductNumber) ||
                ProductNumber < 1 || ProductNumber > Store.Length)
            {
                Console.WriteLine("Invalid product number.");
                continue;
            }
            Product SelectedProduct = Store[ProductNumber - 1];

            if (SelectedProduct.RemainingStock <= 0)
            {
                Console.WriteLine("Product is out of stock.");
                continue;
            }

            Console.Write("Enter quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int Quantity) || Quantity < 1)
            {
                Console.WriteLine("Invalid quantity.");
                continue;
            }

            if (Quantity > SelectedProduct.RemainingStock)
            {
                Console.WriteLine("Not enough stock available.");
                continue;
            }
            int remainingCartSpace = 10 - totalQuantity;
            if (Quantity > remainingCartSpace)
            {
                Console.WriteLine($"\nOnly {remainingCartSpace} items can still be added.");

                string choice;
                while (true)
                {
                    Console.Write("Add remaining items? (Y/N): ");
                    choice = Console.ReadLine().ToUpper();

                    if (choice == "Y" || choice == "N")
                        break;

                    Console.WriteLine("Invalid input. Please enter Y or N only.");
                }
                if (choice == "Y")
                {
                    Quantity = remainingCartSpace;
                }
                else
                {
                    Console.WriteLine("Item not added.");
                    continue;
                }
            }

            bool FoundInCart = false;

            for (int i = 0; i < CartCount; i++)
            {
                if (Cart[i].Product.Id == SelectedProduct.Id)
                {
                    Cart[i].Quantity += Quantity;
                    Cart[i].SubTotal += SelectedProduct.GetItemTotal(Quantity);
                    FoundInCart = true;
                    break;
                }
            }

            if (!FoundInCart)
            {
                Cart[CartCount] = new CartItem
                {
                    Product = SelectedProduct,
                    Quantity = Quantity,
                    SubTotal = SelectedProduct.GetItemTotal(Quantity)
                };
                CartCount++;
            }

            totalQuantity += Quantity;
            SelectedProduct.RemainingStock -= Quantity;

            if (totalQuantity >= 10)
            {
                Console.WriteLine("\nCart reached maximum capacity (10 items).");
                break;
            }
            while (true)
            {
                Console.Write("Add more items? (Y/N): ");
                AddMore = Console.ReadLine().ToUpper();

                if (AddMore == "Y" || AddMore == "N")
                    break;

                Console.WriteLine("Invalid input. type (Y/N) only.");
            }

            if (AddMore == "N")
                break;
        }
        Console.WriteLine("\n Receipt");
        double GrandTotal = 0;

        for (int i = 0; i < CartCount; i++)
        {
            Console.WriteLine($"{Cart[i].Product.Name} x {Cart[i].Quantity} = {Cart[i].SubTotal}");
            GrandTotal += Cart[i].SubTotal;
        }

        Console.WriteLine($"Grand Total: {GrandTotal}");

        double Discount = 0;
        if (GrandTotal >= 5000)
        {
            Discount = GrandTotal * 0.10;
            Console.WriteLine($"Discount (10%): {Discount}");
        }

        Console.WriteLine($"Final Total: {GrandTotal - Discount}");
        Console.WriteLine("\n--- Updated Stock ---");
        for (int i = 0; i < Store.Length; i++)
        {
            Console.WriteLine($"{Store[i].Name} - Stock: {Store[i].RemainingStock}");
        }
        Console.WriteLine("\nThank you for shopping!");
    }
}