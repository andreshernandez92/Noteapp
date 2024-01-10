using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.Collections.Generic;

namespace Backend.Data.Models.Entities
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Archived { get; set; }

        // Navigation property for the many-to-many relationship with Category
        public List<Category> Categories { get; set; }
    }
}

