using System.Data.SqlClient;
using teste.Repositories;
using teste.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRepositiory, Repository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddHttpContextAccessor();

// Configure the HTTP request pipeline.
var app = builder.Build();

// Setup database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
using (var connection = new SqlConnection(connectionString))
{
    connection.Open();
    var command = connection.CreateCommand();

    // Cria as tabelas se elas não existirem, PARA FUNCIONAR É PRECISO QUE CRIE UM BANCO DE DADOS COM O NOME QUE QUISER E DEPOIS COLOCAR ESSE NOME
    // NOME NA CONNECTION_STRING,E NA QUERY ABAIXO ANTES DE INICIAR O PROJETO
    command.CommandText = @"
        USE teste123;

        IF NOT EXISTS(SELECT * FROM sys.tables WHERE name = 'Usuario')
        BEGIN
            CREATE TABLE Usuario (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Nome NVARCHAR(255) NOT NULL,
                Email NVARCHAR(255) NOT NULL
            );
        END;

        IF NOT EXISTS(SELECT * FROM sys.tables WHERE name = 'Tarefas')
        BEGIN
            CREATE TABLE Tarefas (
                Id INT PRIMARY KEY IDENTITY(1,1),
                IdUsuario INT NOT NULL,
                Titulo NVARCHAR(255) NOT NULL,
                Descricao NVARCHAR(MAX) NOT NULL,
                FOREIGN KEY (IdUsuario) REFERENCES Usuario(Id)
            );
        END;
    ";
    command.ExecuteNonQuery();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => options.WithOrigins("http://localhost:5173/").AllowAnyHeader().AllowAnyMethod());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
