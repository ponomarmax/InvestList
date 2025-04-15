import React from 'react';
import { Typography, Box, Container } from '@mui/material';

const Home: React.FC = () => {
  return (
    <Container maxWidth="lg">
      <Box sx={{ my: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Welcome to InvestList
        </Typography>
        <Typography variant="body1">
          Your platform for managing investments and projects.
        </Typography>
      </Box>
    </Container>
  );
};

export default Home; 