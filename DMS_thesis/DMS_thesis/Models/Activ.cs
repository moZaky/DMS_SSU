using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DMS_thesis.Models
{
    // Модель Активы
    public class Activ
    {
        public int Id { get; set; }
        // номер кабинета
        [Required]
        [Display(Name = "Номер кабінету")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string CabNumber { get; set; }

        // Внешний ключ
        // ID Отдела - обычное свойство
        [Required]
        [Display(Name = "Відділ")]
        public int? DepartmentId { get; set; }
        // Отдел - Навигационное свойство
        public Department Department { get; set; }
    }
}