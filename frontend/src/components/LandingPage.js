import React, { useState, useEffect } from "react";
import api from "../axiosConfig";  // Import the configured axios instance
import { useNavigate } from "react-router-dom";

const LandingPage = () => {
  const [userId, setUserId] = useState('');
  const [projects, setProjects] = useState([]);
  const [newProjectName, setNewProjectName] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    if (userId) {
      api.get(`/users/${userId}/boards`)   // Adjust API request
        .then(res => setProjects(res.data))
        .catch(err => console.error(err));
    }
  }, [userId]);

  const handleCreateProject = () => {
    if (newProjectName) {
      api.post(`/users/${userId}/boards`, { name: newProjectName })   // Adjust API request
        .then(res => setProjects([...projects, res.data]))
        .catch(err => console.error(err));
    }
  };

  const handleDeleteProject = (bid) => {
    api.delete(`/users/${userId}/boards/${bid}`)   // Adjust API request
      .then(() => setProjects(projects.filter(p => p.bid !== bid)))
      .catch(err => console.error(err));
  };

  const handleSelectProject = (bid) => {
    navigate(`/project/${bid}`);
  };

  return (
    <div>
      <h1>Welcome to Kanban Board</h1>
      <input 
        type="text" 
        placeholder="Enter User ID" 
        value={userId} 
        onChange={(e) => setUserId(e.target.value)} 
      />
      <div>
        <h2>Projects</h2>
        {projects.length ? (
          <ul>
            {projects.map((project) => (
              <li key={project.bid}>
                {project.name}
                <button onClick={() => handleSelectProject(project.bid)}>Open</button>
                <button onClick={() => handleDeleteProject(project.bid)}>Delete</button>
              </li>
            ))}
          </ul>
        ) : <p>No projects found</p>}
      </div>
      <div>
        <input 
          type="text" 
          placeholder="New Project Name" 
          value={newProjectName} 
          onChange={(e) => setNewProjectName(e.target.value)} 
        />
        <button onClick={handleCreateProject}>Create Project</button>
      </div>
    </div>
  );
};

export default LandingPage;
