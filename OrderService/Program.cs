var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var orders = new List<Order>
{
    new Order(1, 1, 1, 2, 2400),
    new Order(2, 2, 3, 1, 45)
};

app.MapGet("/", () => "Order Service is running");

app.MapGet("/api/orders", () =>
{
    return Results.Ok(orders);
});

app.MapGet("/api/orders/{id}", (int id) =>
{
    var order = orders.FirstOrDefault(o => o.Id == id);

    if (order == null)
    {
        return Results.NotFound("Order not found");
    }

    return Results.Ok(order);
});

app.MapPost("/api/orders", (Order order) =>
{
    orders.Add(order);
    return Results.Created($"/api/orders/{order.Id}", order);
});

app.MapPut("/api/orders/{id}", (int id, Order updatedOrder) =>
{
    var index = orders.FindIndex(o => o.Id == id);

    if (index == -1)
    {
        return Results.NotFound("Order not found");
    }

    orders[index] = updatedOrder;
    return Results.Ok(updatedOrder);
});

app.MapDelete("/api/orders/{id}", (int id) =>
{
    var order = orders.FirstOrDefault(o => o.Id == id);

    if (order == null)
    {
        return Results.NotFound("Order not found");
    }

    orders.Remove(order);
    return Results.Ok("Order deleted successfully");
});

app.Run();

record Order(int Id, int CustomerId, int ProductId, int Quantity, decimal TotalPrice);