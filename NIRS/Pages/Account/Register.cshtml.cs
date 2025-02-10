using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NIRS.Data;
using NIRS.Models;
using Microsoft.AspNetCore.Identity;
using NIRS.Models;
using System.ComponentModel.DataAnnotations;
using System;

namespace NIRS.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly NIRSContext _context;

        public RegisterModel(NIRSContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "���/��")]
            [RegularExpression(@"^\s*\S+(\s+\S+)+\s*$", ErrorMessage = "������ ���� ������� 2 ����� (������� � ���)")]
            [Required(ErrorMessage = "��������� ������ ��� (��� ��� ��)")]
            public string? FullName { get; set; }

            [Display(Name = "Email")]
            [DataType(DataType.EmailAddress)]
            [Required(ErrorMessage = "��������� ����� ����������� �����")]
            public string? EmailAddress { get; set; }

            [Display(Name = "������")]
            [DataType(DataType.Password)]
            [Required]
            public string? Password { get; set; }

            [Display(Name = "����")]
            public string? Role { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingUser = await _context.User.FirstOrDefaultAsync(u => u.EmailAddress == Input.EmailAddress);
            if (existingUser != null)
            {
                ModelState.AddModelError("Input.Email", "Email already exists.");
                return Page();
            }

            var user = new User
            {
                FullName = Input.FullName,
                EmailAddress = Input.EmailAddress,
                Password = Input.Password,
                Role = "User",
                Balance = 0
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Account/Login");
        }
    }
}