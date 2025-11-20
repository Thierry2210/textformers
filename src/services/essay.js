import axios from "axios";

const api = axios.create({
  baseURL: "https://localhost:7280"
});

export async function registerEssay(payload) {
  const res = await api.post("/user/register", payload);
  return res.data;
}

export default api;
