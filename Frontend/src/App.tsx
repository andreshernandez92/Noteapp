import React, { useState, useEffect } from "react";
import apiService from "./api/apiService";
import "./App.css";

type Note = {
  id: number;
  title: string;
  content: string;
  categories: string[] | Array<{ name: string }>;
  timeCreated: string;
  timeModified: string;
  archived?: boolean;
};

const App = () => {
  const [notes, setNotes] = useState<Note[]>([]);
  const [selectedNote, setSelectedNote] = useState<Note | null>(null);
  const [title, setTitle] = useState("");
  const [content, setContent] = useState("");
  const [selectedCategories, setSelectedCategories] = useState<string[]>([]);
  const [filterCategory, setFilterCategory] = useState<string>("");

  useEffect(() => {
    fetchNotes();
  }, []);

  const fetchNotes = async () => {
    try {
      const fetchedNotes = await apiService.getAllNotes();
      setNotes(fetchedNotes);
    } catch (error) {
      console.error("Error fetching notes:", error);
    }
  };

  const handleUpdateNote = async (event: React.FormEvent) => {
    event.preventDefault();

    if (!selectedNote) {
      return;
    }

    const updatedNote: Note = {
      ...selectedNote,
      title: title || selectedNote.title,
      content: content || selectedNote.content,
      categories: selectedCategories.length > 0 ? selectedCategories : selectedNote.categories,
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
    event.preventDefault();

    const newNote: Note = {
      id: notes.length + 1,
      title: title,
      content: content,
      categories: selectedCategories,
      timeCreated: new Date().toISOString(),
      timeModified: new Date().toISOString(),
    };

    await handleSaveNote(newNote, false);
  };

  const handleSaveNote = async (note: Note, isUpdate: boolean) => {
    try {
      if (isUpdate) {
        await apiService.updateNoteWithCategories(note.id, note);
      } else {
        await apiService.createNoteWithCategories(note);
      }

      fetchNotes(); // Refresh the notes after saving
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
      const filteredNotes = await apiService.getNotesByCategories(filterCategory, false);
      setNotes(filteredNotes);
    } catch (error) {
      console.error("Error filtering notes by category:", error);
    }
  };

  const handleAddOrUpdateNote = async () => {
    if (selectedNote) {
      await handleUpdateNote({} as React.FormEvent); // Update the selected note
    } else {
      await handleAddNote({} as React.FormEvent); // Add a new note
    }
  };

  const handleArchiveNote = async (noteId: number, archive: boolean) => {
    try {
      await apiService.updateNoteArchiveStatus(noteId, archive);
      fetchNotes(); // Refresh the notes after archiving/unarchiving
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
        <h1>Futuristic Notes</h1>
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
                checked={selectedNote?.archived || false}
                onChange={() => setSelectedNote((prevNote) => prevNote && { ...prevNote, archived: !prevNote.archived })}
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
            </div>
            <button type="submit">{selectedNote ? "Update Note" : "Create Note"}</button>
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
              <button type="submit">Filter</button>
              <button type="button" onClick={() => setFilterCategory("")}>
                Clear Filter
              </button>
              <button type="button" onClick={() => setFilterCategory("Archived")}>
                Show Archived
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
              <button onClick={() => handleArchiveNote(note.id, !note.archived)}>
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
