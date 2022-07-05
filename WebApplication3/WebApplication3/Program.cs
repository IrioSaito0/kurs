using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

    List<Events> data = JsonConvert.DeserializeObject<List<Events>>(File.ReadAllText(@"E:\Yaromyr\VisStudio\projects\WebApplication3\WebApplication3\json.json"));

app.MapGet("/event/", (string Own) =>
{
    int n = data.Count;
    List<Events> Give = new List<Events>(); 
    for (int i = 0; i < n; i++)
    {
        if (data[i].Owner == Own) Give.Add(data[i]);
    }
    return Results.Json(Give);
});

app.MapPost("/event", (Events newevent) =>
{
    data.Add(newevent);
    File.WriteAllText(@"E:\Yaromyr\VisStudio\projects\WebApplication3\WebApplication3\json.json", JsonConvert.SerializeObject(data));
    return newevent;
});

app.MapPut("/event", (string Owner, string Name, Events changeevent) =>
{
    //List<Events> data = JsonConvert.DeserializeObject<List<Events>>(File.ReadAllText(@"E:\Yaromyr\VisStudio\projects\WebApplication3\WebApplication3\json.json"));
    //var check = data.FirstOrDefault(u => u.Name == changeevent.Name);
    //bool check2 = (check.Owner == changeevent.Owner);
    //// если не найден, отправляем статусный код и сообщение об ошибке
    //if ((check == null) || (check2 == false)) return Results.NotFound(new { message = "Not found" });
    //// если пользователь найден, изменяем его данные и отправляем обратно клиенту

    //File.WriteAllText(@"E:\Yaromyr\VisStudio\projects\WebApplication3\WebApplication3\json.json", JsonConvert.SerializeObject(data));
    //return Results.Json(check);

    List<Events?> check = data.FindAll(u => u.Name == Name);
    bool key = false;
    for (int i = 0; i < check.Count; i++)
    {
        bool check2 = (check[i].Owner == Owner);
        bool check3 = (check[i].Owner == changeevent.Owner);
        if ((check != null) || (check2 != false) || (check3 != false)) { key = true; }
        if (key == true)
        {
            check[i].Name = changeevent.Name;
            check[i].Category = changeevent.Category;
            check[i].Location = changeevent.Location;
            check[i].Labels = changeevent.Labels;
            check[i].Status = changeevent.Status;
            check[i].Begin = changeevent.Begin;
            check[i].End = changeevent.End;
            File.WriteAllText(@"E:\Yaromyr\VisStudio\projects\WebApplication3\WebApplication3\json.json", JsonConvert.SerializeObject(data));
            return Results.Json(check[i]);
            break;
        }

    }
    return Results.NotFound(new { message = "Not found" });
});

app.MapPut("/event/end", (string Owner, string Name) =>
{
    //List<Events> data = JsonConvert.DeserializeObject<List<Events>>(File.ReadAllText(@"E:\Yaromyr\VisStudio\projects\WebApplication3\WebApplication3\json.json"));
    //Events? check = data.FirstOrDefault(u => u.Name == changeevent.Name);
    //bool check2 = (check.Owner == changeevent.Owner);
    //// если не найден, отправляем статусный код и сообщение об ошибке
    //if ((check == null) || (check2 == false)) return Results.NotFound(new { message = "Not found" });
    //// если пользователь найден, изменяем его данные и отправляем обратно клиенту
    //check.Status = "ended";
    //File.WriteAllText(@"E:\Yaromyr\VisStudio\projects\WebApplication3\WebApplication3\json.json", JsonConvert.SerializeObject(data));
    //return Results.Json(check);

    List<Events?> check = data.FindAll(u => u.Name == Name);
    bool key = false;
    for (int i = 0; i < check.Count; i++)
    {
        bool check2 = (check[i].Owner == Owner);
        if ((check != null) || (check2 != false)) { key = true;}
        if (key == true)
        {
            check[i].Status = "ended";
            File.WriteAllText(@"E:\Yaromyr\VisStudio\projects\WebApplication3\WebApplication3\json.json", JsonConvert.SerializeObject(data));
            return Results.Json(check[i]);
            break;
        }

    }
    return Results.NotFound(new { message = "Not found" });
});

app.MapDelete("/event/", ([FromBody]Events changeevent) =>
{
    List<Events> check = data.FindAll(u => u.Name == changeevent.Name);
    bool key = false;
    for (int i = 0; i < check.Count; i++)
    {
        bool check2 = (check[i].Owner == changeevent.Owner);
        if ((check != null) || (check2 != false)) { key = true; }
        if (key == true)
        {
            data.Remove(check[i]);
            File.WriteAllText(@"E:\Yaromyr\VisStudio\projects\WebApplication3\WebApplication3\json.json", JsonConvert.SerializeObject(data));
            return Results.Json(check[i]);
            break;
        }

    }
        return Results.NotFound(new { message = "Not found" });
});

app.Run();

class Events
{
        public string Owner { get; set; }
    public string Name { get; set; }
    public string? Category { get; set; }
        public string? Location { get; set; }
        public List<string>? Labels { get; set; }
        public string? Status { get; set; }
        public DateTime? Begin { get; set; }
        public DateTime? End { get; set; }
    public Events(string owner, string name, string? category, string? location, List<string>? labels, string? status, DateTime? begin, DateTime? end)
    {
        Owner = owner;
        Name = name;
        Category = category;
        Location = location;
        Labels = labels;
        Status = status;
        Begin = begin;
        End = end;

    }

}