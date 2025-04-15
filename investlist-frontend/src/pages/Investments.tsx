import React, { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { Typography, CircularProgress, Box, Alert, Grid, Card, CardContent, CardActions, Button, IconButton } from '@mui/material';
import RefreshIcon from '@mui/icons-material/Refresh';
import { investmentService } from '../services/investmentService';
import { Link } from 'react-router-dom';

const Investments: React.FC = () => {
  const [page, setPage] = useState(1);
  const pageSize = 10;

  const { data: investments, isLoading, error, refetch } = useQuery({
    queryKey: ['investments', page, pageSize],
    queryFn: () => investmentService.getInvestments(page, pageSize)
  });

  if (isLoading && !investments) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="200px">
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return (
      <Box sx={{ mt: 2 }}>
        <Alert severity="error">
          {error instanceof Error ? error.message : 'An error occurred while fetching investments'}
        </Alert>
      </Box>
    );
  }

  return (
    <div className="container">
      <Box display="flex" justifyContent="space-between" alignItems="center" sx={{ mb: 3 }}>
        <Typography variant="h4">
          Investment Projects
        </Typography>
        <IconButton onClick={() => refetch()} color="primary" disabled={isLoading}>
          <RefreshIcon />
        </IconButton>
      </Box>

      {isLoading && (
        <Box display="flex" justifyContent="center" sx={{ my: 2 }}>
          <CircularProgress size={24} />
        </Box>
      )}

      {investments?.items.length === 0 ? (
        <Box sx={{ mt: 2 }}>
          <Alert severity="info">
            No investment projects found. Please check back later.
          </Alert>
        </Box>
      ) : (
        <Grid container spacing={3}>
          {investments?.items.map((investment) => (
            <Grid item key={investment.id} xs={12} md={6} lg={4}>
              <Card sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
                <CardContent sx={{ flexGrow: 1 }}>
                  <Typography variant="h6" gutterBottom>
                    {investment.title}
                  </Typography>
                  <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                    {investment.description}
                  </Typography>
                  <Typography variant="body2">
                    Created: {new Date(investment.createdAt).toLocaleDateString()}
                  </Typography>
                </CardContent>
                <CardActions>
                  <Button
                    component={Link}
                    to={`/investments/${investment.slug}`}
                    size="small"
                    color="primary"
                  >
                    View Details
                  </Button>
                </CardActions>
              </Card>
            </Grid>
          ))}
        </Grid>
      )}

      {investments && investments.totalCount > pageSize && (
        <Box sx={{ mt: 3, display: 'flex', justifyContent: 'center' }}>
          <Button
            onClick={() => setPage(p => p - 1)}
            disabled={page === 1 || isLoading}
            sx={{ mr: 2 }}
          >
            Previous
          </Button>
          <Button
            onClick={() => setPage(p => p + 1)}
            disabled={page * pageSize >= investments.totalCount || isLoading}
          >
            Next
          </Button>
        </Box>
      )}
    </div>
  );
};

export default Investments; 