import { ReactNode } from 'react';
import { RouteObject } from 'react-router-dom';

declare module 'react-router-dom' {
  export interface RouteObject {
    path: string;
    element: ReactNode;
    children?: RouteObject[];
  }

  export interface BrowserRouterProps {
    children?: ReactNode;
  }

  export function BrowserRouter(props: BrowserRouterProps): JSX.Element;
  export function Routes(props: { children?: ReactNode }): JSX.Element;
  export function Route(props: { path: string; element: ReactNode }): JSX.Element;
}

declare module '*.svg' {
  const content: string;
  export default content;
}

declare module '*.png' {
  const content: string;
  export default content;
}

declare module '*.jpg' {
  const content: string;
  export default content;
}

declare module '*.jpeg' {
  const content: string;
  export default content;
}

declare module '*.gif' {
  const content: string;
  export default content;
}

declare module '*.bmp' {
  const content: string;
  export default content;
}

declare module '*.tiff' {
  const content: string;
  export default content;
} 