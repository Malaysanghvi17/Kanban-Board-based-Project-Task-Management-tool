import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import LandingPage from "./components/LandingPage";  // Adjust the path if necessary
import KanbanBoard from "./components/KanbanBoard";  // Adjust the path if necessary

const App = () => {
  return (
    <Router>
      <Routes>
        {/* Root route rendering the LandingPage */}
        <Route path="/" element={<LandingPage />} />

        {/* Kanban board route rendering the KanbanBoard component based on project ID (bid) */}
        <Route path="/project/:bid" element={<KanbanBoard />} />
      </Routes>
    </Router>
  );
};

export default App;
