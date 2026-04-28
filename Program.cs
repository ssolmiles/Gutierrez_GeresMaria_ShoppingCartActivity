using System;

class CartItem
{
    public Product Product;
    public int Quantity;

    public double GetSubTotal()
    {
        return Product.CalculateSubTotal(Quantity);
    }
}

class Program
{
    // Validation helpers

    static int GetIntInRange(string prompt, int min, int max)
    {
        int value;

        while (true)
        {
            Console.Write(prompt);

            if (!int.TryParse(Console.ReadLine(), out value))
            {
                Console.WriteLine("Invalid input. Numbers only.");
                continue;
            }

            if (value < min || value > max)
            {
                Console.WriteLine($"Enter a number between {min} and {max}.");
                continue;
            }

            return value;
        }
    }

    static int GetPositiveInt(string prompt)
    {
        int value;

        while (true)
        {
            Console.Write(prompt);

            if (!int.TryParse(Console.ReadLine(), out value))
            {
                Console.WriteLine("Invalid input. Numbers only.");
                continue;
            }

            if (value <= 0)
            {
                Console.WriteLine("Must be greater than 0.");
                continue;
            }

            return value;
        }
    }

    static string GetNonEmptyString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be empty.");
                continue;
            }

            return input;
        }
    }

    //  MAIN 

    static void Main()
    {
        Product[] store =
        {
            new Product { Id = 1, Name = "Pizza", Category = "Food", Price = 80, RemainingStock = 20 },
            new Product { Id = 2, Name = "Gelato", Category = "Food", Price = 60, RemainingStock = 15 },

            new Product { Id = 3, Name = "Earpods", Category = "Electronics", Price = 500, RemainingStock = 10 },
            new Product { Id = 4, Name = "Speaker", Category = "Electronics", Price = 1200, RemainingStock = 6 },

            new Product { Id = 5, Name = "Chino", Category = "Clothing", Price = 350, RemainingStock = 12 },
            new Product { Id = 6, Name = "Long Sleeve", Category = "Clothing", Price = 500, RemainingStock = 5 }
        };
