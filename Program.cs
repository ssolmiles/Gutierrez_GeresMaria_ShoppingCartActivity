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
    static void Main()
    {
        // products 
        Product[] store = new Product[]
        {
            new Product { Id = 1, Name = "Boots", Category = "Footwear", Price = 3000, RemainingStock = 5 },
            new Product { Id = 2, Name = "Loafers", Category = "Footwear", Price = 2500, RemainingStock = 10 },
            new Product { Id = 3, Name = "Rubber Shoes", Category = "Footwear", Price = 1900, RemainingStock = 8 },
            new Product { Id = 4, Name = "Kitten Heels", Category = "Footwear", Price = 1400, RemainingStock = 3 },
            new Product { Id = 5, Name = "Slides", Category = "Footwear", Price = 1500, RemainingStock = 15 }
        };

        // cart and history 
        CartItem[] cart = new CartItem[3];   // cart limit PER ROW
        int cartCount = 0;

        double[] orderHistory = new double[20];
        int orderCount = 0;

        int receiptNumber = 1;

        // main loops
        while (true)
        {
            Console.WriteLine("\n=== SHOPPING CART MENU ===");
            Console.WriteLine("1. View Products");
            Console.WriteLine("2. Add to Cart");
            Console.WriteLine("3. View Cart");
            Console.WriteLine("4. Update Cart Quantity");
            Console.WriteLine("5. Remove Cart Item");
            Console.WriteLine("6. Checkout");
            Console.WriteLine("7. View Order History");
            Console.WriteLine("8. Exit");

            Console.Write("Choose option: ");
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input.");
                continue;
            }

            // view products
            if (choice == 1)
            {
                Console.WriteLine("\n--- PRODUCTS ---");
                foreach (Product p in store)
                    p.DisplayProduct();
            }

            // add to cart 
            else if (choice == 2)
            {
                if (cartCount >= cart.Length)
                {
                    Console.WriteLine("Cart row limit reached.");
                    continue;
                }

                Console.Write("Enter product ID: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("Invalid input.");
                    continue;
                }

                Product selected = null;
                foreach (Product p in store)
                {
                    if (p.Id == id)
                        selected = p;
                }

                if (selected == null)
                {
                    Console.WriteLine("Product not found.");
                    continue;
                }

                Console.Write("Enter quantity: ");
                if (!int.TryParse(Console.ReadLine(), out int qty) || qty < 1)
                {
                    Console.WriteLine("Invalid quantity.");
                    continue;
                }

                // stock validation
                if (!selected.HasEnoughStock(qty))
                {
                    Console.WriteLine("Not enough stock available.");
                    continue;
                }

                bool found = false;
                for (int i = 0; i < cartCount; i++)
                {
                    if (cart[i].Product.Id == selected.Id)
                    {
                        int newQty = cart[i].Quantity + qty;

                        if (!selected.HasEnoughStock(newQty))
                        {
                            Console.WriteLine("Exceeds available stock.");
                            found = true;
                            break;
                        }

                        cart[i].Quantity = newQty;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    cart[cartCount] = new CartItem
                    {
                        Product = selected,
                        Quantity = qty
                    };
                    cartCount++;
                }

                Console.WriteLine("Item added to cart.");
            }

            // view cart 
            else if (choice == 3)
            {
                if (cartCount == 0)
                {
                    Console.WriteLine("Cart is empty.");
                    continue;
                }

                Console.WriteLine("\n--- CART ---");
                for (int i = 0; i < cartCount; i++)
                {
                    Console.WriteLine(
                        $"{i + 1}. {cart[i].Product.Name} x {cart[i].Quantity} = PHP {cart[i].GetSubTotal()}"
                    );
                }
            }

            // update quantity
            else if (choice == 4)
            {
                Console.Write("Enter cart item number: ");
                if (!int.TryParse(Console.ReadLine(), out int index) ||
                    index < 1 || index > cartCount)
                {
                    Console.WriteLine("Invalid choice.");
                    continue;
                }

                Console.Write("Enter new quantity: ");
                if (!int.TryParse(Console.ReadLine(), out int newQty) || newQty < 1)
                {
                    Console.WriteLine("Invalid quantity.");
                    continue;
                }

                Product product = cart[index - 1].Product;

                if (!product.HasEnoughStock(newQty))
                {
                    Console.WriteLine("Not enough stock.");
                    continue;
                }

                cart[index - 1].Quantity = newQty;
                Console.WriteLine("Cart updated.");
            }

            // remove item
            else if (choice == 5)
            {
                Console.Write("Enter cart item number to remove: ");
                if (!int.TryParse(Console.ReadLine(), out int remove) ||
                    remove < 1 || remove > cartCount)
                {
                    Console.WriteLine("Invalid choice.");
                    continue;
                }

                for (int i = remove - 1; i < cartCount - 1; i++)
                    cart[i] = cart[i + 1];

                cartCount--;
                Console.WriteLine("Item removed.");
            }

            // checkout 
            else if (choice == 6)
            {
                if (cartCount == 0)
                {
                    Console.WriteLine("Cart is empty.");
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
                    Console.Write($"Final Total: PHP {finalTotal}\nEnter payment: ");
                    if (!double.TryParse(Console.ReadLine(), out payment) ||
                        payment < finalTotal)
                    {
                        Console.WriteLine("Insufficient or invalid payment.");
                        continue;
                    }
                    break;
                }

                double change = payment - finalTotal;

                // stock deduction
                for (int i = 0; i < cartCount; i++)
                    cart[i].Product.DeductStock(cart[i].Quantity);

                Console.WriteLine("\n=== RECEIPT ===");
                Console.WriteLine($"Receipt No: {receiptNumber:0000}");
                Console.WriteLine($"Date: {DateTime.Now}");
                Console.WriteLine($"Total: PHP {total}");
                Console.WriteLine($"Discount: PHP {discount}");
                Console.WriteLine($"Final Total: PHP {finalTotal}");
                Console.WriteLine($"Payment: PHP {payment}");
                Console.WriteLine($"Change: PHP {change}");

                orderHistory[orderCount++] = finalTotal;
                receiptNumber++;

                Console.WriteLine("\nLOW STOCK ALERT:");
                foreach (Product p in store)
                {
                    if (p.IsLowStock(5))
                        Console.WriteLine($"{p.Name} has only {p.RemainingStock} left.");
                }

                cartCount = 0; // clear cart
            }

            // order history 
            else if (choice == 7)
            {
                Console.WriteLine("\nORDER HISTORY");
                for (int i = 0; i < orderCount; i++)
                {
                    Console.WriteLine($"Receipt #{i + 1:0000} - PHP {orderHistory[i]}");
                }
            }

            //exit
            else if (choice == 8)
            {
                Console.WriteLine("Thank you for shopping!");
                break;
            }
        }
    }
}