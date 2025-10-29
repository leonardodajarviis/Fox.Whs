using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fox.Whs.Data;

namespace Fox.Whs.SapModels;

[Table("OUSR"), ReadOnlyEntity]
public class User
{
    [Column("USERID"), Key]
    public Int16 Id { get; set; }

    [Column("USER_CODE")]
    public string Username { get; set; } = null!;

    [Column("U_NAME")]
    public string FullName { get; set; } = null!;

    public ICollection<UserGroupAssignment> GroupAssignments { get; set; } = [];
}

[Table("OUGR"), ReadOnlyEntity]
public class UserGroup
{
    [Column("GroupId"), Key]
    public Int16 Id { get; set; }

    [Column("GroupName")]
    public string GroupName { get; set; } = null!;
}

[Table("USR7"), ReadOnlyEntity]
public class UserGroupAssignment
{
    [Column("UserId")]
    public Int16 UserId { get; set; }

    [Column("GroupId")]
    public Int16 GroupId { get; set; }

    [ForeignKey("GroupId")]
    public UserGroup? Group { get; set; }

    [NotMapped]
    public string? GroupName => Group?.GroupName;
}