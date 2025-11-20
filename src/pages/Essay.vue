<template>
  <div class="min-h-screen">
    <div class="max-w-7xl mx-auto px-4 py-8">
      <div class="grid grid-cols-1 xl:grid-cols-2 gap-6 lg:gap-8">
        <section class="bg-white rounded-2xl shadow-strong border border-primary-200 overflow-hidden">
          <div class="bg-gradient-to-r from-primary-600 to-primary-700 px-6 py-4">
            <h1 class="text-2xl font-bold text-white flex items-center gap-3">
              <i class="fas fa-edit text-2xl"></i>
              Nova Redação
            </h1>
          </div>

          <div class="p-6 space-y-6">
            <div class="space-y-2">
              <label class="text-sm font-medium text-secondary-700 flex items-center gap-2">
                <i class="fas fa-heading text-primary-500"></i>
                Título da Redação
              </label>
              <input v-model="title" placeholder="Digite o título da sua redação..."
                class="w-full text-black px-4 py-3 border border-primary-200 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent transition-all duration-200 bg-primary-50 hover:bg-white" />
            </div>

            <div class="space-y-2">
              <label class="text-sm font-medium text-secondary-700 flex items-center gap-2">
                <i class="fas fa-file-alt text-primary-500"></i>
                Conteúdo
              </label>
              <textarea v-model="content" rows="14" placeholder="Escreva sua redação aqui..."
                class="w-full text-black px-4 py-3 border border-primary-200 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent transition-all duration-200 bg-primary-50 hover:bg-white resize-none"></textarea>
            </div>

            <div class="space-y-4">
              <div class="flex flex-col sm:flex-row gap-3">
                <div class="flex-1">
                  <label class="block text-sm font-medium text-secondary-700 mb-2">
                    <i class="fas fa-upload text-green-500 mr-2"></i>
                    Carregar Arquivo
                  </label>
                  <input ref="fileInputRef" type="file" accept=".txt,.doc,.docx" @change="handleFile"
                    class="block w-full text-sm text-secondary-500 file:mr-4 file:py-2 file:px-4 file:rounded-lg file:border-0 file:text-sm file:font-medium file:bg-primary-50 file:text-primary-700 hover:file:bg-primary-100" />
                </div>
                <button type="button" @click="clearFile"
                  class="px-4 py-2 rounded-lg border border-red-300 text-red-600 hover:bg-red-50 transition-all duration-200 flex items-center gap-2 font-medium">
                  <i class="fas fa-sync-alt animate-spin" v-show="clearing"></i>
                  <i class="fas fa-trash-alt" v-show="!clearing"></i>
                  Limpar Redação
                </button>
              </div>
            </div>

            <div class="space-y-3">
              <button :disabled="!canSubmit" @click="submitEssay"
                class="w-full inline-flex items-center justify-center gap-3 px-6 py-4 rounded-lg text-white font-semibold shadow-medium transition-all duration-200 transform hover:scale-[1.02] active:scale-[0.98] focus:outline-none focus:ring-4 focus:ring-primary-300 disabled:cursor-not-allowed disabled:opacity-60 disabled:transform-none"
                :class="canSubmit ? 'bg-gradient-to-r from-primary-600 to-primary-700 hover:from-primary-700 hover:to-primary-800' : 'bg-secondary-400'">
                <i v-if="submitting" class="fas fa-spinner animate-spin text-xl"></i>
                <i v-else class="fas fa-paper-plane text-xl"></i>
                <span>{{ submitting ? 'Enviando...' : 'Enviar para Correção' }}</span>
              </button>

              <div class="flex items-center justify-between text-sm">
                <div class="flex items-center gap-2 text-secondary-600">
                  <i class="fas fa-chart-line text-green-500"></i>
                  <span>Correções restantes: <span class="font-semibold text-primary-600">{{ remaining }}</span></span>
                </div>
                <div v-if="!content.trim() || !title.trim()" class="flex items-center gap-2 text-red-600">
                  <i class="fas fa-exclamation-triangle"></i>
                  <span>Preencha título e conteúdo</span>
                </div>
              </div>
            </div>
          </div>
        </section>

        <section v-if="feedback" class="bg-white rounded-2xl shadow-strong border border-primary-200 overflow-hidden">
          <div class="bg-gradient-to-r from-green-600 to-emerald-600 px-6 py-4">
            <h2 class="text-2xl font-bold text-white flex items-center gap-3">
              <i class="fas fa-check-circle text-2xl"></i>
              Feedback da Correção
            </h2>
          </div>

          <div class="p-6 space-y-6">
            <div class="bg-gradient-to-r from-primary-50 to-primary-100 rounded-xl p-4 border border-primary-200">
              <div class="flex items-center justify-between">
                <div class="flex items-center gap-3">
                  <i class="fas fa-star text-yellow-500 text-2xl"></i>
                  <span class="text-lg font-medium text-secondary-700">Nota Final</span>
                </div>
                <div class="text-3xl font-bold text-primary-600">{{ feedback.score }}</div>
              </div>
            </div>

            <div class="space-y-2">
              <h3 class="text-lg font-semibold text-secondary-800 flex items-center gap-2">
                <i class="fas fa-comment-alt text-primary-500"></i>
                Resumo Geral
              </h3>
              <p class="text-secondary-700 bg-primary-50 rounded-lg p-4 border-l-4 border-primary-500">{{
                feedback.summary }}</p>
            </div>

          </div>
        </section>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onUnmounted } from 'vue'
import { loadEssays, addEssay } from '@/stores/essays.js'

const title = ref('')
const content = ref('')
const feedback = ref(null)
const remaining = ref(10)
const submitting = ref(false)
const clearing = ref(false)
let isMounted = true

onUnmounted(() => { isMounted = false })

const canSubmit = computed(() =>
  title.value.trim().length > 0 &&
  content.value.trim().length > 0 &&
  !submitting.value &&
  remaining.value > 0
)

function clearFile() {
  clearing.value = true
  setTimeout(() => {
    title.value = ''
    content.value = ''
    feedback.value = null
    clearing.value = false
  }, 500)
}

async function submitEssay() {
  if (!canSubmit.value) return
  submitting.value = true

  const fb = {
    summary: `Sua redação apresenta um ótimo domínio da norma padrão e demonstra maturidade argumentativa ao discutir a invisibilidade do trabalho de cuidado. 
    As referências a Djamila Ribeiro e Zygmunt Bauman enriquecem a reflexão e mostram repertório sociocultural produtivo. 
    Contudo, seria interessante aprimorar a fluidez entre os parágrafos e evitar repetições para tornar o texto mais coeso. 
    A conclusão é pertinente e propõe soluções viáveis, reforçando a competência de intervenção. Excelente desempenho geral!`,
    score: 9.5
  }

  feedback.value = fb
  addEssay({ title: title.value, content: content.value, feedback: fb })
  remaining.value--
  submitting.value = false
}

const fileInputRef = ref(null)

function handleFile(event) {
  const file = event.target.files[0]
  if (!file) return

  const reader = new FileReader()
  reader.onload = () => {
    content.value = reader.result 
  }
  reader.readAsText(file)
}

</script>
