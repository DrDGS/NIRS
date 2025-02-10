using System.ComponentModel.DataAnnotations;

namespace NIRS.Models
{
    public class Rate
    {
        public int Id { get; set; }

        [Display(Name = "Название тарифа")]
        [Required(ErrorMessage = "Требуется название тарифа")]
        public string? Name { get; set; }

        [Display(Name = "Описание тарифа")]
        [Required(ErrorMessage = "Требуется описание тарифа")]
        public string? Description { get; set; }

        [Display(Name = "Руб/мин")]
        [Required(ErrorMessage = "Требуется цена (руб/мин) тарифа")]
        public int? RublesPerMinute { get; set; }

        [Display(Name = "Устройство")]
        [Required(ErrorMessage = "Требуется указание устройства")]
        public string? DeviceType { get; set; }
    }
}
