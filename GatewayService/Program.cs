var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Gateway Service is running");

app.MapGet("/gateway/products", async (IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient();

    var response = await client.GetAsync("http://productservice:8080/api/products");

    if (!response.IsSuccessStatusCode)
    {
        return Results.Problem("Product Service is not available");
    }

    var data = await response.Content.ReadAsStringAsync();
    return Results.Content(data, "application/json");
});

app.MapGet("/gateway/customers", async (IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient();

    var response = await client.GetAsync("http://customerservice:8080/api/customers");

    if (!response.IsSuccessStatusCode)
    {
        return Results.Problem("Customer Service is not available");
    }

    var data = await response.Content.ReadAsStringAsync();
    return Results.Content(data, "application/json");
});


app.MapGet("/gateway/orders", async (IHttpClientFactory httpClientFactory) =>
{
    var client = httpClientFactory.CreateClient();

    var response = await client.GetAsync("http://orderservice:8080/api/orders");

    if (!response.IsSuccessStatusCode)
    {
        return Results.Problem("Order Service is not available");
    }

    var data = await response.Content.ReadAsStringAsync();
    return Results.Content(data, "application/json");
});

app.Run();