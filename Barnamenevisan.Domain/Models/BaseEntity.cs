using System.ComponentModel.DataAnnotations;

namespace Barnamenevisan.Domain.Models;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
    public DateTime RegisterDate { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; }
}