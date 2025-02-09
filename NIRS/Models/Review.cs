using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NIRS.Models;

namespace NIRS.Models
{
    public class Review
    {
        public Review()
        {
            CreationDate = DateTime.UtcNow;
        }

        public int Id { get; set; }

        [Display(Name = "Клуб")]
        [Required(ErrorMessage = "Требуется указание клуба")] public int ClubId { get; set; }
        [ForeignKey("ClubId")]
        public Club? Club { get; set; }

        [Display(Name = "Клиент")]
        [Required(ErrorMessage = "Требуется указание клиента")] 
        public int UserId { get; set; }
        [ForeignKey("ClientId")]
        public User? User { get; set; }

        [Display(Name = "Оценка")]
        [Range(1, 5, ErrorMessage = "Оценка должна быть от 1 до 5 звёзд")]
        [Required(ErrorMessage = "Требуется оценка")]
        public int Rating { get; set; }

        [Display(Name = "Текст отзыва")]
        [StringLength(3000, ErrorMessage = "Текст должен быть не более 3000 символов")]
        public string Text { get; set; } = string.Empty;

        [Display(Name = "Дата отзыва")]
        [DataType(DataType.Date)]
        public DateTime? CreationDate { get; private set; }
    }
}
