using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Common.Database
{

    public class ReadEntityRequest<TEntity> : IRequest<TEntity>
    {
        public ReadEntityRequest(Guid id) 
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
