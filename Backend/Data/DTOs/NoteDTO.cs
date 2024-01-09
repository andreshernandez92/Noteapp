using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.DTO
{
    public class NoteDTO
    {
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Archived { get; set; }
        public IEnumerable<string> Categories { get; set; }
    }
}