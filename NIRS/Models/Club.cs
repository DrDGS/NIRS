using System.ComponentModel.DataAnnotations;

namespace NIRS.Models
{
    public class Club
    {
        public int Id { get; set; }

        [Display(Name = "Адрес")]
        [Required(ErrorMessage = "Требуется адрес")]
        public string? Address { get; set; }
    }
}