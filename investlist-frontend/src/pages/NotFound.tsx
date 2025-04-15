import { Typography, Paper, Box } from '@mui/material';

export const NotFound = () => {
  return (
    <Box display="flex" justifyContent="center" alignItems="center" minHeight="60vh">
      <Paper sx={{ p: 4, textAlign: 'center' }}>
        <Typography variant="h3" gutterBottom>
          404
        </Typography>
        <Typography variant="h5" gutterBottom>
          Page Not Found
        </Typography>
        <Typography variant="body1" color="text.secondary">
          The page you are looking for does not exist.
        </Typography>
      </Paper>
    </Box>
  );
}; 