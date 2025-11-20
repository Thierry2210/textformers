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
              
              <div v-if="it.feedback" class="text-right">
                <div class="flex items-center gap-2 mb-1">
                  <i class="fas fa-star text-yellow-500"></i>
                  <span class="text-lg font-bold text-gray-800">{{ it.feedback.score }}</span>
                </div>
                <div class="text-xs text-gray-500 max-w-xs">{{ it.feedback.summary }}</div>
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
                <div v-if="it.feedback.errors && it.feedback.errors.length > 0" class="bg-red-50 border border-red-200 rounded-lg p-4">
                  </div>

                <div v-if="it.feedback.suggestions && it.feedback.suggestions.length > 0" class="bg-yellow-50 border border-yellow-200 rounded-lg p-4">
                  </div>

                <div class="bg-gray-50 border border-gray-200 rounded-lg p-4">
                  </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
</template>

<script setup>
import { ref } from 'vue'

const items = ref([
  {
    id: 'essay-1', 
    title: 'Minha Redação de Exemplo 1', 
    content: 'Conteúdo da redação...',
    createdAt: new Date().toISOString(),
    feedback: {
      score: 8.5,
      summary: 'Bom texto, mas revise as vírgulas.',
      errors: [{ type: 'Pontuação', message: 'Vírgula fora de lugar' }],
      suggestions: ['Revisar uso de vírgulas.']
    }
  },
  {
    id: 'essay-2', 
    title: 'Outra Redação Corrigida', 
    content: 'Mais conteúdo...',
    createdAt: '2025-10-20T10:00:00.000Z',
    feedback: {
      score: 9.0,
      summary: 'Ótima argumentação.',
      errors: [],
      suggestions: ['Melhorar a introdução.']
    }
  }
])

const expandedItems = ref([])

function toggleDetails(itemId) {
  const index = expandedItems.value.indexOf(itemId)
  if (index > -1) {
    expandedItems.value.splice(index, 1)
  } else {
    expandedItems.value.push(itemId)
  }
}
</script>