using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DMS_thesis.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Назва категорії")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Name { get; set; }
    
    }
}