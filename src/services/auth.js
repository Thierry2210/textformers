import axios from 'axios';
import { ref, computed } from 'vue'

const token = ref(localStorage.getItem('token'))

const isLoggedIn = computed(() => !!token.value)

function setToken(newToken) {
  token.value = newToken
  if (newToken) {
    localStorage.setItem('token', newToken)
  } else {
    localStorage.removeItem('token')
  }
}

export { isLoggedIn, setToken }

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
    setToken(token);
  }

  if (res.data?.userPermission === 'Admin') {
    localStorage.setItem('isAdmin', 'true');
  } else {
    localStorage.removeItem('isAdmin');
  }

  return res.data;
}

export async function getCurrentUser() {
  const res = await api.get('/User')

  if (res.data?.userPermission === 'Admin') {
    localStorage.setItem('isAdmin', 'true');
  } else {
    localStorage.removeItem('isAdmin');
  }

  return res.data
}

export async function registerUser(payload) {
  return api.post('/User', payload);
}

export async function updateUser(payload) {
  const res = await api.put('/User', payload);
  return res.data;
}

export function logoutUser() {
  setToken(null);
}

export async function getAllUsers() {
  const res = await api.get('/User/all')
  return res.data
}

export async function updateUserPermission(userId, permission) {
  const res = await api.patch('/permission', { id: userId, permission })
  return res.data
}

export default api;
