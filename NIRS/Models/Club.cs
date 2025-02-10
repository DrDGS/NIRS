using System.ComponentModel.DataAnnotations;

namespace NIRS.Models
{
    public class Club
    {
        public int Id { get; set; }

        [Display(Name = "Адрес")]
        [Required(ErrorMessage = "Требуется адрес")]
        public string? Address { get; set; }

        public ICollection<Worker> Workers { get; set; } = new List<Worker>();
        public ICollection<Device> Devices { get; set; } = new List<Device>();
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}