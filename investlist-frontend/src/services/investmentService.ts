import axios from 'axios';
import { ApiResponse } from '../types/api';

export interface InvestmentPost {
  id: string;
  slug: string;
  title: string;
  description: string;
  content: string;
  createdAt: string;
  updatedAt: string;
  language: string;
  tags: string[];
  minInvestValues: {
    currency: number;
    minValue: number;
  }[];
  investDurationYears: number;
  investDurationMonths: number;
  totalInvestment: number;
  annualInvestmentReturn: number;
}

export interface InvestmentPostList {
  items: InvestmentPost[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5001';

// Create an axios instance with default config
const api = axios.create({
  baseURL: API_URL,
  withCredentials: true,
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  }
});

// Add a response interceptor to handle errors
api.interceptors.response.use(
  response => response,
  error => {
    console.error('API Error:', error);
    return Promise.reject(error);
  }
);

export const investmentService = {
  getInvestmentsCount: async (): Promise<number> => {
    const response = await api.get<ApiResponse<{ count: number }>>('/api/investments/count');
    return response.data.data?.count ?? 0;
  },

  getInvestments: async (
    page: number = 1,
    pageSize: number = 10,
    search?: string,
    language: string = 'uk',
    tagIds?: string[]
  ): Promise<InvestmentPostList> => {
    const params = new URLSearchParams({
      page: page.toString(),
      pageSize: pageSize.toString(),
      language,
      ...(search && { search }),
      ...(tagIds && { tagIds: tagIds.join(',') })
    });

    const response = await api.get<ApiResponse<InvestmentPostList>>(
      `/api/investments?${params.toString()}`
    );
    return response.data.data ?? { items: [], totalCount: 0, page, pageSize, totalPages: 0, hasNextPage: false, hasPreviousPage: false };
  },

  getInvestment: async (slug: string, language: string = 'uk'): Promise<InvestmentPost | null> => {
    const response = await api.get<ApiResponse<InvestmentPost>>(
      `/api/investments/${slug}?language=${language}`
    );
    return response.data.data ?? null;
  },

  createInvestment: async (data: Partial<InvestmentPost>): Promise<InvestmentPost> => {
    const response = await api.post<ApiResponse<InvestmentPost>>(
      '/api/investments',
      data
    );
    if (!response.data.data) {
      throw new Error(response.data.message ?? 'Failed to create investment');
    }
    return response.data.data;
  },

  updateInvestment: async (id: string, data: Partial<InvestmentPost>): Promise<InvestmentPost> => {
    const response = await api.put<ApiResponse<InvestmentPost>>(
      `/api/investments/${id}`,
      data
    );
    if (!response.data.data) {
      throw new Error(response.data.message ?? 'Failed to update investment');
    }
    return response.data.data;
  }
}; 