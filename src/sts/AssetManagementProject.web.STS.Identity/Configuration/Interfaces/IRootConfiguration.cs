using Skoruba.IdentityServer4.Shared.Configuration.Configuration.Identity;

namespace AssetManagementProject.web.STS.Identity.Configuration.Interfaces
{
    public interface IRootConfiguration
    {
        AdminConfiguration AdminConfiguration { get; }

        RegisterConfiguration RegisterConfiguration { get; }
    }
}







