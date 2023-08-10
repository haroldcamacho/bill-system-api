
using System.ComponentModel.DataAnnotations;
namespace BasicBilling.API.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Bill> Bills { get; set; } = new List<Bill>();

    }
}
