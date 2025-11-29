import api from './auth.js';

export async function createEssay(payload) {
  const res = await api.post(`/Essay`, payload)
  return res.data
}

export async function getEssays() {
  const res = await api.get('/Essay')
  return res.data
}

export async function getEssayById(id) {
  const res = await api.get(`/Essay/${id}`)
  return res.data
}

export async function deleteEssay(id) {
  const res = await api.delete(`/Essay/${id}`)
  return res.data
}

export default api