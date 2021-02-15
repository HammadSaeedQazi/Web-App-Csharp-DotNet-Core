using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Models
{
    public class RegularUser    //User class, randomly named as regular user with attributes and some pre-defined rules.
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter username")]
        [StringLength(50)]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please enter password")]
        [StringLength(20)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please enter password")]
        [StringLength(20)]
        public string anotherPassword { get; set; }
        [Required(ErrorMessage = "Please enter email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public IFormFile profilePicture { get; set; }
        public string picAddress { get; set; }
    }
}
