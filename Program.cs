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
        Console.WriteLine("Choose an option:");
        Console.WriteLine("1. Fetch products from API");
        Console.WriteLine("2. Save products to database");
        Console.WriteLine("3. Fetch customers from API");
        Console.WriteLine("4. Save customers to database");

        var option = Console.ReadLine();

        using (var db = new AppDbContext())
        {
            switch (option)
            {
                case "1":
                    await FetchProductsAsync();
                    break;
                case "2":
                    await SaveProductsAsync(db);
                    break;
                case "3":
                    await FetchCustomersAsync();
                    break;
                case "4":
                    await SaveCustomersAsync(db);
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    static async Task FetchProductsAsync()
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

    static async Task SaveProductsAsync(AppDbContext db)
    {
        var responseBody = await System.IO.File.ReadAllTextAsync("products.json");
        var products = JsonConvert.DeserializeObject<List<Product>>(responseBody);

        if (products == null || products.Count == 0)
        {
            Console.WriteLine("No products found to save.");
            return;
        }

        foreach (var product in products)
        {
            if (!await db.Products.AnyAsync(p => p.Id == product.Id))
            {
                db.Products.Add(product);
            }
            else
            {
                Console.WriteLine($"Product with ID {product.Id} already exists.");
            }
        }

        await db.SaveChangesAsync();
        Console.WriteLine("Products saved to the database successfully.");
    }

    static async Task FetchCustomersAsync()
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

    static async Task SaveCustomersAsync(AppDbContext db)
    {
        var responseBody = await System.IO.File.ReadAllTextAsync("customers.json");
        var customers = JsonConvert.DeserializeObject<List<Customer>>(responseBody);

        if (customers == null || customers.Count == 0)
        {
            Console.WriteLine("No customers found to save.");
            return;
        }

        foreach (var customer in customers)
        {
            if (!await db.Customers.AnyAsync(c => c.Id == customer.Id))
            {
                db.Customers.Add(customer);
            }
            else
            {
                Console.WriteLine($"Customer with ID {customer.Id} already exists.");
            }
        }

        await db.SaveChangesAsync();
        Console.WriteLine("Customers saved to the database successfully.");
    }
}
