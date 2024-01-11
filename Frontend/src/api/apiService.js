// apiService.js
import axios from 'axios';

const BASE_URL = 'http://localhost:5013/api/';

const apiService = {
  createNoteWithCategories: async (noteDto) => {
    try {
      const response = await axios.post(`${BASE_URL}notes/create`, noteDto);
      return response.data;
    } catch (error) {
      console.error('Error creating note with categories:', error);
      throw error;
    }
  },

  getNoteById: async (id) => {
    try {
      const response = await axios.get(`${BASE_URL}notes/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error getting note by ID:', error);
      throw error;
    }
  },

  getAllNotes: async () => {
    try {
      const response = await axios.get(`${BASE_URL}notes`);
      return response.data;
    } catch (error) {
      console.error('Error getting all notes:', error);
      throw error;
    }
  },

  deleteNote: async (id) => {
    try {
      const response = await axios.delete(`${BASE_URL}notes/${id}`);
      return response.data;
    } catch (error) {
      console.error('Error deleting note:', error);
      throw error;
    }
  },

  updateNoteWithCategories: async (id, updatedNote) => {
    try {
      const response = await axios.put(`${BASE_URL}notes/update-with-categories/${id}`, updatedNote);
      return response.data;
    } catch (error) {
      console.error('Error updating note with categories:', error);
      throw error;
    }
  },

  removeCategoryFromNote: async (noteId, categoryId) => {
    try {
      const response = await axios.delete(`${BASE_URL}notes/remove-category-from-note?noteId=${noteId}&categoryId=${categoryId}`);
      return response.data;
    } catch (error) {
      console.error('Error removing category from note:', error);
      throw error;
    }
  },

  getAllArchivedNotes: async () => {
    try {
      const response = await axios.get(`${BASE_URL}notes/archived`);
      return response.data;
    } catch (error) {
      console.error('Error getting all archived notes:', error);
      throw error;
    }
  },

  getAllActiveNotes: async () => {
    try {
      const response = await axios.get(`${BASE_URL}notes/active`);
      return response.data;
    } catch (error) {
      console.error('Error getting all active notes:', error);
      throw error;
    }
  },

  getAllCategories: async () => {
    try {
      const response = await axios.get(`${BASE_URL}notes/get-all`);
      return response.data;
    } catch (error) {
      console.error('Error fetching categories:', error);
      throw error;
    }
  },
  async getNotesByCategories(categoryName, isArchived) {
    try {
      const response = await axios.get(`${BASE_URL}notes/by-categories`, {
        params: {
          categoryName: categoryName,
          isArchived: isArchived,
        },
      });

      return response.data;
    } catch (error) {
      console.error('Error getting notes by categories:', error);
      throw error;
    }
  },updateNoteArchiveStatus: async (noteId, archive) => {
    try {
      const response = await axios.patch(`${BASE_URL}/notes/${noteId}`, { archived: archive });
      return response.data;
    } catch (error) {
      throw error;
    }
  },
  
};

export default apiService;
