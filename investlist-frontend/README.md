# InvestList Frontend

This is the React frontend for the InvestList application, built with Vite, TypeScript, and Material-UI.

## Project Structure

```
src/
├── components/     # Reusable UI components
├── pages/         # Page components
├── services/      # API service layer
├── hooks/         # Custom React hooks
├── types/         # TypeScript types/interfaces
├── utils/         # Utility functions
├── context/       # React context providers
├── layouts/       # Layout components
└── assets/        # Static assets
```

## Dependencies

- React 18
- TypeScript
- Vite
- Material-UI
- React Router
- React Query
- Axios
- Zustand

## Setup

1. Install dependencies:
   ```bash
   npm install
   ```

2. Create a `.env` file with the following variables:
   ```
   VITE_API_URL=https://localhost:5001
   ```

3. Start the development server:
   ```bash
   npm run dev
   ```

## Development

The development server will start at `http://localhost:5173`.

## Building for Production

To build the application for production:

```bash
npm run build
```

The built files will be in the `dist` directory.

## Features

- Modern React with TypeScript
- Material-UI components
- React Router for navigation
- React Query for data fetching
- Axios for API calls
- Zustand for state management

## Notes

- The frontend is designed to work with the InvestList .NET Web API
- Uses environment variables for configuration
- Follows modern React best practices
- Implements responsive design
