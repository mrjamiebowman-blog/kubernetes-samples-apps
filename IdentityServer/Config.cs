using Duende.IdentityServer.Models;

namespace MrJB.Kubernetes.IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            /* customers */
            new ApiScope("api.customers"),
            new ApiScope("consumer.orders"),
            /* orders */
            new ApiScope("api.orders"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "postman.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("9B7B2D08-829C-4F03-BA38-BC9A25DDEA99".Sha256()) },

                AllowedScopes = { "api.customers", "api.orders", "consumer.orders" }
            },

            // api: customers
            new Client
            {
                ClientId = "api.customers",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("C86ABBFF-629B-4FBB-9E01-6EBE65E579F8".Sha256()) },

                AllowedScopes = { "api.customers" }
            },

            // api: orders
            new Client
            {
                ClientId = "api.orders",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("C58F5C93-3B9E-4E0B-BEAE-FDBBDF5BC333".Sha256()) },

                AllowedScopes = { "api.orders" }
            },

            // consumers: customers
            new Client
            {
                ClientId = "consumers.customers",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("50BBF80B-1834-4896-B6FC-3D6D1AEB1AF4".Sha256()) },

                AllowedScopes = { "consumer.orders" }
            },

            // app: identityserver
            new Client
            {
                ClientId = "app.identityserver",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("3B9E750B-32AC-4181-8C7F-6028266123F3".Sha256()) },

                AllowedScopes = { "scope1" }
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "interactive",
                ClientSecrets = { new Secret("BF07D2F6-EAB4-4E60-ABC5-5998921F646C\"".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:44300/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "consumer.orders", "api.customers",  "api.orders" }
            },
        };
}
