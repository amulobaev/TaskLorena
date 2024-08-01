using System.ComponentModel.DataAnnotations;

namespace TaskLorena.Data;

public class SalonEntity
{
    [Key]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    public double Discount { get; set; }

    public int? ParentId { get; set; }

    public bool Dependency { get; set; }

    [MaxLength(124)]
    public string? Description { get; set; }
}
