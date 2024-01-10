using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace Backend.Data.DTO
{
    public class NoteDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Archived { get; set; }
        public List<CategoryDTO> Categories { get; set; }
    }
}
