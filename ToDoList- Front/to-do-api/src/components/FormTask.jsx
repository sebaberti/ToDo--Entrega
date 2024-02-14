import React, { useState } from 'react';

function TaskForm({ onAddTask }) {
  const [taskName, setTaskName] = useState('');
  const [deadline, setDeadline] = useState('');

  const handleSubmit = (event) => {
    event.preventDefault();
    if (!taskName.trim() || !deadline.trim()) {
      alert('Por favor, complete todos los campos.');
      return;
    }
    // Llama a la función de devolución de llamada proporcionada por el componente padre para agregar la tarea
    onAddTask({ name: taskName, deadline });
    // Limpiar los campos después de enviar el formulario
    setTaskName('');
    setDeadline('');
  };

  return (
    <form onSubmit={handleSubmit} className="p-3 border rounded">
      <div className="form-group">
        <label htmlFor="taskName">Nombre de la Tarea:</label>
        <input
          type="text"
          id="taskName"
          value={taskName}
          onChange={(e) => setTaskName(e.target.value)}
          className="form-control"
          placeholder="Ingrese el nombre de la tarea"
        />
      </div>
      <div className="form-group">
        <label htmlFor="deadline">Fecha Límite:</label>
        <input
          type="date"
          id="deadline"
          value={deadline}
          onChange={(e) => setDeadline(e.target.value)}
          className="form-control"
        />
      </div>
      <button type="submit" className="btn btn-primary">Agregar Tarea</button>
    </form>
  );
}

export default TaskForm;
