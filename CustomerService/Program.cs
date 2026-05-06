var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var customers = new List<Customer>
{
    new Customer(1, "Ali Ahmed", "ali@email.com"),
    new Customer(2, "Sara Mohamed", "sara@email.com")
};

app.MapGet("/", () => "Customer Service is running");

app.MapGet("/api/customers", () =>
{
    return Results.Ok(customers);
});

app.MapGet("/api/customers/{id}", (int id) =>
{
    var customer = customers.FirstOrDefault(c => c.Id == id);

    if (customer == null)
    {
        return Results.NotFound("Customer not found");
    }

    return Results.Ok(customer);
});

app.MapPost("/api/customers", (Customer customer) =>
{
    customers.Add(customer);
    return Results.Created($"/api/customers/{customer.Id}", customer);
});

app.MapPut("/api/customers/{id}", (int id, Customer updatedCustomer) =>
{
    var index = customers.FindIndex(c => c.Id == id);

    if (index == -1)
    {
        return Results.NotFound("Customer not found");
    }

    customers[index] = updatedCustomer;
    return Results.Ok(updatedCustomer);
});

app.MapDelete("/api/customers/{id}", (int id) =>
{
    var customer = customers.FirstOrDefault(c => c.Id == id);

    if (customer == null)
    {
        return Results.NotFound("Customer not found");
    }

    customers.Remove(customer);
    return Results.Ok("Customer deleted successfully");
});

app.Run();

record Customer(int Id, string FullName, string Email);