import axios from 'axios';

const api = axios.create({
  baseURL: 'https://localhost:7280',
  headers: { 'Content-Type': 'application/json' }
});

api.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token && config.headers) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export async function loginUser(payload) {
  const { email, password } = payload;

  const res = await api.post('/User/auth', { email, password });

  const token = res.data?.token ?? res.data;

  if (token) {
    localStorage.setItem('token', token);
  }

  return res.data;
}

export async function getCurrentUser() {
  const res = await api.get('/User')
  return res.data
}

export async function registerUser(payload) {
  return api.post('/User', payload);
}

export function logoutUser() {
  localStorage.removeItem('token');
}

export default api;
