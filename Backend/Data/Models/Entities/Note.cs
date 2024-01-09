using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Backend.Data.Models.Entities
{
       public class Note
    {
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Archived { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
