using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using NIRS.Data;
using NIRS.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System;

namespace NIRS.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly NIRSContext _context;

        public LoginModel(NIRSContext context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            var user = await _context.User.FirstOrDefaultAsync(u => u.EmailAddress == Input.Email);
            if (user == null || Input.Password != user.Password)
            {
                var worker = await _context.Worker.FirstOrDefaultAsync(u => u.EmailAddress == Input.Email);

                if (worker != null && Input.Password == worker.Password)
                {
                    var claims_worker = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, worker.Id.ToString()),
                        new Claim(ClaimTypes.Email, worker.EmailAddress),
                        new Claim(ClaimTypes.Name, worker.FullName),
                        new Claim(ClaimTypes.Role, worker.Role),
                        new Claim("ClubId", worker.ClubId.ToString()),
                        new Claim("Balance", "0")
                    };

                    var identity_worker = new ClaimsIdentity(claims_worker, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal_worker = new ClaimsPrincipal(identity_worker);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal_worker);

                    var balanceClaim_worker = User.Claims.FirstOrDefault(u => u.Type == "Balance");
                    if (balanceClaim_worker != null && int.TryParse(balanceClaim_worker.Value, out int balance_worker))
                    {
                        ViewData["ClubId"] = balance_worker;
                    }
                    var clubIdClaim_worker = User.Claims.FirstOrDefault(u => u.Type == "ClubId");
                    if (clubIdClaim_worker != null && int.TryParse(clubIdClaim_worker.Value, out int clubId_worker))
                    {
                        ViewData["ClubId"] = clubId_worker;
                    }

                    return RedirectToPage("/Index");
                }

                ModelState.AddModelError("", "Invalid email or password.");
                return Page();
            }

            // Authenticate the user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, "User"),
                new Claim("ClubId", "0"),
                new Claim("Balance", user.Balance.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            var balanceClaim = User.Claims.FirstOrDefault(u => u.Type == "Balance");
            if (balanceClaim != null && int.TryParse(balanceClaim.Value, out int balance))
            {
                ViewData["Balance"] = balance;
            }
            var clubIdClaim = User.Claims.FirstOrDefault(u => u.Type == "ClubId");
            if (clubIdClaim != null && int.TryParse(clubIdClaim.Value, out int clubId))
            {
                ViewData["ClubId"] = clubId;
            }

            return RedirectToPage("/Index");
        }
    }
}