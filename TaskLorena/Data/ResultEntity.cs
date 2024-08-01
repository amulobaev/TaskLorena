using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskLorena.Data;

public class ResultEntity
{
    [Key]
    public int Id { get; set; }

    public int SalonId { get; set; }

    [ForeignKey(nameof(SalonId))]
    public SalonEntity? Salon { get; set; }

    public double Discount { get; set; }

    public double Price { get; set; }

    public double DiscountParent { get; set; }

    public double CalculatedPrice { get; set; }
}
