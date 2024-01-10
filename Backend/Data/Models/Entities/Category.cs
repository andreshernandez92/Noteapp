using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        // Navigation property for the many-to-many relationship with Note
        public List<Note> Notes { get; set; }
    }
}

