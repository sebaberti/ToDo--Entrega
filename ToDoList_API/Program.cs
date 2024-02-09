using Microsoft.EntityFrameworkCore;
using ToDoList_API;
using ToDoList_API.Data;
using ToDoList_API.Repository;
using ToDoList_API.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});



builder.Services.AddAutoMapper(typeof(MappingConfig));


builder.Services.AddScoped<ITaskRepositorio, TaskRepositorio>();
builder.Services.AddScoped<ICatTaskRepositorio, CatTaskRepositorio>();
builder.Services.AddScoped<ICategoriesRepositorio, CategoriesRepositorio>();

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.DateFormatString = "dd/MM/yyyy";
    });




var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowReactApp");


app.UseHttpsRedirection();



app.UseAuthorization();

app.MapControllers();

app.Run();

