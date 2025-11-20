import axios from "axios";

const api = axios.create({
  baseURL: "https://localhost:7280"
});

export function setAuthToken(token) {
  if (token) api.defaults.headers.common["Authorization"] = `Bearer ${token}`;
  else delete api.defaults.headers.common["Authorization"];
}

export async function registerUser(payload) {
  const res = await api.post("/user/register", payload);
  return res.data;
}

export async function loginUser(payload) {
  const res = await api.post("/user/login", payload);
  return res.data;
}

export async function logoutUser(payload) {
  const res = await api.post(`/user/logout/${userId}`, payload);
  return res.data;
}

export default api;
