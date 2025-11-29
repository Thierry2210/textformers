<template>
  <div class="max-w-7xl mx-auto px-4 py-8">
    <div class="mb-8">
      <div class="flex items-center gap-4 mb-2">
        <div class="p-3 bg-gradient-to-r from-primary-600 to-primary-700 rounded-xl">
          <i class="fas fa-tachometer-alt text-2xl text-white"></i>
        </div>
        <div>
          <h1 class="text-3xl font-bold text-white">Dashboard</h1>
          <p class="text-primary-100">Visão geral do seu progresso</p>
        </div>
      </div>
    </div>

    <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4 sm:gap-6 mb-8">
      <div
        class="bg-white rounded-2xl shadow-strong border border-primary-200 overflow-hidden hover:shadow-strong transition-all duration-200 transform hover:scale-105">
        <div class="bg-gradient-to-r from-green-500 to-emerald-600 px-6 py-4">
          <div class="flex items-center justify-between">
            <div>
              <p class="text-green-100 text-sm font-medium">Textos Corrigidos</p>
              <p class="text-3xl font-bold text-white">{{ totalCorrigidos }}</p>
            </div>
            <div class="p-3 bg-white/20 rounded-xl">
              <i class="fas fa-file-check text-2xl text-white"></i>
            </div>
          </div>
        </div>
        <div class="p-4">
          <div class="flex items-center text-sm text-secondary-600">
            <i class="fas fa-arrow-up text-green-500 mr-2"></i>
            <span>+2 este mês</span>
          </div>
        </div>
      </div>

      <div
        class="bg-white rounded-2xl shadow-strong border border-primary-200 overflow-hidden hover:shadow-strong transition-all duration-200 transform hover:scale-105">
        <div class="bg-gradient-to-r from-orange-500 to-red-600 px-6 py-4">
          <div class="flex items-center justify-between">
            <div>
              <p class="text-orange-100 text-sm font-medium">Erros Frequentes</p>
              <p class="text-3xl font-bold text-white">{{ principaisErros.length }}</p>
            </div>
            <div class="p-3 bg-white/20 rounded-xl">
              <i class="fas fa-exclamation-triangle text-2xl text-white"></i>
            </div>
          </div>
        </div>
        <div class="p-4">
          <div class="space-y-1">
            <div v-for="e in principaisErros.slice(0, 2)" :key="e" class="text-sm text-secondary-600 flex items-center">
              <i class="fas fa-circle text-orange-500 text-xs mr-2"></i>
              <span>{{ e }}</span>
            </div>
          </div>
        </div>
      </div>

      <div
        class="bg-white rounded-2xl shadow-strong border border-primary-200 overflow-hidden hover:shadow-strong transition-all duration-200 transform hover:scale-105">
        <div class="bg-gradient-to-r from-primary-500 to-primary-700 px-6 py-4">
          <div class="flex items-center justify-between">
            <div>
              <p class="text-primary-100 text-sm font-medium">Correções Restantes</p>
              <p class="text-3xl font-bold text-white">{{ remainingCorrections }}
              </p>
            </div>
            <div class="p-3 bg-white/20 rounded-xl">
              <i class="fas fa-gem text-2xl text-white"></i>
            </div>
          </div>
        </div>
        <div class="p-4">
          <div class="flex items-center text-sm text-secondary-600">
            <i class="fas fa-infinity text-primary-500 mr-2"></i>
            <span>Plano atual</span>
          </div>
        </div>
      </div>
    </div>

    <div class="bg-white rounded-2xl shadow-strong border border-primary-200 overflow-hidden mb-8">
      <div class="bg-gradient-to-r from-primary-600 to-primary-700 px-6 py-4">
        <h2 class="text-xl font-bold text-white flex items-center gap-3">
          <i class="fas fa-chart-line text-2xl"></i>
          Evolução das Correções
        </h2>
      </div>
      <div class="p-6">
        <div class="h-64 w-full">
          <Line :data="chartData" :options="chartOptions" />
        </div>
      </div>
    </div>

    <div class="bg-white rounded-2xl shadow-strong border border-primary-200 overflow-hidden">
      <div class="bg-gradient-to-r from-primary-600 to-primary-800 px-6 py-4">
        <h2 class="text-xl font-bold text-white flex items-center gap-3">
          <i class="fas fa-clock text-2xl"></i>
          Histórico Recente
        </h2>
      </div>
      <div class="p-6">
        <div v-if="recent.length > 0" class="space-y-4">
          <div v-for="it in recent" :key="it.id"
            class="flex items-center justify-between p-4 bg-primary-50 rounded-lg hover:bg-primary-100 transition-colors duration-200">
            <div class="flex items-center gap-4">
              <div class="p-2 bg-primary-100 rounded-lg">
                <i class="fas fa-file-alt text-primary-600"></i>
              </div>
              <div>
                <div class="font-medium text-secondary-800">{{ it.title }}</div>
                <div class="text-sm text-secondary-500 flex items-center gap-2">
                  <i class="fas fa-calendar-alt"></i>
                  <span>{{ new Date(it.createdAt).toLocaleString('pt-BR') }}</span>
                </div>
              </div>
            </div>
            <div v-if="it.feedback" class="flex items-center gap-2">
              <i class="fas fa-star text-yellow-500"></i>
              <span class="font-semibold text-secondary-700">{{ it.feedback.score }}</span>
            </div>
          </div>
        </div>
        <div v-else class="text-center py-8">
          <div class="mx-auto mb-4 w-16 h-16 rounded-full bg-primary-100 grid place-items-center">
            <i class="fas fa-file-alt text-2xl text-primary-400"></i>
          </div>
          <p class="text-secondary-500">Nenhuma redação encontrada</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getEssays } from '../services/essay.js'
import { Line } from 'vue-chartjs'
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
} from 'chart.js'

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
)

const loading = ref(true)
const totalCorrigidos = ref(0)
const principaisErros = ref(['Vírgula', 'Concordância'])
const remainingCorrections = ref(9)
const recent = ref([])

const chartData = ref({
  labels: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun'],
  datasets: [
    {
      label: 'Correções',
      data: [2, 5, 3, 8, 6, 10],
      borderColor: '#3B82F6',
      backgroundColor: 'rgba(59, 130, 246, 0.1)',
      tension: 0.4,
      fill: true
    }
  ]
})

const chartOptions = ref({
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: {
      display: false
    },
    tooltip: {
      backgroundColor: 'rgba(0, 0, 0, 0.8)',
      titleColor: '#fff',
      bodyColor: '#fff'
    }
  },
  scales: {
    y: {
      beginAtZero: true,
      grid: {
        color: 'rgba(0, 0, 0, 0.1)'
      },
      ticks: {
        color: '#6B7280'
      }
    },
    x: {
      grid: {
        display: false
      },
      ticks: {
        color: '#6B7280'
      }
    }
  }
})

onMounted(async () => {
  try {
    const essays = await getEssays()
    totalCorrigidos.value = essays.length
    recent.value = essays.slice(-5).reverse()
  } catch (error) {
    console.error('Erro ao carregar dados do dashboard:', error)
  } finally {
    loading.value = false
  }
})
</script>