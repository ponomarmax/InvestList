import React from 'react';
import { useParams } from 'react-router-dom';
import { Typography, Paper } from '@mui/material';

const InvestmentDetails: React.FC = () => {
  const { id } = useParams<{ id: string }>();

  return (
    <div className="container">
      <Paper elevation={3} sx={{ p: 3 }}>
        <Typography variant="h4" gutterBottom>
          Investment Details
        </Typography>
        <Typography variant="body1">
          Investment ID: {id}
        </Typography>
      </Paper>
    </div>
  );
};

export default InvestmentDetails; 