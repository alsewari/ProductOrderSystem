var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var products = new List<Product>
{
    new Product(1, "Laptop", 1200),
    new Product(2, "Mouse", 25),
    new Product(3, "Keyboard", 45)
};

app.MapGet("/", () => "Product Service is running");

app.MapGet("/api/products", () =>
{
    return Results.Ok(products);
});

app.MapGet("/api/products/{id}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);

    if (product == null)
    {
        return Results.NotFound("Product not found");
    }

    return Results.Ok(product);
});

app.MapPost("/api/products", (Product product) =>
{
    products.Add(product);
    return Results.Created($"/api/products/{product.Id}", product);
});

app.MapPut("/api/products/{id}", (int id, Product updatedProduct) =>
{
    var index = products.FindIndex(p => p.Id == id);

    if (index == -1)
    {
        return Results.NotFound("Product not found");
    }

    products[index] = updatedProduct;
    return Results.Ok(updatedProduct);
});

app.MapDelete("/api/products/{id}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);

    if (product == null)
    {
        return Results.NotFound("Product not found");
    }

    products.Remove(product);
    return Results.Ok("Product deleted successfully");
});

app.Run();

record Product(int Id, string Name, decimal Price);