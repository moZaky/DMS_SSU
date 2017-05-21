using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
 
namespace DMS_thesis.Models
{
    public class User
    {
        // ID 
        public int Id { get; set; }
        // Имя
        [Required]
        [Display(Name = "Ім'я")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string First_name { get; set; }
        // Отчество
        [Required]
        [Display(Name = "По-батькові")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Middle_name { get; set; }
        // Фамилия
        [Required]
        [Display(Name = "Прізвище")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Last_name { get; set; }
        // Логин
        [Required]
        [Display(Name = "Логін")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Login { get; set; }
        // Пароль
        [Required]
        [Display(Name = "Пароль")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Password { get; set; }
        // Должность
        [Display(Name = "Посада")]
        [MaxLength(50, ErrorMessage = "Превышена максимальная длина записи")]
        public string Position { get; set; }
        // Отдел
        [Display(Name = "Відділ")]
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
        // Статус
        [Required]
        [Display(Name = "Статус")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
        // Электронная почта (E-mail)
        [Required]
        [Display(Name = "E-mail")]
        [MaxLength(100, ErrorMessage = "Превышена максимальная длина записи")]
        public string Email { get; set; }
    }
}