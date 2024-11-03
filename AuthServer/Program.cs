using Duende.IdentityServer.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddIdentityServer()
    .AddInMemoryClients(new List<Client>
    {
        new Client
        {
            ClientId = "MPC",
            AllowedGrantTypes = GrantTypes.ClientCredentials, // OAuth2 Authorization Code Grant
            ClientSecrets = { new Secret("secret".Sha256()) },

            AllowedScopes = { "api1" }, // OpenID Connect scopes
            AllowOfflineAccess = true, // Enable refresh tokens
            RequirePkce = false
        }
    })
    .AddInMemoryApiScopes(new List<ApiScope>
    {
        new ApiScope("api1", "My API")
    });

var app = builder.Build();

app.UseIdentityServer(); // Enable IdentityServer

app.Run();
