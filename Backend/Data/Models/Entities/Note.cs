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
    public DateTime TimeCreated { get; set; }
    public DateTime TimeModified { get; set; }
    public List<NoteCategories> NoteCategories { get; set; }
}
}

