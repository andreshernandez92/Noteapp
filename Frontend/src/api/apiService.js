// apiService.js
import axios from 'axios';

axios.defaults.baseURL =  process.env.REACT_APP_API_URL

const apiService = {
  createNoteWithCategories: async (noteDTO) => {
    try {
   
      
      const response = await axios.post(`notes/create`, noteDTO);
      return response.data;
    } catch (error) {
      console.error('Error creating note with categories:', error);
      throw error;
    }
  },

  getNoteById: async (id) => {
    try {
      const response = await axios.get(`notes/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error getting note by ID:', error);
      throw error;
    }
  },

  getAllNotes: async () => {
    try {
      const response = await axios.get(`notes`);
      return response.data;
    } catch (error) {
      console.error('Error getting all notes:', error);
      throw error;
    }
  },

  getAllActiveNotes: async () => {
    try {
      const response = await axios.get(`notes/active`);
      return response.data;
    } catch (error) {
      console.error('Error getting all active notes:', error);
      throw error;
    }
  },

  getAllArchivedNotes: async () => {
    try {
      const response = await axios.get(`notes/archived`);
      return response.data;
    } catch (error) {
      console.error('Error getting all archived notes:', error);
      throw error;
    }
  },

  deleteNote: async (id) => {
    try {
      const response = await axios.delete(`notes/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error deleting note:', error);
      throw error;
    }
  },

  updateNoteWithCategories: async (id, updatedNote) => {
    try {
      const response = await axios.put(`notes/update-with-categories/${id}`, updatedNote);
      return response.data;
    } catch (error) {
      console.error('Error updating note with categories:', error);
      throw error;
    }
  },

  removeCategoryFromNote: async (noteId, categoryId) => {
    try {
      const response = await axios.delete(`notes/remove-category-from-note?noteId=${noteId}&categoryId=${categoryId}`);
      return response.data;
    } catch (error) {
      console.error('Error removing category from note:', error);
      throw error;
    }
  },

  

 

  getAllCategories: async () => {
    try {
      const response = await axios.get(`notes/get-all`);
      return response.data;
    } catch (error) {
      console.error('Error fetching categories:', error);
      throw error;
    }
  },
  async getNotesByCategories(categoryName, isArchived) {
    try {
      const response = await axios.get(`notes/by-categories`, {
        params: {
          categoryName,
          Archived: isArchived // Explicitly pass isArchived as a boolean
        },
      });
  
      return response.data;
    } catch (error) {
      console.error('Error getting notes by categories:', error);
      throw error;
    }
  },updateNoteArchiveStatus: async (noteId, archive) => {
    try {
      const response = await axios.patch(`notes/${noteId}`, { archived: archive });
      return response.data;
    } catch (error) {
      throw error;
    }
  },
  
};

export default apiService;
