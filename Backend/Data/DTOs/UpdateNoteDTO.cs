using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.DTOs
{
public class UpdateNoteDTO
{
 public string Title { get; set; }
    public string Content { get; set; }
    public bool Archived { get; set; }
    public List<CategoryUpdateDTO> CategoryUpdates { get; set; }
}
}