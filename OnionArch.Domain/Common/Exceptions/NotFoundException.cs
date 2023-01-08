using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityName, object entityKey, string message)
            : base($"\"{entityName}\" ({entityKey}) 未找到.{message}")
        {
        }
    }
}
