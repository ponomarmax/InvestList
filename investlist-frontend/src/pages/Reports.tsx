import React from 'react';
import { Typography, Box, Container } from '@mui/material';

const Reports: React.FC = () => {
  return (
    <Container maxWidth="lg">
      <Box sx={{ my: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Reports
        </Typography>
        <Typography variant="body1">
          View and analyze your investment reports here.
        </Typography>
      </Box>
    </Container>
  );
};

export default Reports; 