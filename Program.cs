using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

class Program
{
    private static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        try
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Fetch products from API");
            Console.WriteLine("2. Save one product to database");
            Console.WriteLine("3. Fetch customers from API");
            Console.WriteLine("4. Save one customer to database");

            var option = Console.ReadLine();

            using (var db = new AppDbContext())
            {
                switch (option)
                {
                    case "1":
                        await FetchProductsAsync();
                        break;
                    case "2":
                        await SaveOneProductAsync(db);
                        break;
                    case "3":
                        await FetchCustomersAsync();
                        break;
                    case "4":
                        await SaveOneCustomerAsync(db);
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"A database update error occurred: {dbEx.Message}");
            if (dbEx.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {dbEx.InnerException.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task FetchProductsAsync()
    {
        try
        {
            var response = await client.GetAsync("https://eftechnical.azurewebsites.net/api/products");

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Product API Response: " + responseBody);

                
                await System.IO.File.WriteAllTextAsync("products.json", responseBody);
            }
            else
            {
                Console.WriteLine($"Failed to fetch products. Status code: {response.StatusCode}");
            }
        }
        catch (HttpRequestException httpEx)
        {
            Console.WriteLine($"HTTP Request error: {httpEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while fetching products: {ex.Message}");
        }
    }

    static async Task SaveOneProductAsync(AppDbContext db)
    {
        try
        {
            var responseBody = await System.IO.File.ReadAllTextAsync("products.json");
            var products = JsonConvert.DeserializeObject<List<Product>>(responseBody);

            if (products == null || products.Count == 0)
            {
                Console.WriteLine("No products found to save.");
                return;
            }

            Console.Write("Enter the ID of the product to save: ");
            if (int.TryParse(Console.ReadLine(), out int productId))
            {
                var product = products.Find(p => p.Id == productId);

                if (product == null)
                {
                    Console.WriteLine($"No product found with ID {productId}.");
                    return;
                }

                if (!await db.Products.AnyAsync(p => p.Id == product.Id))
                {
                    db.Products.Add(product);
                    await db.SaveChangesAsync();
                    Console.WriteLine("Product saved to the database successfully.");
                }
                else
                {
                    Console.WriteLine($"Product with ID {product.Id} already exists.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving the product: {ex.Message}");
        }
    }

    static async Task FetchCustomersAsync()
    {
        try
        {
            var response = await client.GetAsync("https://eftechnical.azurewebsites.net/api/customer");

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Customer API Response: " + responseBody);

                
                await System.IO.File.WriteAllTextAsync("customers.json", responseBody);
            }
            else
            {
                Console.WriteLine($"Failed to fetch customers. Status code: {response.StatusCode}");
            }
        }
        catch (HttpRequestException httpEx)
        {
            Console.WriteLine($"HTTP Request error: {httpEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while fetching customers: {ex.Message}");
        }
    }

    static async Task SaveOneCustomerAsync(AppDbContext db)
    {
        try
        {
            var responseBody = await System.IO.File.ReadAllTextAsync("customers.json");
            var customers = JsonConvert.DeserializeObject<List<Customer>>(responseBody);

            if (customers == null || customers.Count == 0)
            {
                Console.WriteLine("No customers found to save.");
                return;
            }

            Console.Write("Enter the ID of the customer to save: ");
            if (int.TryParse(Console.ReadLine(), out int customerId))
            {
                var customer = customers.Find(c => c.Id == customerId);

                if (customer == null)
                {
                    Console.WriteLine($"No customer found with ID {customerId}.");
                    return;
                }

                if (!await db.Customers.AnyAsync(c => c.Id == customer.Id))
                {
                    db.Customers.Add(customer);
                    await db.SaveChangesAsync();
                    Console.WriteLine("Customer saved to the database successfully.");
                }
                else
                {
                    Console.WriteLine($"Customer with ID {customer.Id} already exists.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving the customer: {ex.Message}");
        }
    }

   
}
