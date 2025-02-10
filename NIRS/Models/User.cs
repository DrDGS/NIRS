using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIRS.Models
{
    public class User
    {
        public User()
        {
            Role = "User";
            Balance = 0;
        }

        public int Id { get; set; }

        [Display(Name = "ФИО/ФИ")]
        [RegularExpression(@"^\s*\S+(\s+\S+)+\s*$", ErrorMessage = "Должно быть минимум 2 слова (Фамилия и Имя)")]
        [Required(ErrorMessage = "Требуется полное имя (ФИО или ФИ)")]
        public string? FullName { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Требуется адрес электронной почты")]
        public string? EmailAddress { get; set; }

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [Required]
        public string? Password { get; set; }

        [Display(Name = "Роль")]
        [Required(ErrorMessage = "Требуется указание роли")]
        public string? Role { get; set; }

        [Display(Name = "Баланс")]
        public int? Balance { get; set; }

        public ICollection<Order> Orders { get; } = new List<Order>();
    }
}