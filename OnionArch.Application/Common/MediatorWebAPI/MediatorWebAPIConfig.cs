using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Common.HTTP
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MediatorWebAPIConfigAttribute : Attribute
    {
        public HttpMethodToGenerate HttpMethod { get; set; }
        public string? HttpUrl { get; set; }
    }

    [Flags]
    public enum HttpMethodToGenerate
    {
        Post,
        Get,
        Put,
        Patch,
        Delete,
        Default,
    }



}
