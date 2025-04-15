import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { ThemeProvider } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import { theme } from './theme';
import Layout from './components/Layout';
import Home from './pages/Home';
import Projects from './pages/Projects';
import ProjectDetails from './pages/ProjectDetails';
import Investments from './pages/Investments';
import InvestmentDetails from './pages/InvestmentDetails';
import CreateInvestment from './pages/CreateInvestment';
import EditInvestment from './pages/EditInvestment';
import { NotFound } from './pages/NotFound';

const App: React.FC = () => {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <BrowserRouter>
        <Layout>
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/blacklist" element={<Projects />} />
            <Route path="/news" element={<Projects />} />
            <Route path="/invest" element={<Projects />} />
            <Route path="/invest/create" element={<Projects />} />
            <Route path="/projects" element={<Projects />} />
            <Route path="/projects/:id" element={<ProjectDetails />} />
            <Route path="/investments" element={<Investments />} />
            <Route path="/investments/:id" element={<InvestmentDetails />} />
            <Route path="/investments/create" element={<CreateInvestment />} />
            <Route path="/investments/:id/edit" element={<EditInvestment />} />
            <Route path="/privacy-policy" element={<Projects />} />
            <Route path="*" element={<NotFound />} />
          </Routes>
        </Layout>
      </BrowserRouter>
    </ThemeProvider>
  );
};

export default App;
