using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Common.CQRS
{

    public interface ICommand<out TResult> : IRequest<TResult>
    {
    }
}
