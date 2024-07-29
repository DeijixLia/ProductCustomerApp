﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

class Program
{
    private static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        try
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Add a new product");
            Console.WriteLine("2. Add a new customer");
            Console.WriteLine("3. View all products");
            Console.WriteLine("4. View all customers");
            Console.WriteLine("5. View last added product");
            Console.WriteLine("6. View last added customer");

            var option = Console.ReadLine();

            using (var db = new AppDbContext())
            {
                switch (option)
                {
                    case "1":
                        await AddNewProductAsync(db);
                        await ViewAllProductsAsync(db); 
                        break;
                    case "2":
                        await AddNewCustomerAsync(db);
                        await ViewAllCustomersAsync(db); 
                        break;
                    case "3":
                        await ViewAllProductsAsync(db);
                        break;
                    case "4":
                        await ViewAllCustomersAsync(db);
                        break;
                    case "5":
                        await ViewLastAddedProductAsync(db);
                        break;
                    case "6":
                        await ViewLastAddedCustomerAsync(db);
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task AddNewProductAsync(AppDbContext db)
    {
        Console.WriteLine("Enter product details:");
        Console.Write("Title: ");
        var title = Console.ReadLine();
        Console.Write("Price: ");
        var price = decimal.Parse(Console.ReadLine());
        Console.Write("Description: ");
        var description = Console.ReadLine();
        Console.Write("Category: ");
        var category = Console.ReadLine();

        var product = new Product { Title = title, Price = price, Description = description, Category = category };
        db.Products.Add(product);
        await db.SaveChangesAsync();
        Console.WriteLine("Product added successfully.");
    }

    static async Task AddNewCustomerAsync(AppDbContext db)
    {
        Console.WriteLine("Enter customer details:");
        Console.Write("Name: ");
        var name = Console.ReadLine();

        var customer = new Customer { Name = name };
        db.Customers.Add(customer);
        await db.SaveChangesAsync();
        Console.WriteLine("Customer added successfully.");
    }

    static async Task ViewAllProductsAsync(AppDbContext db)
    {
        var products = await db.Products.ToListAsync();
        Console.WriteLine("Products:");
        foreach (var product in products)
        {
            Console.WriteLine($"{product.Id} - {product.Title} - {product.Price} - {product.Description} - {product.Category}");
        }
    }

    static async Task ViewAllCustomersAsync(AppDbContext db)
    {
        var customers = await db.Customers.ToListAsync();
        Console.WriteLine("Customers:");
        foreach (var customer in customers)
        {
            Console.WriteLine($"{customer.Id} - {customer.Name}");
        }
    }

    static async Task ViewLastAddedProductAsync(AppDbContext db)
    {
        var product = await db.Products.OrderByDescending(p => p.Id).FirstOrDefaultAsync();
        if (product != null)
        {
            Console.WriteLine("Last added product:");
            Console.WriteLine($"{product.Id} - {product.Title} - {product.Price} - {product.Description} - {product.Category}");
        }
        else
        {
            Console.WriteLine("No products found.");
        }
    }

    static async Task ViewLastAddedCustomerAsync(AppDbContext db)
    {
        var customer = await db.Customers.OrderByDescending(c => c.Id).FirstOrDefaultAsync();
        if (customer != null)
        {
            Console.WriteLine("Last added customer:");
            Console.WriteLine($"{customer.Id} - {customer.Name}");
        }
        else
        {
            Console.WriteLine("No customers found.");
        }
    }
}
