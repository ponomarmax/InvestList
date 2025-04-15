import React, { useState } from 'react';
import { Link, Outlet, useNavigate } from 'react-router-dom';
import {
  AppBar,
  Box,
  CssBaseline,
  Drawer,
  IconButton,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  Toolbar,
  Typography,
  useTheme,
  useMediaQuery,
  Container,
  Menu,
  MenuItem,
  Button,
} from '@mui/material';
import { styled } from '@mui/material/styles';
import {
  Menu as MenuIcon,
  Home as HomeIcon,
  Business as BusinessIcon,
  AttachMoney as AttachMoneyIcon,
  Language as LanguageIcon,
  Telegram as TelegramIcon,
  Instagram as InstagramIcon,
} from '@mui/icons-material';
import { useTranslation } from 'react-i18next';

const StyledAppBar = styled(AppBar)(() => ({
  backgroundColor: '#EAEAEA',
  color: '#333333',
  boxShadow: '0 2px 4px rgba(0,0,0,0.1)',
}));

const Logo = styled('img')({
  height: '50px',
  width: '50px',
});

const LogoText = styled(Link)({
  fontWeight: 700,
  fontSize: '20px',
  textDecoration: 'none',
  color: 'black',
  marginLeft: '8px',
});

const Footer = styled('footer')({
  borderTop: '1px solid #e5e5e5',
  backgroundColor: '#f8f9fa',
  padding: '20px 0',
  marginTop: 'auto',
});

const FooterLink = styled('a')({
  color: '#495057',
  textDecoration: 'none',
  '&:hover': {
    color: '#0056b3',
  },
});

const drawerWidth = 240;

interface LayoutProps {
  children: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ children }) => {
  const { i18n } = useTranslation();
  const [mobileOpen, setMobileOpen] = useState(false);
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('sm'));
  const navigate = useNavigate();
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);

  const handleDrawerToggle = () => {
    setMobileOpen(!mobileOpen);
  };

  const handleLanguageMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const changeLanguage = (lng: string) => {
    i18n.changeLanguage(lng);
    handleClose();
  };

  const menuItems = [
    { text: 'Home', icon: <HomeIcon />, path: '/' },
    { text: 'Projects', icon: <BusinessIcon />, path: '/projects' },
    { text: 'Investments', icon: <AttachMoneyIcon />, path: '/investments' },
  ];

  const drawer = (
    <div>
      <Toolbar />
      <List>
        {menuItems.map((item) => (
          <ListItem
            button
            key={item.text}
            onClick={() => {
              navigate(item.path);
              if (isMobile) {
                setMobileOpen(false);
              }
            }}
          >
            <ListItemIcon>{item.icon}</ListItemIcon>
            <ListItemText primary={item.text} />
          </ListItem>
        ))}
      </List>
    </div>
  );

  return (
    <Box sx={{ display: 'flex' }}>
      <CssBaseline />
      <AppBar
        position="fixed"
        sx={{
          width: { sm: `calc(100% - ${drawerWidth}px)` },
          ml: { sm: `${drawerWidth}px` },
        }}
      >
        <Toolbar>
          <IconButton
            color="inherit"
            aria-label="open drawer"
            edge="start"
            onClick={handleDrawerToggle}
            sx={{ mr: 2, display: { sm: 'none' } }}
          >
            <MenuIcon />
          </IconButton>
          <Typography variant="h6" noWrap component="div">
            InvestList
          </Typography>
        </Toolbar>
      </AppBar>
      <Box
        component="nav"
        sx={{ width: { sm: drawerWidth }, flexShrink: { sm: 0 } }}
      >
        <Drawer
          variant={isMobile ? 'temporary' : 'permanent'}
          open={isMobile ? mobileOpen : true}
          onClose={handleDrawerToggle}
          ModalProps={{
            keepMounted: true, // Better open performance on mobile.
          }}
          sx={{
            '& .MuiDrawer-paper': {
              boxSizing: 'border-box',
              width: drawerWidth,
            },
          }}
        >
          {drawer}
        </Drawer>
      </Box>
      <Box
        component="main"
        sx={{
          flexGrow: 1,
          p: 3,
          width: { sm: `calc(100% - ${drawerWidth}px)` },
        }}
      >
        <Toolbar />
        <Container component="main" sx={{ flex: 1, py: 4 }}>
          <Outlet />
        </Container>

        <Footer>
          <Container maxWidth="lg">
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', borderBottom: '1px solid #e5e5e5', pb: 3, mb: 3 }}>
              <Box>
                <Link to="/">
                  <Logo src="/ico/logo3.png" alt="invest radar logo" />
                </Link>
              </Box>
              <Box sx={{ display: 'flex', gap: 2 }}>
                <FooterLink href="https://forms.gle/BpSkLHWuq6y7yRiP9">
                  <Typography variant="body2">Лишити відгук</Typography>
                </FooterLink>
                <FooterLink href="https://forms.gle/DTgQQGUEixvhyLAA7">
                  <Typography variant="body2">Ваш досвід інвестування</Typography>
                </FooterLink>
                <Link to="/privacy-policy" style={{ color: '#495057', textDecoration: 'none' }}>
                  <Typography variant="body2">Політика конфіденційності</Typography>
                </Link>
              </Box>
              <Box sx={{ display: 'flex', gap: 2 }}>
                <FooterLink href="https://t.me/invest_radar_com">
                  <TelegramIcon />
                </FooterLink>
                <FooterLink href="https://www.instagram.com/invest_radar_com">
                  <InstagramIcon />
                </FooterLink>
              </Box>
            </Box>
          </Container>
        </Footer>
      </Box>
    </Box>
  );
};

export default Layout; 