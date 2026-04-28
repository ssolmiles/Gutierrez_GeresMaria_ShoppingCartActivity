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

        CartItem[] cart = new CartItem[3];
        int cartCount = 0;

        double[] orderHistory = new double[20];
        int orderCount = 0;

        int receiptNumber = 1;

        while (true)
        {
            Console.WriteLine("\n=== SHOP MENU ===");
            Console.WriteLine("1. View Products");
            Console.WriteLine("2. Search Product");
            Console.WriteLine("3. Filter by Category");
            Console.WriteLine("4. Add to Cart");
            Console.WriteLine("5. View Cart");
            Console.WriteLine("6. Checkout");
            Console.WriteLine("7. Order History");
            Console.WriteLine("8. Exit");

            int choice = GetIntInRange("Choose: ", 1, 8);

            // view 
            if (choice == 1)
            {
                foreach (Product p in store)
                    p.DisplayProduct();
            }

            // search 
            else if (choice == 2)
            {
                string key = GetNonEmptyString("Enter product name: ");

                bool found = false;

                foreach (Product p in store)
                {
                    if (p.MatchesName(key))
                    {
                        p.DisplayProduct();
                        found = true;
                    }
                }

                if (!found)
                    Console.WriteLine("No product found.");
            }

            // category filter
            else if (choice == 3)
            {
                Console.WriteLine("\n1. Food\n2. Electronics\n3. Clothing");
                int cat = GetIntInRange("Choose category: ", 1, 3);

                string category =
                    cat == 1 ? "Food" :
                    cat == 2 ? "Electronics" :
                    "Clothing";

                bool found = false;

                foreach (Product p in store)
                {
                    if (p.MatchesCategory(category))
                    {
                        p.DisplayProduct();
                        found = true;
                    }
                }

                if (!found)
                    Console.WriteLine("No products found.");
            }

            // add to cart
            else if (choice == 4)
            {
                if (cartCount >= cart.Length)
                {
                    Console.WriteLine("Cart limit reached.");
                    continue;
                }

                int id = GetPositiveInt("Enter product ID: ");

                Product selected = null;

                foreach (Product p in store)
                    if (p.Id == id)
                        selected = p;

                if (selected == null)
                {
                    Console.WriteLine("Product not found.");
                    continue;
                }

                int qty = GetPositiveInt("Enter quantity: ");

                if (!selected.HasEnoughStock(qty))
                {
                    Console.WriteLine("Not enough stock.");
                    continue;
                }

                cart[cartCount++] = new CartItem
                {
                    Product = selected,
                    Quantity = qty
                };

                Console.WriteLine("Added to cart.");
            }
            //view cart
            else if (choice == 5)
            {
                if (cartCount == 0)
                {
                    Console.WriteLine("Cart empty.");
                    continue;
                }

                for (int i = 0; i < cartCount; i++)
                {
                    Console.WriteLine(
                        $"{i + 1}. {cart[i].Product.Name} x {cart[i].Quantity} = PHP {cart[i].GetSubTotal()}"
                    );
                }
            }

            // checkout
            else if (choice == 6)
            {
                if (cartCount == 0)
                {
                    Console.WriteLine("Cart empty.");
                    continue;
                }

                double total = 0;

                for (int i = 0; i < cartCount; i++)
                    total += cart[i].GetSubTotal();

                double discount = total >= 5000 ? total * 0.10 : 0;
                double finalTotal = total - discount;

                double payment;

                while (true)
                {
                    Console.WriteLine($"\nFinal Total: PHP {finalTotal}");
                    Console.Write("Enter payment: ");

                    if (!double.TryParse(Console.ReadLine(), out payment))
                    {
                        Console.WriteLine("Invalid input.");
                        continue;
                    }

                    if (payment < finalTotal)
                    {
                        Console.WriteLine("Insufficient payment.");
                        continue;
                    }

                    break;
                }

                double change = payment - finalTotal;

                // stock deduction
                for (int i = 0; i < cartCount; i++)
                    cart[i].Product.DeductStock(cart[i].Quantity);

                // receipt
                Console.WriteLine("\n==========================");
                Console.WriteLine($"Receipt No: {receiptNumber:0000}");
                Console.WriteLine($"Date: {DateTime.Now}");
                Console.WriteLine("--------------------------");

                for (int i = 0; i < cartCount; i++)
                    Console.WriteLine($"{cart[i].Product.Name} x {cart[i].Quantity} = PHP {cart[i].GetSubTotal()}");

                Console.WriteLine("--------------------------");
                Console.WriteLine($"Grand Total: PHP {total}");
                Console.WriteLine($"Discount: PHP {discount}");
                Console.WriteLine($"Final Total: PHP {finalTotal}");
                Console.WriteLine($"Payment: PHP {payment}");
                Console.WriteLine($"Change: PHP {change}");
                Console.WriteLine("==========================");

                orderHistory[orderCount++] = finalTotal;
                receiptNumber++;

                // LOW STOCK ALERT
                Console.WriteLine("\nLOW STOCK ALERT:");

                bool low = false;

                foreach (Product p in store)
                {
                    if (p.IsLowStock(5))
                    {
                        Console.WriteLine($"{p.Name} has only {p.RemainingStock} left.");
                        low = true;
                    }
                }

                if (!low)
                    Console.WriteLine("No low stock items.");

                cartCount = 0;
            }

            // 7 HISTORY
            else if (choice == 7)
            {
                for (int i = 0; i < orderCount; i++)
                    Console.WriteLine($"Receipt #{i + 1:0000} - PHP {orderHistory[i]}");
            }

            // 8 EXIT
            else if (choice == 8)
            {
                Console.WriteLine("Thank you!");
                break;
            }
        }
    }
}

