using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DMS_thesis.Models
{
    public class Template
    {
        // ID 
        public int Id { get; set; }
        // Наименование заявки
        [Required]
        [Display(Name = "Назва шаблону")]
        [MaxLength(100, ErrorMessage = "Перевищена максимальна довжина запису")]
        public string Name { get; set; }
   
        // Комментарий к заявке
        [Display(Name = "Шлях до файлу шаблону")]
        [MaxLength(1000, ErrorMessage = "Перевищена максимальна довжина запису")]
        public string Path { get; set; }

        // Внешний ключ Категория
        [Display(Name = "Категорія")]
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        //public int CategorytId { get; set; }
        //public virtual Category Category { get; set; }

        //public virtual ICollection<Category> Categories { get; set; }
    }
}