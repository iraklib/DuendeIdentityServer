using Duende.IdentityServer.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddIdentityServer(
    options =>
    {
        options.UserInteraction.LoginUrl = "/Account/Login";
        options.UserInteraction.LogoutUrl = "/Account/Logout";
    })
    .AddInMemoryClients(new List<Client>
    {
        new Client
        {
            ClientId = "client_id",
            AllowedGrantTypes = GrantTypes.ClientCredentials, // OAuth2 Authorization Code Grant
            ClientSecrets = { new Secret("secret".Sha256()) },
            RedirectUris = { "https://localhost:44303/signin-oidc" }, // Client redirect URI
            PostLogoutRedirectUris = { "https://localhost:44303/signout-callback-oidc" },
            AllowedScopes = { "openid", "profile", "api1" }, // OpenID Connect scopes
            AllowOfflineAccess = true, // Enable refresh tokens
            RequirePkce = false
        }
    })
    .AddInMemoryApiScopes(new List<ApiScope>
    {
        new ApiScope("api1", "My API")
    })
    .AddInMemoryIdentityResources(new List<IdentityResource>
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
    });

builder.Services.AddControllers();
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
app.UseIdentityServer(); // Enable IdentityServer
app.UseAuthorization();

app.MapControllers();

app.Run();
