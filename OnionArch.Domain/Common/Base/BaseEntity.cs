using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Common.Base
{
    public record BaseEntity
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid TenantId { get; set; }


    }
}
