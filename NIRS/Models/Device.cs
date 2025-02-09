using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NIRS.Models;

namespace NIRS.Models
{
    public class Device
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Требуется название устройства")]
        public string? Name { get; set; }

        [Display(Name = "Клуб")]
        [Required(ErrorMessage = "Требуется указание клуба")]
        public int ClubId { get; set; }
        [ForeignKey("ClubId")]
        public Club? Club { get; set; }

        [Display(Name = "Работоспособность")]
        [Required(ErrorMessage = "Требуется указание состояния устройства")]
        public bool isWorking { get; set; }
    }
}
