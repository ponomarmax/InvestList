export interface User {
  id: string;
  username: string;
  email: string;
  externalLogins?: string[];
  emailConfirmed?: boolean;
  createdAt?: Date;
} 