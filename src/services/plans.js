import api from './auth.js';

export async function postPlan(payload) {
  const res = await api.post(`/Plan`, payload)
  return res.data
}

export async function getPlans() {
  const res = await api.get('/Plan')
  return res.data
}

export async function getPlan(id) {
  const res = await api.get(`/Plan/${id}`)
  return res.data
}

export async function changePlan(id, payload) {
  const res = await api.put(`/Plan/${id}`, payload)
  return res.data
}

export async function deletePlan(id) {
  const res = await api.delete(`/Plan/${id}`)
  return res.data
}