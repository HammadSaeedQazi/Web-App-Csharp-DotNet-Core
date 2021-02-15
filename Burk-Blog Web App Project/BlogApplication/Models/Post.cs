using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Models
{
    public class Post       //post class attributes with some pre-defined rules.
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter title")]
        [StringLength(50)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter content")]
        public string Content { get; set; }
        public string Date { get; set; }
        public string Usr { get; set; }
        public string usrPP { get; set; }
    }
}
