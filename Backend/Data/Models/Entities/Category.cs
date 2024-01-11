using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models.Entities
{
using System;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public List<NoteCategories> NoteCategories { get; set; }
}
}

