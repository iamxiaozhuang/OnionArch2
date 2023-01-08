using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Common.Exceptions
{
    public class DeleteFailureException : Exception
    {
        public DeleteFailureException(string entityName, object entityKey, string message)
          : base($"\"{entityName}({entityKey})\"  删除失败. {message}")
        {
        }
    }
}
