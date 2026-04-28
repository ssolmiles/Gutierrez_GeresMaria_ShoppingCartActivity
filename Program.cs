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

class OrderRecord
{
    public int ReceiptNumber;
    public double FinalTotal;
}

class Program
{
    // ─── Validation Helpers ──────────────────────

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

    // FIX #5 — Y/N validation helper 
    static bool GetYesOrNo(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine()?.Trim().ToLower();
            if (input == "y") return true;
            if (input == "n") return false;
            Console.WriteLine("Invalid input. Please enter Y or N only.");
        }
    }

    // FIX #6 — Compute how much of a product is already in the cart
    static int GetCartQuantityForProduct(CartItem[] cart, int cartCount, int productId)
    {
        int total = 0;
        for (int i = 0; i < cartCount; i++)
            if (cart[i].Product.Id == productId)
                total += cart[i].Quantity;
        return total;
    }

    // ─── Cart Management Submenu ─────────────────────────

    // FIX #1 — Dedicated cart submenu (remove, update, clear all missing before)
    static void CartMenu(CartItem[] cart, ref int cartCount)
    {
        while (true)
        {
            Console.WriteLine("\n=== CART MENU ===");
            Console.WriteLine("1. View Cart");
            Console.WriteLine("2. Update Item Quantity");
            Console.WriteLine("3. Remove Item");
            Console.WriteLine("4. Clear Cart");
            Console.WriteLine("5. Back");

            int choice = GetIntInRange("Choose: ", 1, 5);

            if (choice == 1)
            {
                ViewCart(cart, cartCount);
            }
            else if (choice == 2)
            {
                ViewCart(cart, cartCount);
                if (cartCount == 0) continue;

                int index = GetIntInRange("Enter item number to update: ", 1, cartCount) - 1;
                CartItem item = cart[index];

                // FIX #6 — Stock check must account for OTHER cart entries of the same product
                int otherCartQty = GetCartQuantityForProduct(cart, cartCount, item.Product.Id) - item.Quantity;
                int availableStock = item.Product.RemainingStock - otherCartQty;

                Console.WriteLine($"Current quantity: {item.Quantity} | Available stock: {availableStock}");

                int newQty = GetPositiveInt("Enter new quantity: ");

                if (newQty > availableStock)
                {
                    Console.WriteLine($"Not enough stock. Max you can set: {availableStock}");
                    continue;
                }

                item.Quantity = newQty;
                Console.WriteLine("Quantity updated.");
            }
            else if (choice == 3)
            {
                ViewCart(cart, cartCount);
                if (cartCount == 0) continue;

                int index = GetIntInRange("Enter item number to remove: ", 1, cartCount) - 1;

                Console.WriteLine($"Removing: {cart[index].Product.Name}");

                // Shift items left to fill the gap
                for (int i = index; i < cartCount - 1; i++)
                    cart[i] = cart[i + 1];

                cart[cartCount - 1] = null;
                cartCount--;

                Console.WriteLine("Item removed.");
            }
            else if (choice == 4)
            {
                if (cartCount == 0)
                {
                    Console.WriteLine("Cart is already empty.");
                    continue;
                }

                if (GetYesOrNo("Clear all items from cart? (Y/N): "))
                {
                    for (int i = 0; i < cartCount; i++)
                        cart[i] = null;
                    cartCount = 0;
                    Console.WriteLine("Cart cleared.");
                }
            }
            else if (choice == 5)
            {
                break;
            }
        }
    }

    static void ViewCart(CartItem[] cart, int cartCount)
    {
        if (cartCount == 0)
        {
            Console.WriteLine("Cart is empty.");
            return;
        }

        Console.WriteLine("\n--- Your Cart ---");
        for (int i = 0; i < cartCount; i++)
        {
            Console.WriteLine(
                $"{i + 1}. {cart[i].Product.Name} x {cart[i].Quantity} = PHP {cart[i].GetSubTotal()}"
            );
        }
    }

    // ─── Checkout ─────────────────────────────────────────────────────────────

    static void Checkout(CartItem[] cart, ref int cartCount, Product[] store,
                         OrderRecord[] orderHistory, ref int orderCount, ref int receiptNumber)
    {
        if (cartCount == 0)
        {
            Console.WriteLine("Cart is empty.");
            return;
        }

        ViewCart(cart, cartCount);

        // FIX #5 — Y/N confirmation before checkout
        if (!GetYesOrNo("Proceed to checkout? (Y/N): "))
        {
            Console.WriteLine("Checkout cancelled.");
            return;
        }

        // FIX #6 — Validate stock for every cart item at checkout time
        //          (catches cases where the same product was added twice)
        bool stockOk = true;
        for (int i = 0; i < cartCount; i++)
        {
            int totalQtyInCart = GetCartQuantityForProduct(cart, cartCount, cart[i].Product.Id);

            if (totalQtyInCart > cart[i].Product.RemainingStock)
            {
                Console.WriteLine(
                    $"Insufficient stock for {cart[i].Product.Name}. " +
                    $"Requested: {totalQtyInCart}, Available: {cart[i].Product.RemainingStock}"
                );
                stockOk = false;
            }
        }

        if (!stockOk)
        {
            Console.WriteLine("Please update your cart and try again.");
            return;
        }

        double total = 0;
        for (int i = 0; i < cartCount; i++)
            total += cart[i].GetSubTotal();

        double discount = total >= 5000 ? total * 0.10 : 0;
        double finalTotal = total - discount;

        // FIX #5 — Payment input re-prompts on invalid/insufficient (already present, kept & cleaned)
        double payment;
        while (true)
        {
            Console.WriteLine($"\nFinal Total: PHP {finalTotal}");
            Console.Write("Enter payment: PHP ");
            if (!double.TryParse(Console.ReadLine(), out payment))
            {
                Console.WriteLine("Invalid input. Numbers only.");
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

        // Deduct stock
        for (int i = 0; i < cartCount; i++)
            cart[i].Product.DeductStock(cart[i].Quantity);

        // Print receipt
        Console.WriteLine("\n==================================");
        Console.WriteLine($"  Receipt No: {receiptNumber:0000}");
        Console.WriteLine($"  Date: {DateTime.Now:MMMM dd, yyyy h:mm tt}");
        Console.WriteLine("----------------------------------");
        for (int i = 0; i < cartCount; i++)
            Console.WriteLine($"  {cart[i].Product.Name} x {cart[i].Quantity} = PHP {cart[i].GetSubTotal()}");
        Console.WriteLine("----------------------------------");
        Console.WriteLine($"  Grand Total : PHP {total}");
        Console.WriteLine($"  Discount    : PHP {discount}");
        Console.WriteLine($"  Final Total : PHP {finalTotal}");
        Console.WriteLine($"  Payment     : PHP {payment}");
        Console.WriteLine($"  Change      : PHP {change}");
        Console.WriteLine("==================================");

        // FIX #4 — Store both receipt number AND total (was only storing total before)
        orderHistory[orderCount++] = new OrderRecord
        {
            ReceiptNumber = receiptNumber,
            FinalTotal = finalTotal
        };

        receiptNumber++;

        // Low stock alert
        Console.WriteLine("\nLOW STOCK ALERT:");
        bool low = false;
        foreach (Product p in store)
        {
            if (p.IsLowStock(5))
            {
                Console.WriteLine($"  {p.Name} has only {p.RemainingStock} left.");
                low = true;
            }
        }
        if (!low)
            Console.WriteLine("  No low stock items.");

        // Clear cart
        for (int i = 0; i < cartCount; i++)
            cart[i] = null;
        cartCount = 0;
    }

    // ─── Main ───

    static void Main()
    {
        Product[] store =
        {
            new Product { Id = 1, Name = "Pizza",       Category = "Food",        Price = 80,   RemainingStock = 20 },
            new Product { Id = 2, Name = "Gelato",      Category = "Food",        Price = 60,   RemainingStock = 15 },
            new Product { Id = 3, Name = "Earpods",     Category = "Electronics", Price = 500,  RemainingStock = 10 },
            new Product { Id = 4, Name = "Speaker",     Category = "Electronics", Price = 1200, RemainingStock = 6  },
            new Product { Id = 5, Name = "Chino",       Category = "Clothing",    Price = 350,  RemainingStock = 12 },
            new Product { Id = 6, Name = "Long Sleeve", Category = "Clothing",    Price = 500,  RemainingStock = 5  }
        };

        //
        CartItem[] cart = new CartItem[3];
        int cartCount = 0;

        // FIX #4 — Order history now stores OrderRecord (receipt number + total)
        OrderRecord[] orderHistory = new OrderRecord[20];
        int orderCount = 0;

        int receiptNumber = 1;

        while (true)
        {
            Console.WriteLine("\n=== SHOP MENU ===");
            Console.WriteLine("1. View Products");
            Console.WriteLine("2. Search Product");
            Console.WriteLine("3. Filter by Category");
            Console.WriteLine("4. Add to Cart");
            Console.WriteLine("5. Cart Menu");        // FIX #1 — replaces bare "View Cart"
            Console.WriteLine("6. Checkout");
            Console.WriteLine("7. Order History");
            Console.WriteLine("8. Exit");

            int choice = GetIntInRange("Choose: ", 1, 8);

            // 1 — View Products
            if (choice == 1)
            {
                foreach (Product p in store)
                    p.DisplayProduct();
            }

            // 2 — Search
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

            // 3 — Category Filter
            else if (choice == 3)
            {
                Console.WriteLine("\n1. Food\n2. Electronics\n3. Clothing");
                int cat = GetIntInRange("Choose category: ", 1, 3);
                string category = cat == 1 ? "Food" : cat == 2 ? "Electronics" : "Clothing";
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

            // 4 — Add to Cart
            else if (choice == 4)
            {
                if (cartCount >= cart.Length)
                {
                    Console.WriteLine("Cart is full (max 3 unique items).");
                    continue;
                }

                foreach (Product p in store)
                    p.DisplayProduct();

                int id = GetPositiveInt("Enter product ID: ");

                Product selected = null;
                foreach (Product p in store)
                    if (p.Id == id) { selected = p; break; }

                if (selected == null)
                {
                    Console.WriteLine("Product not found.");
                    continue;
                }

                int qty = GetPositiveInt("Enter quantity: ");

                // FIX #6 — Available stock = RemainingStock minus what's already in cart
                int alreadyInCart = GetCartQuantityForProduct(cart, cartCount, selected.Id);
                int availableStock = selected.RemainingStock - alreadyInCart;

                if (qty > availableStock)
                {
                    Console.WriteLine(
                        availableStock <= 0
                            ? "No more stock available (already in cart)."
                            : $"Only {availableStock} more available (already have {alreadyInCart} in cart)."
                    );
                    continue;
                }

                // FIX #3 — If product already exists in cart, update its quantity instead of duplicating
                bool merged = false;
                for (int i = 0; i < cartCount; i++)
                {
                    if (cart[i].Product.Id == selected.Id)
                    {
                        cart[i].Quantity += qty;
                        Console.WriteLine($"Updated {selected.Name} quantity to {cart[i].Quantity}.");
                        merged = true;
                        break;
                    }
                }

                if (!merged)
                {
                    cart[cartCount++] = new CartItem { Product = selected, Quantity = qty };
                    Console.WriteLine("Added to cart.");
                }
            }

            // 5 — Cart Menu (FIX #1)
            else if (choice == 5)
            {
                CartMenu(cart, ref cartCount);
            }

            // 6 — Checkout
            else if (choice == 6)
            {
                Checkout(cart, ref cartCount, store, orderHistory, ref orderCount, ref receiptNumber);
            }

            // 7 — Order History
            else if (choice == 7)
            {
                if (orderCount == 0)
                {
                    Console.WriteLine("No orders yet.");
                    continue;
                }

                Console.WriteLine("\n=== ORDER HISTORY ===");
                for (int i = 0; i < orderCount; i++)
                    Console.WriteLine(
                        $"Receipt #{orderHistory[i].ReceiptNumber:0000} - PHP {orderHistory[i].FinalTotal}"
                    );
            }

            // 8 — Exit
            else if (choice == 8)
            {
                if (GetYesOrNo("Are you sure you want to exit? (Y/N): "))
                {
                    Console.WriteLine("Thank you!");
                    break;
                }
            }
        }
    }
}