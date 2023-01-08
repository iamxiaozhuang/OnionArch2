using OnionArch.Domain.Common.Tenant;

namespace OnionArch.WebApi.Common.Tenant
{
    public class CurrentTenant : ICurrentTenant
    {
        public CurrentTenant(IHttpContextAccessor httpContextAccessor)
        {
            //TenantId = httpContextAccessor.HttpContext?.Request.Headers["x-tenant-Id"].ToString();
            TenantId = Guid.Parse("7213d0f8-b9e2-4c83-b3b8-e781fc329aab");
        }

        public Guid TenantId { get; }
    }
}
