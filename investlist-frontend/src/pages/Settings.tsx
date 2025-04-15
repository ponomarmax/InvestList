import React, { useState, useEffect } from 'react';
import {
  Typography,
  Box,
  Container,
  Paper,
  Tabs,
  Tab,
  FormControl,
  FormControlLabel,
  Switch,
  Select,
  MenuItem,
  Button,
  TextField,
  Alert,
  AlertProps,
  Divider,
  SelectChangeEvent,
  List,
  ListItem,
  ListItemText,
  ListItemSecondaryAction,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
} from '@mui/material';
import { useTranslation } from 'react-i18next';
import DeleteIcon from '@mui/icons-material/Delete';
import { useAuth } from '../contexts/AuthContext';
import { styled } from '@mui/material/styles';

interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

function TabPanel(props: TabPanelProps) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`settings-tabpanel-${index}`}
      aria-labelledby={`settings-tab-${index}`}
      {...other}
    >
      {value === index && (
        <Box sx={{ p: 3 }}>
          {children}
        </Box>
      )}
    </div>
  );
}

const StyledAlert = styled(Alert)<AlertProps>(({ theme }) => ({
  marginBottom: theme.spacing(2),
}));

const Settings: React.FC = () => {
  const [tabValue, setTabValue] = useState(0);
  const [language, setLanguage] = useState('uk');
  const [emailNotifications, setEmailNotifications] = useState(true);
  const [profileData, setProfileData] = useState({
    username: '',
    email: '',
    currentPassword: '',
    newPassword: '',
    confirmPassword: '',
  });
  const [externalLogins, setExternalLogins] = useState<string[]>([]);
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [statusMessage, setStatusMessage] = useState<{ type: 'success' | 'error', message: string } | null>(null);
  const { t, i18n } = useTranslation();
  const { user, updateProfile, changePassword, removeExternalLogin } = useAuth();

  useEffect(() => {
    if (user) {
      setProfileData(prev => ({
        ...prev,
        username: user.username || '',
        email: user.email || '',
      }));
      setExternalLogins(user.externalLogins || []);
    }
  }, [user]);

  const handleTabChange = (event: React.SyntheticEvent, newValue: number) => {
    setTabValue(newValue);
  };

  const handleLanguageChange = (event: SelectChangeEvent) => {
    const newLanguage = event.target.value;
    setLanguage(newLanguage);
    i18n.changeLanguage(newLanguage);
  };

  const handleEmailNotificationsChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setEmailNotifications(event.target.checked);
  };

  const handleProfileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    setProfileData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSaveProfile = async () => {
    try {
      await updateProfile({
        username: profileData.username,
        email: profileData.email,
      });
      setStatusMessage({ type: 'success', message: t('settings.profileUpdated') });
    } catch (error) {
      setStatusMessage({ type: 'error', message: t('settings.profileUpdateError') });
    }
  };

  const handleChangePassword = async () => {
    if (profileData.newPassword !== profileData.confirmPassword) {
      setStatusMessage({ type: 'error', message: t('settings.passwordsDontMatch') });
      return;
    }

    try {
      await changePassword(profileData.currentPassword, profileData.newPassword);
      setStatusMessage({ type: 'success', message: t('settings.passwordChanged') });
      setProfileData(prev => ({ ...prev, currentPassword: '', newPassword: '', confirmPassword: '' }));
    } catch (error) {
      setStatusMessage({ type: 'error', message: t('settings.passwordChangeError') });
    }
  };

  const handleRemoveExternalLogin = async (provider: string) => {
    try {
      await removeExternalLogin(provider);
      setExternalLogins(prev => prev.filter(p => p !== provider));
      setStatusMessage({ type: 'success', message: t('settings.externalLoginRemoved') });
    } catch (error) {
      setStatusMessage({ type: 'error', message: t('settings.externalLoginRemoveError') });
    }
  };

  const handleDeleteAccount = async () => {
    try {
      // TODO: Implement account deletion
      setDeleteDialogOpen(false);
      setStatusMessage({ type: 'success', message: t('settings.accountDeleted') });
    } catch (error) {
      setStatusMessage({ type: 'error', message: t('settings.accountDeleteError') });
    }
  };

  return (
    <Container maxWidth="lg">
      <Box sx={{ my: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          {t('settings.title')}
        </Typography>

        {statusMessage && (
          <Alert 
            severity={statusMessage.type as AlertProps['severity']} 
            onClose={() => setStatusMessage({ type: 'success', message: '' })}
            sx={{ mb: 2 }}
          >
            {statusMessage.message}
          </Alert>
        )}
        
        <Paper sx={{ width: '100%', mb: 2 }}>
          <Tabs
            value={tabValue}
            onChange={handleTabChange}
            indicatorColor="primary"
            textColor="primary"
            variant="fullWidth"
          >
            <Tab label={t('settings.general')} />
            <Tab label={t('settings.profile')} />
            <Tab label={t('settings.security')} />
            <Tab label={t('settings.externalLogins')} />
            <Tab label={t('settings.personalData')} />
          </Tabs>

          <TabPanel value={tabValue} index={0}>
            <FormControl fullWidth sx={{ mb: 3 }}>
              <Typography variant="h6" gutterBottom>
                {t('settings.language')}
              </Typography>
              <Select
                value={language}
                onChange={handleLanguageChange}
                displayEmpty
              >
                <MenuItem value="uk">Українська</MenuItem>
                <MenuItem value="en">English</MenuItem>
              </Select>
            </FormControl>

            <FormControl fullWidth>
              <Typography variant="h6" gutterBottom>
                {t('settings.notifications')}
              </Typography>
              <FormControlLabel
                control={
                  <Switch
                    checked={emailNotifications}
                    onChange={handleEmailNotificationsChange}
                  />
                }
                label={t('settings.emailNotifications')}
              />
            </FormControl>
          </TabPanel>

          <TabPanel value={tabValue} index={1}>
            <Typography variant="h6" gutterBottom>
              {t('settings.profileInfo')}
            </Typography>
            <TextField
              fullWidth
              label={t('settings.username')}
              name="username"
              value={profileData.username}
              onChange={handleProfileChange}
              sx={{ mb: 2 }}
            />
            <TextField
              fullWidth
              label={t('settings.email')}
              name="email"
              type="email"
              value={profileData.email}
              onChange={handleProfileChange}
              sx={{ mb: 2 }}
            />
            <Button
              variant="contained"
              color="primary"
              onClick={handleSaveProfile}
              sx={{ mt: 2 }}
            >
              {t('settings.saveChanges')}
            </Button>
          </TabPanel>

          <TabPanel value={tabValue} index={2}>
            <Typography variant="h6" gutterBottom>
              {t('settings.changePassword')}
            </Typography>
            <TextField
              fullWidth
              label={t('settings.currentPassword')}
              name="currentPassword"
              type="password"
              value={profileData.currentPassword}
              onChange={handleProfileChange}
              sx={{ mb: 2 }}
            />
            <TextField
              fullWidth
              label={t('settings.newPassword')}
              name="newPassword"
              type="password"
              value={profileData.newPassword}
              onChange={handleProfileChange}
              sx={{ mb: 2 }}
            />
            <TextField
              fullWidth
              label={t('settings.confirmPassword')}
              name="confirmPassword"
              type="password"
              value={profileData.confirmPassword}
              onChange={handleProfileChange}
              sx={{ mb: 2 }}
            />
            <Button
              variant="contained"
              color="primary"
              onClick={handleChangePassword}
              sx={{ mt: 2 }}
            >
              {t('settings.updatePassword')}
            </Button>
          </TabPanel>

          <TabPanel value={tabValue} index={3}>
            <Typography variant="h6" gutterBottom>
              {t('settings.externalLogins')}
            </Typography>
            <List>
              {externalLogins.map((provider) => (
                <ListItem key={provider}>
                  <ListItemText primary={provider} />
                  <ListItemSecondaryAction>
                    <IconButton
                      edge="end"
                      aria-label="delete"
                      onClick={() => handleRemoveExternalLogin(provider)}
                    >
                      <DeleteIcon />
                    </IconButton>
                  </ListItemSecondaryAction>
                </ListItem>
              ))}
            </List>
          </TabPanel>

          <TabPanel value={tabValue} index={4}>
            <Typography variant="h6" gutterBottom>
              {t('settings.personalData')}
            </Typography>
            <Button
              variant="contained"
              color="primary"
              onClick={() => setDeleteDialogOpen(true)}
              sx={{ mt: 2 }}
            >
              {t('settings.deleteAccount')}
            </Button>
          </TabPanel>
        </Paper>
      </Box>

      <Dialog
        open={deleteDialogOpen}
        onClose={() => setDeleteDialogOpen(false)}
      >
        <DialogTitle>{t('settings.deleteAccountConfirm')}</DialogTitle>
        <DialogContent>
          <Typography>
            {t('settings.deleteAccountWarning')}
          </Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setDeleteDialogOpen(false)}>
            {t('settings.cancel')}
          </Button>
          <Button onClick={handleDeleteAccount} color="error">
            {t('settings.delete')}
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default Settings; 