using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
    public string? FirstName { get; set; }

    [Column("middleName")]
    public string? MiddleName { get; set; }
}