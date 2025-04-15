import React from 'react';
import { useParams } from 'react-router-dom';
import { Typography, Paper } from '@mui/material';

const ProjectDetails: React.FC = () => {
  const { id } = useParams<{ id: string }>();

  return (
    <div className="container">
      <Paper elevation={3} sx={{ p: 3 }}>
        <Typography variant="h4" gutterBottom>
          Project Details
        </Typography>
        <Typography variant="body1">
          Project ID: {id}
        </Typography>
      </Paper>
    </div>
  );
};

export default ProjectDetails; 