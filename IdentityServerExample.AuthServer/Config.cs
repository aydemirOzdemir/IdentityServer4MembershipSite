using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace IdentityServerExample.AuthServer;

public static class Config
{
    public static IEnumerable<ApiResource> GetApiResources()
    {
        return new List<ApiResource>()
        {
            new ApiResource("resource_api1")
            {
                Scopes={ "api1.read", "api1.write", "api1.update" },
                ApiSecrets=new[]{new Secret("secretapi1".Sha256()
                )}
            },
            new ApiResource("resource_api2")
            {
                Scopes={ "api2.read", "api2.write", "api2.update" },
                 ApiSecrets=new[]{new Secret("secretapi2".Sha256()
                )}
            }
        };
    }
    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new List<ApiScope>()
        {
            new ApiScope("api1.read","API 1 için okuma izni"),
            new ApiScope("api1.write","API 1 için yazma izni"),
            new ApiScope("api1.update","API 1 için güncelleme izni"),

               new ApiScope("api2.read","API 2 için okuma izni"),
            new ApiScope("api2.write","API 2 için yazma izni"),
            new ApiScope("api2.update","API 2 için güncelleme izni"),
        };
    }


    public static IEnumerable<Client> GetClient()
    {
        return new List<Client>()
        {
            new Client() {ClientId="client1",
                ClientSecrets=new[]{new Secret("secret".Sha256())},
            ClientName="Client 1 uygulaması",
                AllowedGrantTypes=GrantTypes.ClientCredentials,
            AllowedScopes= { "api1.read" , "api2.write", "api2.update" }
             },

              new Client() {ClientId="client2",
                ClientSecrets=new[]{new Secret("secret".Sha256())},
            ClientName="Client 2 uygulaması",
                AllowedGrantTypes=GrantTypes.ClientCredentials,
            AllowedScopes= { "api1.read" , "api2.write", "api2.update" }
             },

               new Client() {ClientId="Client1.Mvc",
                ClientSecrets=new[]{new Secret("secret".Sha256())},
            ClientName="Client 1 mvc  uygulaması",
            RequirePkce=false,
                AllowedGrantTypes=GrantTypes.Hybrid,
         RedirectUris=new List<string>(){ "https://localhost:7264/signin-oidc" },//token alma işlemini gerçekleştiren url
         PostLogoutRedirectUris=new List<string>{ "https://localhost:7264/signout-callback-oidc" },//çıkış yaptığında hem şuan kullandığın clienttan çıkış yaparsın hem de authserverdan çıkış yaparsın.
          AllowedScopes= {IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile , "api1.read" ,IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles"},
          AccessTokenLifetime=2*60*60,//access tokenın süresi saniye cinsinden
               AllowOfflineAccess=true,//refres token için
               RefreshTokenUsage=TokenUsage.ReUse,
               RefreshTokenExpiration=TokenExpiration.Absolute,
               AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
               RequireConsent=true,//onay sayfası için 3th party için
             },




                     new Client() {ClientId="Client2.Mvc",
                ClientSecrets=new[]{new Secret("secret".Sha256())},
            ClientName="Client 2 mvc  uygulaması",
            RequirePkce=false,
                AllowedGrantTypes=GrantTypes.Hybrid,
         RedirectUris=new List<string>(){ "https://localhost:7106/signin-oidc" },//token alma işlemini gerçekleştiren url
         PostLogoutRedirectUris=new List<string>{ "https://localhost:7106/signout-callback-oidc" },//çıkış yaptığında hem şuan kullandığın clienttan çıkış yaparsın hem de authserverdan çıkış yaparsın.
          AllowedScopes= {IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile , "api2.read","api1.read" ,IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles"},
          AccessTokenLifetime=2*60*60,//access tokenın süresi saniye cinsinden
               AllowOfflineAccess=true,//refres token için
               RefreshTokenUsage=TokenUsage.ReUse,
               RefreshTokenExpiration=TokenExpiration.Absolute,
               AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
               RequireConsent=true,//onay sayfası için 3th party için
             }


          };
    }


    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>()
{
    new IdentityResources.OpenId(),
    new IdentityResources.Profile(),
    new IdentityResource(){Name="CountryAndCity",DisplayName="Country and City",Description="Kullanıcının yaşadığı ülke ve şehir",UserClaims=new[] {"Country","City"}},
    new IdentityResource(){Name="Roles",DisplayName="Roles",Description="Kullanıcı Rolleri",UserClaims=new[]{"role"}}
};
    }

    public static IEnumerable<TestUser> GetUsers()
    {
        return new List<TestUser>()
        {
            new TestUser
            {
                SubjectId = "1",
                Username = "aydemirÖzdemir",
                Password = "Password12*",
                Claims = new List<Claim>()
                {
                    new Claim("given_name", "Aydemir"),
                    new Claim("family_name", "Özdemir"),
                    new Claim("Country","Türkiye"),
                    new Claim("City","Bursa"),
                    new Claim("role","Admin")
                }
            },

            new TestUser
            {
                SubjectId = "2",
                Username = "alpartunÖzdemir",
                Password = "Password12*",
                Claims = new List<Claim>()
                {
                    new Claim("given_name", "Alpartun"),
                    new Claim("family_name", "Özdemir"),
                             new Claim("Country","Türkiye"),
                    new Claim("City","Bursa"),
                    new Claim("role","customer")

                }
            }


        };
    }


}
