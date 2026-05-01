# Gutierrez_GeresMaria_ShoppingCartActivity

<img width="1059" height="1060" alt="Image" src="https://github.com/user-attachments/assets/346f7a8a-50d7-4dbe-bf79-875a6dc37684" />

This project is a simple shopping cart application that allows users to add items to their cart and view the total price. The Console-based shopping cart system built using C#.

Naming Convetion Used: Pascal Case
Flowchart Used: Process Flowchart

Details: 
Main Variables: 17
Classes: 3
Methods: 2


**AI USAGE**
1. I generated questions for errors I encountered while focusing on "Commit and Push" 
	- Fatal errors and Permission Denied.
		- "How to fix fatal error in GitHub?"
		- "How to fix permission denied error in GitHub?"
		- I pasted what git bash was showing me in the terminal to AI and asked for solutions.
		- Then I followed the steps provided by AI to resolve the issues and successfully commit and push my code to GitHub.
2. I also used AI to help me understand the concepts of GitHub and how to use it effectively for version control and collaboration.
		- "What is GitHub and how does it work?"
		- "How to use GitHub for version control?"
3. How to delete repositories: "How to delete a repository on GitHub?"

4. How to use TryParse method in C#: "How to use TryParse(int quantity) for example in C#?"

5. I also asked about classes
	- Which is more efficient: Using files for classes or using a single file for all classes in C#?
6. "What is the best type of flowchart to use for a shopping cart application?"

// 
In conclusion, most of the questions I asked were about introduction to what I don't know yet and how to fix errors I encountered while using GitHub.




PART 2 AI USAGE 

1. Cart quantity merge logic
"How do I avoid duplicate cart entries for the same product?" Added a loop that checks if the product already exists in the cart and merges the quantity instead of adding a new entry.

2. Stock validation against cart
"How do I prevent adding more stock than available when the item is already in the cart?" Introduced GetCartQuantityForProduct() to subtract already-carted quantities from available stock before allowing an add.

3. Cart item removal with array shifting
"How do I remove an item from a fixed array without leaving gaps?" Used a left-shift loop to fill the removed item's slot and set the last element to null.

4.Cart array shifting on removal
 "How do I delete an element from an array without leaving a null gap in the middle?" Used a for loop that shifts every element one index to the left, then sets the last slot to null and decrements cartCount.Preventing overselling with cart-aware stock check

5. Preventing overselling with cart-aware stock check
"How do I check available stock while accounting for quantities already in the cart?" Created GetCartQuantityForProduct() which sums existing cart quantities for a product, then subtracts that from RemainingStock before allowing more to be added.

6.  Merging duplicate cart entries
 "How do I update quantity instead of adding a duplicate when the same product is added twice?" Looped through the cart to find a matching product ID, incremented its quantity if found, and only created a new CartItem if no match existed.

7. Receipt number with leading zeros
"How do I display a receipt number with leading zeros like #0001?" Used C#'s custom numeric format specifier :0000 inside an interpolated string, which pads the number with zeros up to 4 digits automatically.

8. Checkout flow with stock validation
"How do I validate stock availability before processing checkout?" Looped through cart items using GetCartQuantityForProduct() to compare total requested quantity against RemainingStock, cancelling checkout if any item falls short.


Summary of the Part 2 Changes: ------------------------------------------------

Part 2 expanded the system with the following changes:

1. Main Menu System
Added a full loop-based shop menu with 8 options instead of a single linear flow, giving the user more control over navigation.
2. Cart Management Submenu
Added a dedicated Cart Menu with options to view, update quantity, remove a specific item, and clear the entire cart — none of these existed in Part 1.
3. Search and Category Filter
Added the ability to search products by name and filter them by category (Food, Electronics, Clothing).
4. Checkout as a Separate Function
Moved checkout logic into its own Checkout() method with stock re-validation, payment input loop, change computation, and receipt printing with formatted receipt numbers like #0001.
5. Order History
Added an OrderRecord class and orderHistory array that saves every completed transaction and displays it under Order History.
6. Low Stock Alert
Added a post-checkout alert that notifies which products are running low (5 or fewer remaining).
7. Product Class Methods
Added methods like CalculateSubTotal(), MatchesName(), MatchesCategory(), and IsLowStock() to the Product class to support the new features.



OUTPUT SAMPLE 

<img width="349" height="1029" alt="Image" src="https://github.com/user-attachments/assets/1f560010-a6d3-42ad-8126-d7f2dc3c90da" />