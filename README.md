# HernandezLacayo-fbff18

This repository contains the source code for a full-stack application with a C#/.NET backend using Entity Framework and a TypeScript/React/Node.js frontend.

## Prerequisites

Make sure you have the following software installed on your machine:

- Node.js (version 18.18.0)
- .NET SDK (version 7.0.400)

## Folder Structure

Navigate to the `Backend` folder.

Update connection strings in `appsettings.Development.json` and `appsettings.json` if needed.

Run the following commands:

```bash
dotnet restore
dotnet ef database update
dotnet run
```

The backend server will be running at [https://localhost:5001](https://localhost:5001).

### Frontend Setup:

Navigate to the `Frontend` folder.

Run the following commands:

```bash
npm install
npm start
```

The frontend development server will be running at [http://localhost:3000](http://localhost:3000).

## Accessing the Application:

- Open your web browser and visit [http://localhost:3000](http://localhost:3000) to access the frontend.
- Use [https://localhost:5001](https://localhost:5001) for API requests from the frontend.

## Additional Notes:

- Ensure that the backend is running before accessing the frontend.
- Refer to individual folders for more specific setup and configuration details.