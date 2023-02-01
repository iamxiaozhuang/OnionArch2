using Microsoft.EntityFrameworkCore.Infrastructure;
using OnionArch.Domain.Common.CurrentContext;

namespace OnionArch.WebApi.Common.CurrentContext
{
    public class CurrentContext : ICurrentContext
    {
        public CurrentContext(IHttpContextAccessor httpContextAccessor)
        {
            //TenantId =  Guid.Parse(httpContextAccessor.HttpContext?.Request.Headers["x-tenant-Id"].ToString());
            TenantId =  Guid.Parse("7213d0f8-b9e2-4c83-b3b8-e781fc329aab");

            //UserName =  httpContextAccessor.HttpContext?.User.Identity.Name;
            UserName =  "jerry";
        }

        public Guid TenantId { get; }

        public string UserName { get; }

    }
}
