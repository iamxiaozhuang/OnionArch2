namespace OnionArch.Domain.Common.Entities
{
    public interface IEntity
    {
        Guid Id { get; set; }
        Guid TenantId { get; set; }
    }
}