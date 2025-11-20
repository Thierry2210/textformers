const STORAGE_KEY = 'tf_essays_v1'

export function loadEssays() {
    const raw = localStorage.getItem(STORAGE_KEY)
    return raw ? JSON.parse(raw) : []
}

export function saveEssays(essays) {
    localStorage.setItem(STORAGE_KEY, JSON.stringify(essays))
}

export function addEssay(item) {
    const essays = loadEssays()
    const newEssay = { id: crypto.randomUUID(), createdAt: new Date().toISOString(), ...item }
    essays.unshift(newEssay)
    saveEssays(essays)
    return newEssay
}

export function updateFeedback(id, feedback) {
    const essays = loadEssays()
    const idx = essays.findIndex(e => e.id === id)
    if (idx >= 0) {
        essays[idx].feedback = feedback
        saveEssays(essays)
    }
}
