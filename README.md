## Project Overview
Kanban Board-Based Task/Project Management Tool is a web-based application designed for efficient project management in large organizations where teams are working on multiple projects simultaneously, so managing each one manually becomes tough task sometimes. It implements the Kanban methodology to streamline project and workflow management, providing a visual interface for creating, organizing, and tracking tasks across different stages of completion.
Kanban is a lean method used to manage and improve work across human systems. This approach aims to balance demands with available capacity and enhance the handling of system-level bottlenecks.

Key principles of Kanban include:
Manage flow
Make process policies explicit
Implement feedback loops
Improve collaboratively, evolve experimentally

## Features
Create and manage multiple Kanban boards
Add, edit, and delete tasks (cards) within each board
Drag-and-drop cards between columns to update task status
Add detailed information to cards including descriptions, due dates, and assignees
Real-time updates across users

Technology Stack

Frontend: React.js
Backend: ASP.NET Core
Database: PostgreSQL
Additional libraries: react-beautiful-dnd for drag-and-drop functionality

## Steps to Run the Project
1. Clone the repository,
2. Set up the backend:
3. cd backend
4. dotnet restore
5. dotnet run. -> The API should now be running on http://localhost:5126
6. Set up the frontend:
7. cd ../frontend
8. npm install
9. npm start -> The React app should now be running on http://localhost:3000

Configuration
Database connection: Update the connection string in appsettings.json in the backend project.
API URL: If you change the backend port, update the API base URL in the frontend's src/api/config.js file.

## Project snapshots:
1. Kanban Board:
![image](https://github.com/user-attachments/assets/96de3401-a092-4042-93bd-88e891ad7907)
2. Drag and drop feature for cards:
![image](https://github.com/user-attachments/assets/36cfd59d-1c3d-47a6-a80c-867ff4d016fa)
![image](https://github.com/user-attachments/assets/9aef3b9b-38e7-4a33-bb3d-f405638a2858)
3. Edit Cards:
![image](https://github.com/user-attachments/assets/97821cdb-a35b-4d94-9378-f42fc5b3fa3f)
![image](https://github.com/user-attachments/assets/28684eb8-3440-4b50-a583-a950daf1ade5)
4. Deleting of lane:
![image](https://github.com/user-attachments/assets/4a6d946f-aa02-4dc0-a639-65d2b2788062)
5. Adding Lane:
![image](https://github.com/user-attachments/assets/cd3b25c0-274c-4f8d-98eb-005bdac723d4)
6. Landing page where user can enter user id and all the projects related to the userid will show up:
![image](https://github.com/user-attachments/assets/f2cbea5d-29cd-4cb3-a8a5-2d4ea4b8f649)
![image](https://github.com/user-attachments/assets/12fef80f-a5e5-4324-9010-fef059b5084c)

Thankyou for reading. The project is open to any suggestions or improvements.





