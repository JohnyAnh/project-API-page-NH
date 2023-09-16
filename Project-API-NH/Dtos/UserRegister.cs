using System;
using System.ComponentModel.DataAnnotations;
namespace Project_API_NH.Dtos
{
	public class UserRegister
	{
        [Required]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Tel { get; set; }


    }
}

