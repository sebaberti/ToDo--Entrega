import React, { useState, useEffect } from 'react';
import { BackendService } from "../services/BackendService";
import FormTask from "./FormTask";


function TasksComponent() {
    const [tasks, setTasks] = useState([]);
    const [showAddForm, setShowAddForm] = useState(false);
    const [editingTask, setEditingTask] = useState(null);
    const [editedTaskName, setEditedTaskName] = useState('');
    const [editedTaskCompleted, setEditedTaskCompleted] = useState(false);
    const [editedTaskDeadline, setEditedTaskDeadline] = useState('');
   
    const [backendService] = useState(new BackendService());


    const fetchTasks = async () => {
      try {
          const response = await backendService.getTasks();
          if (response !== undefined) {
              const sortedTasks = response.data.result.sort((taskA, taskB) => {
                  const dateA = new Date(taskA.deadline).toLocaleString();
                  const dateB = new Date(taskB.deadline).toLocaleString();
                  return dateA.localeCompare(dateB);
              });
              setTasks(sortedTasks);
          }
      } catch (error) {
          console.error('Error al obtener las tareas:', error);
      }
  };
  

    useEffect(() => {
        fetchTasks();
    }, []);

    const handleAddTask = async (newTaskData) => {
        try {
            const response = await backendService.postTask(newTaskData);
            setTasks([...tasks, response.data.result]);
            setShowAddForm(false);
        } catch (error) {
            console.error('Error al agregar la tarea:', error);
        }
    };

    const handleDeleteTask = async (taskId) => {
        try {
            await backendService.deleteTask(taskId);
            fetchTasks(); // Espera a que se complete la eliminación antes de actualizar la lista de tareas
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
          await backendService.putTask(editingTask.id, {
              id: editingTask.id,
              name: editedTaskName,
              completed: editedTaskCompleted,
              deadline: editedTaskDeadline,
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
            <FormTask onAddTask={handleAddTask} />
        ) : (
            <button
                type="button"
                className="btn btn-primary"
                onClick={() => setShowAddForm(true)}
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
                                {editingTask && editingTask.id === task.id ? (
                                    <input
                                        type="text"
                                        value={editedTaskName}
                                        onChange={(e) => setEditedTaskName(e.target.value)}
                                    />
                                ) : (
                                    task.name
                                )}
                            </td>
                            <td>
                                {editingTask && editingTask.id === task.id ? (
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
                                {editingTask && editingTask.id === task.id ? (
                                    <input
                                        type="text"
                                        value={editedTaskDeadline}
                                        onChange={(e) => setEditedTaskDeadline(e.target.value)}
                                    />
                                ) : (
                                    task.deadLine
                                )}
                            </td>
                            <td>
                              {task.createdAt}
                            
                            </td>
                            <td>
                             {task.updatedAt}
                            </td>


                            <td>
                                {editingTask && editingTask.id === task.id ? (
                                    <>
                                        <button className="btn btn-success" onClick={handleSaveEdit}>Guardar</button>
                                        <button className="btn btn-secondary" onClick={() => setEditingTask(null)}>Cancelar</button>
                                    </>
                                ) : (
                                    <>
                                        <button className="btn btn-primary" onClick={() => handleEditTask(task)}>Editar</button>
                                        <button className="btn btn-danger" onClick={() => handleDeleteTask(task.id)}>Eliminar</button>
                                    </>
                                )}
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
