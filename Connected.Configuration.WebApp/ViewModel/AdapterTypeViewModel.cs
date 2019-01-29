using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Connected.DAL.Core.Configuration.Model;

namespace Connected.Configuration.WebApp.ViewModel
{
    public class AdapterTypeViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required, StringLength(60, MinimumLength = 3), Display(Name = "Adapter Type")]
        public string AdapterType { get; set; }
    }
}