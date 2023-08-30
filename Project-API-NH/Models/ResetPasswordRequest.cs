using System;
using System.ComponentModel.DataAnnotations;

namespace Project_API_NH.Models
{
	public class ResetPasswordRequest
	{
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required, MinLength(6, ErrorMessage = "Please enter at leat 6 characters, dude!")]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password")]
        public string ComfirmPassword { get; set; } = string.Empty;             
    }
}

