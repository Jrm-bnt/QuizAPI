using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuizAPI.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:Key")))
        };
    });
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "V1",
        Title = "Quiz API",
        Description = "Bienvenue dans l'API Quiz, un outil interactif pour créer, jouer et suivre des jeux de quiz en ligne. Cette API vous permet de réaliser les actions suivantes :"
    + "\n\n- Créer des jeux de quiz interactifs."
    + "\n- Ajouter des questions à ces jeux."
    + "\n- Découvrer le gagnant grâce au systeme de points"
    + "\n- Gérer l'historique des parties."
    + "\n- S'identifier afin de gérer ses propres questions et parties."
    + "\n\nProfitez de cette API pour enrichir vos cours, défier vos amis ou organiser des quiz en ligne passionnants."
    + "\n\nPour plus d'informations, contactez Elmeddahi Mohamed & Brenet Jérémy à l'adresse jeremy.brenet@campus-igs-toulouse.fr - m.elmeddahi@campus-igs-toulouse.fr"
    + "\n\nDécouvrez notre site web : [Quiz API](https://jrm-bnt.github.io/react-github-pages/#/login)",
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<QuizDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

app.UseCors(options =>
    options.WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader());


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quiz API V1");
});


app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();