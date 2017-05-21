using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DMS_thesis.Models
{
    public class Files
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

     //   public int RequestId { get; set; }
       // public virtual Request Request { get; set; }
    }
}