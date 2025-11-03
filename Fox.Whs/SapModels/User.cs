using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Fox.Whs.Data;

namespace Fox.Whs.SapModels;

[Table("OUSR"), ReadOnlyEntity]
public class User
{
    [Column("USERID"), Key]
    public short Id { get; set; }

    [Column("USER_CODE")]
    public string Username { get; set; } = null!;

    [Column("U_NAME")]
    public string FullName { get; set; } = null!;

    [NotMapped]
    public Employee? EmployeeInfo { get; set; }

    public ICollection<UserPermission> Permissions { get; set; } = [];
}

[Table("@PHANQUYENWMS_L"), ReadOnlyEntity]
public class UserPermission
{
    [Column("Code")]
    public string Code { get; set; } = null!;

    [Column("LineId")]
    public int LineId { get; set; }

    [Column("U_MUSER")]
    public short UserId { get; set; }

    [ForeignKey("UserId"), JsonIgnore]
    public User? UserDetail { get; set; }

    [Column("U_MBC")]
    public string? Module { get; set; }

    [Column("U_VT")]
    public string? Permission { get; set; }
}
