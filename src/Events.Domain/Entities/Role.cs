namespace Events.Domain.Entities;

public class Role : BaseEntity
{
    public string Title { get; set; } = null!;

    public virtual ICollection<AccountRole> AccountRoles { get; set; } = [];
}
