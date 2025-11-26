<template>
    <div class="max-w-6xl mx-auto px-4 py-8">
      <div class="bg-white rounded-2xl shadow-xl border border-gray-100 overflow-hidden mb-8">
        <div class="bg-gradient-to-r from-indigo-600 to-purple-600 px-6 py-6">
          <div class="flex items-center justify-between">
            <div class="flex items-center gap-4">
              <div class="p-3 bg-white/20 rounded-xl">
                <i class="fas fa-history text-2xl text-white"></i>
              </div>
              <div>
                <h1 class="text-2xl font-bold text-white">Histórico de Correções</h1>
                <p class="text-indigo-200">Acompanhe sua evolução na escrita</p>
              </div>
            </div>
            <div class="text-right">
              <div class="text-3xl font-bold text-white">{{ items.length }}</div>
              <div class="text-indigo-200 text-sm">redações corrigidas</div>
            </div>
          </div>
        </div>
      </div>

      <div v-if="items.length === 0" class="bg-white rounded-2xl shadow-xl border border-gray-100 p-12 text-center">
        <i class="fas fa-inbox text-6xl text-gray-300 mb-4"></i>
        <h3 class="text-xl font-semibold text-gray-600 mb-2">Nenhuma redação corrigida ainda</h3>
        <p class="text-gray-500">Envie sua primeira redação para começar a acompanhar seu progresso.</p>
      </div>

      <div v-else class="space-y-4">
        <div v-for="it in items" :key="it.id" class="bg-white rounded-2xl shadow-lg border border-gray-100 overflow-hidden hover:shadow-xl transition-all duration-200">
          <div class="p-6">
            <div class="flex items-start justify-between gap-4 mb-4">
              <div class="flex-1">
                <div class="flex items-center gap-3 mb-2">
                  <i class="fas fa-file-alt text-blue-500"></i>
                  <h3 class="text-lg font-semibold text-gray-800">{{ it.title }}</h3>
                </div>
                <div class="flex items-center gap-2 text-sm text-gray-500">
                  <i class="fas fa-calendar-alt"></i>
                  <span>{{ new Date(it.createdAt).toLocaleString('pt-BR') }}</span>
                </div>
              </div>
              
              <div class="text-right">
                <div class="flex items-center gap-2 mb-1">
                  <span class="text-lg font-bold text-gray-800">{{ it.score }}/1000</span>
                </div>
                <div class="text-xs text-gray-500 max-w-xs">Nota ENEM</div>
              </div>
            </div>

            <div v-if="it.feedback" class="border-t border-gray-100 pt-4">
              <div class="flex items-center justify-between mb-3">
                <h4 class="font-medium text-gray-700 flex items-center gap-2">
                  <i class="fas fa-chart-line text-green-500"></i>
                  Detalhes da Correção
                </h4>
                <button 
                  @click="toggleDetails(it.id)"
                  class="text-sm text-blue-600 hover:text-blue-800 flex items-center gap-1"
                >
                  <span>{{ expandedItems.includes(it.id) ? 'Ocultar' : 'Ver' }} detalhes</span>
                  <i :class="expandedItems.includes(it.id) ? 'fas fa-chevron-up' : 'fas fa-chevron-down'" class="text-xs"></i>
                </button>
              </div>

              <div v-if="expandedItems.includes(it.id)" class="space-y-4">
                <div class="bg-blue-50 border border-blue-200 rounded-lg p-4">
                  <h5 class="font-medium text-blue-700 mb-2 flex items-center gap-2">
                    <i class="fas fa-file-alt"></i>
                    Conteúdo Original
                  </h5>
                  <p class="text-blue-800 whitespace-pre-line">{{ it.content }}</p>
                </div>

                <div v-if="it.correctedContent" class="bg-green-50 border border-green-200 rounded-lg p-4">
                  <h5 class="font-medium text-green-700 mb-2 flex items-center gap-2">
                    <i class="fas fa-edit"></i>
                    Texto Corrigido
                  </h5>
                  <p class="text-green-800 whitespace-pre-line">{{ it.correctedContent }}</p>
                </div>

                <div class="bg-gray-50 border border-gray-200 rounded-lg p-4">
                  <h5 class="font-medium text-gray-700 mb-2 flex items-center gap-2">
                    <i class="fas fa-comments"></i>
                    Feedback Detalhado
                  </h5>
                  <p class="text-gray-800 whitespace-pre-line">{{ it.feedback }}</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getEssays } from '@/services/essay.js'

const items = ref([])
const expandedItems = ref([])

onMounted(async () => {
  try {
    const data = await getEssays()
    items.value = data
  } catch (error) {
    console.error('Erro ao carregar histórico:', error)
  }
})

function toggleDetails(itemId) {
  const index = expandedItems.value.indexOf(itemId)
  if (index > -1) {
    expandedItems.value.splice(index, 1)
  } else {
    expandedItems.value.push(itemId)
  }
}
</script>