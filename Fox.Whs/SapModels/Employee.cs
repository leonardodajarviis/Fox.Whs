using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Fox.Whs.Data;

namespace Fox.Whs.SapModels;

[Table("OHEM"), ReadOnlyEntity]
public class Employee
{
    [Column("empID"), Key]
    public int Id { get; set; }

    [Column("lastName")]
    public string? LastName { get; set; }

    [Column("firstName")]

    [NotMapped]
    public string? FullName => $"{LastName ?? ""} {MiddleName ?? ""} {FirstName ?? ""}";

    public string? FirstName { get; set; }

    [Column("middleName")]
    public string? MiddleName { get; set; }

    [Column("userId")]
    public int? UserId { get; set; }
}