using MediatR;
using OnionArch.Domain.Common.Entities;
using OnionArch.Domain.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Common.CurrentContext
{
    public interface ICurrentContext
    {
        Guid TenantId { get; }
        string UserName { get; }
    }
}
