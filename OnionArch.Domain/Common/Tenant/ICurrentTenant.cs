using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Common.Tenant
{
    public interface ICurrentTenant
    {
        Guid TenantId { get; }
    }
}
