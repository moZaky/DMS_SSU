using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DMS_thesis.Models;

namespace DMS_thesis.Models
{
    public class ViewTemplates
    {
        public IEnumerable<DMS_thesis.Models.Template> Templates { get; set; }
        public DMS_thesis.Models.Template Template { get; set; }
    }
}