import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { 
  Container, 
  Typography, 
  Box, 
  TextField, 
  Button, 
  Grid,
  MenuItem,
  Alert
} from '@mui/material';
import { investmentService } from '../services/investmentService';
import { InvestmentPost } from '../services/investmentService';

const EditInvestment: React.FC = () => {
  const navigate = useNavigate();
  const { slug } = useParams<{ slug: string }>();
  const [error, setError] = useState<string | null>(null);
  const [formData, setFormData] = useState<Partial<InvestmentPost>>({
    title: '',
    description: '',
    content: '',
    language: 'uk',
    tags: []
  });

  useEffect(() => {
    const fetchInvestment = async () => {
      if (!slug) return;
      try {
        const investment = await investmentService.getInvestment(slug);
        if (investment) {
          setFormData(investment);
        }
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to fetch investment');
      }
    };

    fetchInvestment();
  }, [slug]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.id) return;
    try {
      await investmentService.updateInvestment(formData.id, formData);
      navigate('/investments');
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to update investment');
    }
  };

  return (
    <Container maxWidth="md">
      <Box sx={{ mt: 4, mb: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Edit Investment
        </Typography>
        
        {error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {error}
          </Alert>
        )}

        <form onSubmit={handleSubmit}>
          <Grid container spacing={3}>
            <Grid item xs={12}>
              <TextField
                required
                fullWidth
                label="Title"
                name="title"
                value={formData.title}
                onChange={handleChange}
              />
            </Grid>
            
            <Grid item xs={12}>
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

            <Grid item xs={12}>
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

            <Grid item xs={12}>
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

            <Grid item xs={12}>
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
                  Update Investment
                </Button>
              </Box>
            </Grid>
          </Grid>
        </form>
      </Box>
    </Container>
  );
};

export default EditInvestment; 