using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.Models.Entities
{

public class NoteCategories
{
    public int NoteId { get; set; }
    public Note Note { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }
}
}