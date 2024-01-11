import React, { useState, useEffect } from "react";
import apiService from "./api/apiService";
import "./App.css";

type Note = {
  id: number;
  title: string;
  content: string;
  categories: Array<{ id: number; name: string }>;
  timeCreated: string;
  timeModified: string;
  archived?: boolean;
};

const App = () => {
  const [isArchived, setIsArchived] = useState(false);
  const [notes, setNotes] = useState<Note[]>([]);
  const [selectedNote, setSelectedNote] = useState<Note | null>(null);
  const [title, setTitle] = useState("");
  const [content, setContent] = useState("");
  const [selectedCategories, setSelectedCategories] = useState<string[]>([]);
  const [filterCategory, setFilterCategory] = useState<string>("");
  const [showArchived, setShowArchived] = useState(false);
  

  useEffect(() => {
    fetchNotes();
  }, [showArchived]); 

  
  useEffect(() => {
    if (selectedNote) {
      setTitle(selectedNote.title || "");
      setContent(selectedNote.content || "");
      setSelectedCategories(selectedNote.categories.map(category => (typeof category === 'string' ? category : category.name)) || []);
      setIsArchived(selectedNote.archived || false);
    } else {
      // Reset input fields when no note is selected
      setTitle("");
      setContent("");
      setSelectedCategories([]);
      setIsArchived(false);
    }
  }, [selectedNote]);

  const parseCategories = (categoriesString: string): Array<{ id: number; name: string }> => {
    if (!categoriesString.includes(',')) {
      // If there are no commas, treat the entire string as one category
      return [{ id: 0, name: categoriesString.trim() }];
    }
  
    return categoriesString
      .split(",")
      .map((category) => ({ id: 0, name: category.trim() }));
  };

  const fetchNotes = async () => {
    try {
      const fetchedNotes = showArchived
        ? await apiService.getAllArchivedNotes()
        : await apiService.getAllActiveNotes();
      setNotes(fetchedNotes);
    } catch (error) {
      console.error("Error fetching notes:", error);
    }
  };


  const handleUpdateNote = async (event: React.FormEvent) => {

    if (!selectedNote) {
      return;
    }

    const updatedNote: Note = {
      ...selectedNote,
      title: title || selectedNote.title,
      content: content || selectedNote.content,
      categories: parseCategories(selectedCategories.join(",")),
      archived: isArchived,
    };

    await handleSaveNote(updatedNote, true);
  };

  const handleDeleteNote = async (noteId: number) => {
    try {
      await apiService.deleteNote(noteId);
      fetchNotes(); // Refresh the notes after deleting
    } catch (error) {
      console.error("Error deleting note:", error);
    }
  };

  const handleAddNote = async (event: React.FormEvent) => {
    const newNote: Note = {
      id: notes.length + 1,
      title: title,
      content: content,
      categories: parseCategories(selectedCategories.join(",")),
      archived: isArchived,
      timeCreated: new Date().toISOString(),
      timeModified: new Date().toISOString(),
    };
    await handleSaveNote(newNote, false);
  };


 

  const handleSaveNote = async (note: Note, isUpdate: boolean) => {
    try {
      

      if (isUpdate) {
        const serverData = {
          Title: note.title,
          Content: note.content,
          Archived:  isArchived,
          CategoryUpdates: note.categories.map(category => ({ CategoryId: category.id, CategoryName: category.name })),
        };
        
        await apiService.updateNoteWithCategories(note.id, serverData);
      } else {
        
        await apiService.createNoteWithCategories(note);
      }

      fetchNotes();
      setTitle("");
      setContent("");
      setSelectedCategories([]);
      setSelectedNote(null);
    } catch (error) {
      console.error(`Error ${isUpdate ? "updating" : "adding"} note:`, error);
    }
  };


  const handleFilterByCategory = async (event: React.FormEvent) => {

    event.preventDefault();

    try {
      if (filterCategory === "") {
        await handleClearFilter(); // Call the function to clear the filter
      } else {
        // Filter by category
        const filteredNotes = await apiService.getNotesByCategories(filterCategory, showArchived);
        setNotes(filteredNotes);
      }
    } catch (error) {
      console.error("Error filtering notes:", error);
    }
  };

  const handleClearFilter = async () => {
    try {
      // Clear filter, fetch all notes based on archive status
      const filteredNotes = showArchived
        ? await apiService.getAllArchivedNotes()
        : await apiService.getAllActiveNotes();

      setNotes(filteredNotes);
      setFilterCategory(""); // Clear the filter category
    } catch (error) {
      console.error("Error clearing filter:", error);
    }
   
  };
  const handleAddOrUpdateNote = async () => {
    
    if (selectedNote) {
      await handleUpdateNote({} as React.FormEvent); // Update the selected note
    } else {
      await handleAddNote({} as React.FormEvent); // Add a new note
    }
  };

  const handleArchiveNote = async (selectedNote: Note, archive: boolean) => {
    try {
      const updatedNote = {
        title: selectedNote.title,
        content: selectedNote.content,
        archived: archive,
        categoryUpdates: selectedNote.categories.map((category) => ({
          categoryId: 0, // You need to determine the actual categoryId or handle it accordingly
          categoryName: typeof category === 'string' ? category : category.name,
        })),
      };
  
      // Send the updated note to the API
      await apiService.updateNoteWithCategories(selectedNote.id, updatedNote);
  
    // Fetch all notes again after archiving/unarchiving
    fetchNotes();
    } catch (error) {
      console.error("Error archiving/unarchiving note:", error);
    }
  };
  

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleString();
  };

  return (
    <div className="app-container">
      <header className="header">
        <h1>Note App</h1>
      </header>
      <div className="main-content">
        <section className="note-form">
          <form onSubmit={handleAddOrUpdateNote}>
            <div className="form-row">
              <label htmlFor="title">Title</label>
              <input
                id="title"
                type="text"
                value={title}
                onChange={(event) => setTitle(event.target.value)}
                placeholder="Enter title"
                required
              />
            </div>
            <div className="form-row">
              <label htmlFor="content">Content</label>
              <textarea
                id="content"
                value={content}
                onChange={(event) => setContent(event.target.value)}
                placeholder="Enter content"
                rows={5}
                required
              />
            </div>
            <label htmlFor="archiveCheckbox">Archive Note</label>
            <div className="form-row">
            <input
  type="checkbox"
  id="archiveCheckbox"
  checked={isArchived}
  onChange={() => setIsArchived((prevIsArchived) => !prevIsArchived)}
/>

              
            </div>
            <div className="form-row">
              <label htmlFor="categories">Categories</label>
              <input
                id="categories"
                type="text"
                value={selectedCategories.join(",")}
                onChange={(event) => setSelectedCategories(event.target.value.split(",").map((cat) => cat.trim()))}
                placeholder="Categories (comma-separated)"
              />
               <button type="submit">{selectedNote ? "Update Note" : "Create Note"}</button>
            </div>
           
          </form>
        </section>
        <section className="filter-form">
          <form onSubmit={handleFilterByCategory}>
            <div className="form-row">
              <label htmlFor="filterCategory">Filter by category</label>
              <input
                id="filterCategory"
                type="text"
                value={filterCategory}
                onChange={(event) => setFilterCategory(event.target.value)}
                placeholder="Filter by category"
                required
              />
            </div>
            <div className="form-row">
  <button type="button" onClick={handleClearFilter}>
    Clear Filter
  </button>
  <button type="submit">Filter</button>
</div>
<div className="form-row">
  <button
    type="button"
    onClick={() => {
      setShowArchived(!showArchived);
      setFilterCategory(""); // Clear the filter when toggling between archived/unarchived
    }}
  >
    {showArchived ? "Show Unarchived" : "Show Archived"}
  </button>
</div>
          </form>
        </section>
      </div>
      <section className="notes-container">
        {notes.map((note) => (
          <div key={note.id} className={`note-card ${note.archived ? "archived" : ""}`} onClick={() => setSelectedNote(note)}>

            <h2>{note.title}</h2>
            <p>{note.content}</p>
            <p>Categories: {note.categories.map((category) => (typeof category === 'string' ? category : category.name)).join(", ")}</p>
            <p>Created: {formatDate(note.timeCreated)}</p>
            <p>Modified: {formatDate(note.timeModified)}</p>
            <div className="note-actions">
  <button onClick={() => handleDeleteNote(note.id)}>Delete</button>
  <button onClick={() => handleArchiveNote(note, !note.archived)}>
    {note.archived ? "Unarchive" : "Archive"}
  </button>
            </div>
          </div>
        ))}
      </section>
    </div>
  );
};

export default App;
