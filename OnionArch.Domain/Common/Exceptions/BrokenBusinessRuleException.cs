using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OnionArch.Domain.Common.Exceptions
{
    public class BrokenBusinessRuleException : Exception
    {

        public BrokenBusinessRuleException(string ruleCode, string ruleNeme, string entityName, object entityKey, string message)
            : base($"\"{entityName}({entityKey}\" ) 违反了业务规则 \"{ruleNeme}({ruleCode})\". {message}")
        {
        }

    }
}
