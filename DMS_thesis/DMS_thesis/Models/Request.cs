using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DMS_thesis.Models
{
    // Модель Заявка
    public class Request
    {
        // ID 
        public int Id { get; set; }
        // Наименование заявки
        
        [Display(Name = "Назва заяви")]
        [MaxLength(100, ErrorMessage = "Превышена максимальная длина записи")]
        public string Name { get; set; }
        // Описание
        [Required]
        [Display(Name = "Опис")]
        [MaxLength(200, ErrorMessage = "Превышена максимальная длина записи")]
        public string Description { get; set; }
        // Комментарий к заявке
        [Display(Name = "Коментар")]
        [MaxLength(200, ErrorMessage = "Превышена максимальная длина записи")]
        public string Comment { get; set; }
        // Статус заявки
        [Display(Name = "Статус")]
        public RequestStatus Status { get; set; }
        // Приоритет заявки
        [Display(Name = "Пріорітет")]
        public int Priority { get; set; }
        // Номер кабинета
        [Display(Name = "Кабінет")]
        
        public int? ActivId { get; set; }
        public Activ Activ { get; set; }
        // Файл ошибки
        [Display(Name = "Файл з помилкою")]
        public string File { get; set; }

        // Внешний ключ Категория
        [Display(Name = "Категорія")]
        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        // Внешний ключ
        // ID Пользователя - обычное свойство
        public int? UserId { get; set; }
        // Отдел пользователя - Навигационное свойство
        public User User { get; set; }

        // Внешний ключ
        // ID Пользователя - обычное свойство
        //[ForeignKey("Executor")]
        public int? ExecutorId { get; set; }
        // Отдел пользователя - Навигационное свойство
        public User Executor { get; set; }

        // Внешний ключ
        // ID жизненного цикла заявки - обычное свойство
        public int LifecycleId { get; set; }
        // Ссылка на жизненный цикл заявки - Навигационное свойство
        public Lifecycle Lifecycle { get; set; }

        public virtual ICollection<Files> Files { get; set; }
        // Перечисление для статуса заявки
        public enum RequestStatus
        {
            Open = 1,
            Distributed = 2,
            Proccesing = 3,
            Checking = 4,
            Closed = 5
        }
        // Перечисление для приоритета заявки
        public enum RequestPriority
        {
            Low = 1,
            Medium = 2,
            High = 3
        }
    }
}