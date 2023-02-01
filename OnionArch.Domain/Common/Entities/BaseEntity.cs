using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Common.Entities
{
    public record BaseEntity : IEntity
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid TenantId { get; set; }

    }
}
