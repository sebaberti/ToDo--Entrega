import axios from "axios";

export class BackendService {
    urlBase = 'https://localhost:7180/api';

    async getTasks() {
        let response = undefined;
        try {
            response = await axios.get(this.urlBase + '/Tasks');
        } catch (error) {
            alert(`Error al obtener tareas`);
        }
        return response;
    }

    async postTask(taskData) {
        try {
            const response = await axios.post(this.urlBase + '/Tasks', taskData);
            return response;
        } catch (error) {
            console.error('Error al agregar tarea:', error);
            throw error; // Propagar el error para manejarlo en el componente
        }
    }

    
    async deleteTask(taskId) {
        try {
            const response = await axios.delete(`${this.urlBase}/Tasks/${taskId}`);
            return response.data; // Devuelve los datos de la respuesta en caso de éxito
        } catch (error) {
            console.error('Error al eliminar tarea:', error);
            throw error; // Propaga el error para manejarlo en el componente
        }
    }

    async putTask(taskId, taskData) {
        try {
            const response = await axios.put(`${this.urlBase}/Tasks/${taskId}`, taskData);
            return response.data; // Devuelve los datos de la respuesta en caso de éxito
        } catch (error) {
            console.error('Error al actualizar tarea:', error);
            throw error; // Propaga el error para manejarlo en el componente
        }
    }

}