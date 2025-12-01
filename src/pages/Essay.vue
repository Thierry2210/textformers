<template>
  <div class="max-w-7xl mx-auto px-4 py-8">
    <div class="mb-8">
      <div class="flex items-center justify-between mb-2">
        <div class="flex items-center gap-4">
          <div class="p-3 bg-gradient-to-r from-primary-600 to-primary-700 rounded-xl">
            <i class="fas fa-edit text-2xl text-white"></i>
          </div>
          <div>
            <h1 class="text-3xl font-bold text-white">Enviar Redação</h1>
            <p class="text-primary-100">Envie sua redação para correção</p>
          </div>
        </div>
        <div class="text-right">
          <p class="text-primary-100 text-sm font-medium">Correções Restantes</p>
          <p class="text-2xl font-bold text-white">{{ remainingCorrections > 1000 ? 'Ilimitadas' : remainingCorrections }}</p>
        </div>
      </div>
    </div>

    <div class="bg-white rounded-2xl shadow-strong border border-primary-200 overflow-hidden">
      <div class="bg-gradient-to-r from-primary-600 to-primary-700 px-6 py-4">
        <h2 class="text-xl font-bold text-white flex items-center gap-3">
          <i class="fas fa-file-alt text-2xl"></i>
          Detalhes da Redação
        </h2>
      </div>
      <div class="p-6">
        <form @submit.prevent="handleSubmit" class="space-y-6">
          <p v-if="error" class="text-sm text-red-600 bg-red-50 border border-red-200 rounded-lg px-4 py-3">
            {{ error }}
          </p>

          <div v-if="submittedEssay && submittedEssay.feedback" class="bg-green-50 border border-green-200 rounded-lg px-4 py-3">
            <h3 class="text-sm font-medium text-green-800 mb-2">Feedback da IA:</h3>
            <ul class="list-disc list-inside text-sm text-green-700 space-y-1 mb-2">
              <li v-for="point in feedbackPoints" :key="point">{{ point }}</li>
            </ul>
            <p class="text-sm text-green-600">Nota: {{ submittedEssay.score || 'N/A' }}</p>
            <div v-if="submittedEssay.correctedContent" class="mt-4">
              <h4 class="text-sm font-medium text-green-800 mb-2">Redação Corrigida:</h4>
              <div class="text-sm text-green-700 bg-green-50 p-3 rounded whitespace-pre-line">{{ submittedEssay.correctedContent }}</div>
            </div>
          </div>

          <div>
            <label class="block text-sm font-medium text-secondary-700 mb-2">Título da Redação</label>
            <input v-model="title" type="text" required
              class="w-full text-black bg-primary-50 border border-primary-200 rounded-lg px-4 py-3 focus:ring-2 focus:ring-primary-500 focus:border-transparent transition-all duration-200"
              placeholder="Digite o título da sua redação" />
          </div>

          <div>
            <label class="block text-sm font-medium text-secondary-700 mb-2">Conteúdo da Redação</label>
            <textarea v-model="content" required rows="15"
              class="w-full text-black bg-primary-50 border border-primary-200 rounded-lg px-4 py-3 focus:ring-2 focus:ring-primary-500 focus:border-transparent transition-all duration-200 resize-vertical"
              placeholder="Digite o conteúdo da sua redação aqui..."></textarea>
          </div>

          <div>
            <label class="block text-sm font-medium text-secondary-700 mb-2">Ou envie um arquivo (.txt, .docx)</label>
            <input type="file" @change="handleFileUpload" accept=".txt,.docx"
              class="w-full text-black bg-primary-50 border border-primary-200 rounded-lg px-4 py-3 focus:ring-2 focus:ring-primary-500 focus:border-transparent transition-all duration-200" />
          </div>

          <div class="flex gap-4">
            <button type="button" @click="clearForm"
              class="flex-1 bg-gray-500 text-white py-3 rounded-lg font-medium hover:bg-gray-600 transition-all duration-200 transform hover:scale-105 shadow-medium">
              <i class="fas fa-trash mr-2"></i>
              Limpar Redação
            </button>
            <button type="submit" :disabled="loading || remainingCorrections <= 0"
              class="flex-1 bg-gradient-to-r from-primary-600 to-primary-700 text-white py-3 rounded-lg font-medium hover:from-primary-700 hover:to-primary-800 transition-all duration-200 transform hover:scale-105 shadow-medium disabled:opacity-50 disabled:cursor-not-allowed">
              <i class="fas fa-paper-plane mr-2"></i>
              {{ loading ? 'Enviando...' : 'Enviar Redação' }}
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'
import { createEssay, getEssays } from '../services/essay.js'
import { getCurrentUser } from '../services/auth.js'
import { getPlan } from '../services/plans.js'

const title = ref('')
const content = ref('')
const error = ref('')
const loading = ref(false)
const router = useRouter()
const remainingCorrections = ref(0)
const submittedEssay = ref(null)

const feedbackPoints = computed(() => {
  if (!submittedEssay.value || !submittedEssay.value.feedback) return []
  return submittedEssay.value.feedback.split('\n-').filter(p => p.trim()).map(p => p.trim())
})

onMounted(async () => {
  try {
    const [essays, user] = await Promise.all([getEssays(), getCurrentUser()])
    let plan = null
    if (user.planId) {
      plan = await getPlan(user.planId)
    }
    if (plan && plan.maxCorrections > 1000000) {
      remainingCorrections.value = 999999 // Unlimited
    } else {
      remainingCorrections.value = (plan?.maxCorrections || 0) - essays.length
    }
  } catch (error) {
    console.error('Erro ao carregar dados:', error)
  }
})

function handleFileUpload(event) {
  const file = event.target.files[0]
  if (!file) return

  const reader = new FileReader()
  reader.onload = (e) => {
    content.value = e.target.result
    if (!title.value) {
      title.value = file.name.replace(/\.[^/.]+$/, '')
    }
  }
  reader.readAsText(file)
}

function clearForm() {
  title.value = ''
  content.value = ''
  error.value = ''
  submittedEssay.value = null
}

async function handleSubmit() {
  try {
    error.value = ''
    loading.value = true
    submittedEssay.value = null

    const payload = {
      title: title.value,
      content: content.value
    }

    const result = await createEssay(payload)
    submittedEssay.value = result

    if (!result.feedback) {
      setTimeout(() => {
        router.push('/dashboard')
      }, 2000)
    }
  } catch (err) {
    console.error('Erro ao enviar redação:', err)
    error.value = err.response?.data?.message || 'Erro ao enviar redação. Tente novamente.'
  } finally {
    loading.value = false
  }
}
</script>