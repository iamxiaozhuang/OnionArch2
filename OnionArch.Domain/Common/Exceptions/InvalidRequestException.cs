using System;

namespace OnionArch.Domain.Common.Exceptions
{
    public class InvalidRequestException : Exception
    {
        public string Details { get; }
        public InvalidRequestException(string message, string details) : base(message)
        {
            this.Details = details;
        }
    }
}