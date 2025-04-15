import React, { createContext, useContext, useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { User } from '../types/user';

interface AuthContextType {
  user: User | null;
  loading: boolean;
  error: string | null;
  updateProfile: (data: { username: string; email: string }) => Promise<void>;
  changePassword: (currentPassword: string, newPassword: string) => Promise<void>;
  removeExternalLogin: (provider: string) => Promise<void>;
  logout: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    // Load user data from localStorage or API
    const loadUser = async () => {
      try {
        const token = localStorage.getItem('token');
        if (token) {
          // TODO: Implement API call to get user data
          // const response = await fetch('/api/user/me', {
          //   headers: { Authorization: `Bearer ${token}` }
          // });
          // const userData = await response.json();
          // setUser(userData);
        }
      } catch (err) {
        setError('Failed to load user data');
      } finally {
        setLoading(false);
      }
    };

    loadUser();
  }, []);

  const updateProfile = async (data: { username: string; email: string }) => {
    try {
      // TODO: Implement API call to update profile
      // const response = await fetch('/api/user/profile', {
      //   method: 'PUT',
      //   headers: {
      //     'Content-Type': 'application/json',
      //     Authorization: `Bearer ${localStorage.getItem('token')}`
      //   },
      //   body: JSON.stringify(data)
      // });
      // const updatedUser = await response.json();
      // setUser(updatedUser);
    } catch (err) {
      throw new Error('Failed to update profile');
    }
  };

  const changePassword = async (currentPassword: string, newPassword: string) => {
    try {
      // TODO: Implement API call to change password
      // const response = await fetch('/api/user/password', {
      //   method: 'PUT',
      //   headers: {
      //     'Content-Type': 'application/json',
      //     Authorization: `Bearer ${localStorage.getItem('token')}`
      //   },
      //   body: JSON.stringify({ currentPassword, newPassword })
      // });
      // if (!response.ok) {
      //   throw new Error('Failed to change password');
      // }
    } catch (err) {
      throw new Error('Failed to change password');
    }
  };

  const removeExternalLogin = async (provider: string) => {
    try {
      // TODO: Implement API call to remove external login
      // const response = await fetch(`/api/user/external-login/${provider}`, {
      //   method: 'DELETE',
      //   headers: {
      //     Authorization: `Bearer ${localStorage.getItem('token')}`
      //   }
      // });
      // if (!response.ok) {
      //   throw new Error('Failed to remove external login');
      // }
      // const updatedUser = await response.json();
      // setUser(updatedUser);
    } catch (err) {
      throw new Error('Failed to remove external login');
    }
  };

  const logout = async () => {
    try {
      // TODO: Implement API call to logout
      // await fetch('/api/auth/logout', {
      //   method: 'POST',
      //   headers: {
      //     Authorization: `Bearer ${localStorage.getItem('token')}`
      //   }
      // });
      localStorage.removeItem('token');
      setUser(null);
      navigate('/login');
    } catch (err) {
      throw new Error('Failed to logout');
    }
  };

  return (
    <AuthContext.Provider
      value={{
        user,
        loading,
        error,
        updateProfile,
        changePassword,
        removeExternalLogin,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
}; 