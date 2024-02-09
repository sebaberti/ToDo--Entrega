import React, { useState, useEffect } from 'react';
import axios from 'axios';


function TasksComponent() {
  const [tasks, setTasks] = useState([]);
  const [showAddForm, setShowAddForm] = useState(false);
  const [newTaskName, setNewTaskName] = useState('');
  const [newTaskDeadline, setNewTaskDeadline] = useState('');
  const [editingTask, setEditingTask] = useState(null);
  const [editedTaskName, setEditedTaskName] = useState('');
  const [editedTaskCompleted, setEditedTaskCompleted] = useState(false);
  const [editedTaskDeadline, setEditedTaskDeadline] = useState('');

  const fetchTasks = async () => {
    try {
      const response = await axios.get('https://localhost:7180/api/Tasks');
      // Ordenar las tareas por fecha límite
      const sortedTasks = response.data.result.sort((a, b) => {
        const dateA = new Date(a.deadLine);
        const dateB = new Date(b.deadLine);
        return dateA - dateB;
      });
      setTasks(sortedTasks);
      
    } catch (error) {
      console.error('Error al obtener las tareas:', error);
    }
  };
  useEffect(() => {
    fetchTasks();
  }, []);

  const handleNewTaskNameChange = (event) => {
    setNewTaskName(event.target.value);
  };

  const handleNewTaskDeadlineChange = (event) => {
    setNewTaskDeadline(event.target.value);
  };

  const handleAddTask = async () => {
    setShowAddForm(true);
  };

  const handleSubmit = async () => {
    try {
      const response = await axios.post('https://localhost:7180/api/Tasks', {
        name: newTaskName,
        completed: false,
        deadLine: newTaskDeadline,
      });
      setTasks([...tasks, response.data.result]);
      setNewTaskName('');
      setNewTaskDeadline('');
      setShowAddForm(false);
    } catch (error) {
      console.error('Error al agregar la tarea:', error);
    }
  };

  const handleDeleteTask = async (taskId) => {
    try {
      await axios.delete(`https://localhost:7180/api/Tasks/${taskId}`);
      fetchTasks(); // Actualizar la lista de tareas después de la eliminación
    } catch (error) {
      console.error('Error al eliminar la tarea:', error);
    }
  };

 
  const handleEditTask = (task) => {
    setEditingTask(task);
    setEditedTaskName(task.name || '');
    setEditedTaskCompleted(task.completed || false);
    setEditedTaskDeadline(task.deadLine || '');
  };


  const handleSaveEdit = async () => {
    try {
      await axios.put(`https://localhost:7180/api/Tasks/${editingTask.id}`, {
        name: editedTaskName,
        completed: editedTaskCompleted,
        deadLine: editedTaskDeadline,
      });
      fetchTasks();
      setEditingTask(null);
    } catch (error) {
      console.error('Error al editar la tarea:', error);
    }
  };

  return (
    <div className="container">
      <h1 className="text-center">Lista de Tareas</h1>
      {showAddForm ? (
        <div>
          <div className="mb-3">
            <label htmlFor="newTaskInput" className="form-label">Nombre de la Tarea</label>
            <input 
              type="text" 
              className="form-control" 
              id="newTaskInput" 
              value={newTaskName} 
              onChange={handleNewTaskNameChange} 
              placeholder="Nombre de la tarea" 
            />
          </div>
          <div className="mb-3">
            <label htmlFor="newTaskDeadlineInput" className="form-label">Fecha límite</label>
            <input 
              type="text" 
              className="form-control" 
              id="newTaskDeadlineInput" 
              value={newTaskDeadline} 
              onChange={handleNewTaskDeadlineChange} 
              placeholder="Fecha límite (opcional)" 
            />
          </div>
          <button 
            type="button" 
            className="btn btn-primary" 
            onClick={handleSubmit}
          >
            Agregar Tarea
          </button>
        </div>
      ) : (
        <button 
          type="button" 
          className="btn btn-primary" 
          onClick={handleAddTask}
        >
          Agregar Tarea
        </button>
      )}
      {tasks.length > 0 ? (
        <table className="table table-hover">
          <thead>
            <tr>
              <th scope="col">Nombre</th>
              <th scope="col">Completada</th>
              <th scope="col">Fecha límite</th>
              <th scope="col">Creada en</th>
              <th scope="col">Actualizada en</th>
              
              <th scope="col">Acciones</th>
            </tr>
          </thead>
          <tbody>
            {tasks.map(task => (
              <tr key={task.id}>
                <td>
                  {editingTask === task ? (
                    <input 
                      type="text" 
                      className="form-control" 
                      value={editedTaskName} 
                      onChange={(e) => setEditedTaskName(e.target.value)} 
                    />
                  ) : (
                    task.name
                  )}
                </td>
                <td>
                  {editingTask === task ? (
                    <input 
                      type="checkbox" 
                      checked={editedTaskCompleted} 
                      onChange={(e) => setEditedTaskCompleted(e.target.checked)} 
                    />
                  ) : (
                    task.completed ? 'Sí' : 'No'
                  )}
                </td>
                <td>
                  {editingTask === task ? (
                    <input 
                      type="text" 
                      className="form-control" 
                      value={editedTaskDeadline} 
                      onChange={(e) => setEditedTaskDeadline(e.target.value)} 
                    />
                  ) : (
                        task.deadLine 
                        


                  )}
                </td>
                <td>{new Date(task.createdAt).toLocaleDateString('es-ES')}</td>
                <td>{new Date(task.updatedAt).toLocaleDateString('es-ES')}</td>
             
                   <td>
                  {editingTask === task ? (
                    <>
                      <button className="btn btn-success" onClick={handleSaveEdit}>Guardar</button>
                      <button className="btn btn-secondary" onClick={() => setEditingTask(null)}>Cancelar</button>
                    </>
                  ) : (
                    <button className="btn btn-primary" onClick={() => handleEditTask(task)}>Editar</button>
                  )}
                  <button className="btn btn-danger" onClick={() => handleDeleteTask(task.id)}>Eliminar</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      ) : (
        <p>No hay tareas disponibles</p>
      )}
    </div>
  );
}

export default TasksComponent;
