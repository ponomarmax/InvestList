import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { 
  Container, 
  Typography, 
  Box, 
  TextField, 
  Button, 
  Grid,
  MenuItem,
  Alert,
  AlertProps
} from '@mui/material';
import { investmentService } from '../services/investmentService';
import { InvestmentPost } from '../services/investmentService';

const CreateInvestment: React.FC = () => {
  const navigate = useNavigate();
  const [error, setError] = useState<string | null>(null);
  const [formData, setFormData] = useState<Partial<InvestmentPost>>({
    title: '',
    description: '',
    content: '',
    language: 'uk',
    tags: []
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await investmentService.createInvestment(formData);
      navigate('/investments');
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to create investment');
    }
  };

  return (
    <Container maxWidth="md">
      <Box sx={{ mt: 4, mb: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Create New Investment
        </Typography>
        
        {error && (
          <Alert severity="error" sx={{ mb: 2 }} component="div">
            {error}
          </Alert>
        )}

        <form onSubmit={handleSubmit}>
          <Grid container spacing={3} component="div">
            <Grid item xs={12} component="div">
              <TextField
                required
                fullWidth
                label="Title"
                name="title"
                value={formData.title}
                onChange={handleChange}
              />
            </Grid>
            
            <Grid item xs={12} component="div">
              <TextField
                fullWidth
                label="Description"
                name="description"
                multiline
                rows={4}
                value={formData.description}
                onChange={handleChange}
              />
            </Grid>

            <Grid item xs={12} component="div">
              <TextField
                fullWidth
                label="Content"
                name="content"
                multiline
                rows={6}
                value={formData.content}
                onChange={handleChange}
              />
            </Grid>

            <Grid item xs={12} component="div">
              <TextField
                required
                fullWidth
                select
                label="Language"
                name="language"
                value={formData.language}
                onChange={handleChange}
              >
                <MenuItem value="uk">Ukrainian</MenuItem>
                <MenuItem value="en">English</MenuItem>
              </TextField>
            </Grid>

            <Grid item xs={12} component="div">
              <Box sx={{ display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
                <Button
                  variant="outlined"
                  onClick={() => navigate('/investments')}
                >
                  Cancel
                </Button>
                <Button
                  type="submit"
                  variant="contained"
                  color="primary"
                >
                  Create Investment
                </Button>
              </Box>
            </Grid>
          </Grid>
        </form>
      </Box>
    </Container>
  );
};

export default CreateInvestment; 