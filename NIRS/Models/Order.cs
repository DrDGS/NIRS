using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NIRS.Models;

namespace NIRS.Models
{
    public class Order
    {
        public Order()
        {
            CreationDate = DateTime.UtcNow;
        }

        public int Id { get; set; }

        [Display(Name = "Клуб")]
        [Required(ErrorMessage = "Требуется указание клуба")]
        public int ClubId { get; set; }
        [ForeignKey("ClubId")]
        public Club? Club { get; set; }

        [Display(Name = "Клиент")]
        [Required(ErrorMessage = "Требуется указание клиента")]
        public int UserId { get; set; }
        [ForeignKey("ClientId")]
        public User? User { get; set; }

        [Display(Name = "Тариф")]
        [Required(ErrorMessage = "Требуется указание тарифа")]
        public int RateId { get; set; }
        [ForeignKey("RateId")]
        public Rate? Rate { get; set; }

        [Display(Name = "Количество минут")]
        [Required(ErrorMessage = "Требуется указание количества минут")]
        public int Minutes { get; set; }

        [Display(Name = "Дата оформления")]
        [DataType(DataType.Date)]
        public DateTime? CreationDate { get; private set; }
    }
}
