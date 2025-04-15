import React from 'react';
import { Typography, Box, Container } from '@mui/material';

const Projects: React.FC = () => {
  return (
    <Container maxWidth="lg">
      <Box sx={{ my: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Projects
        </Typography>
        <Typography variant="body1">
          Manage your investment projects here.
        </Typography>
      </Box>
    </Container>
  );
};

export default Projects; 