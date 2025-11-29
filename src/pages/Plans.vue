<template>
    <!-- HERO -->
    <header class="relative isolate px-6 pt-14 lg:px-8">
      <div class="mx-auto max-w-4xl py-24 sm:py-32 lg:py-40 text-center">
        <div class="mb-8">
          <i class="fas fa-crown text-6xl text-primary-400 mb-6"></i>
        </div>
        <h1
          class="text-4xl font-bold tracking-tight sm:text-6xl bg-gradient-to-r from-white to-primary-200 bg-clip-text text-transparent">
          Planos que evoluem com você
        </h1>
        <p class="mt-6 text-lg leading-8 text-primary-100 max-w-2xl mx-auto">
          Escolha um plano acessível, repleto dos melhores recursos para engajar seu público, criar fidelidade e
          impulsionar resultados.
        </p>
        <div class="mt-10 flex justify-center">
          <p class="text-primary-100">Preços mensais</p>
        </div>
      </div>
    </header>

    <!-- PRICING -->
    <section class="mx-auto max-w-7xl px-6 pb-24 lg:px-8">
      <div class="grid gap-6 sm:gap-8 lg:grid-cols-3 lg:gap-x-8">
        <div v-for="p in plans" :key="p.id" :class="[
          'flex flex-col rounded-3xl bg-gradient-to-br from-primary-900 to-primary-800 p-6 shadow-strong ring-1 ring-primary-700 relative transition-all duration-300 hover:scale-105 sm:p-8',
          p.highlight ? 'ring-2 ring-primary-400 scale-105 sm:scale-110 z-10 shadow-primary-500/25' : ''
        ]">
          <!-- Badge para plano destacado -->
          <div v-if="p.highlight" class="absolute -top-4 left-1/2 transform -translate-x-1/2">
            <span
              class="bg-gradient-to-r from-primary-400 to-primary-500 text-white px-4 py-1 rounded-full text-sm font-bold">
              <i class="fas fa-star mr-1"></i>
              Mais Popular
            </span>
          </div>

          <div class="text-center mb-6">
            <h3 class="text-2xl font-bold text-white mb-2">{{ p.name }}</h3>
            <div class="flex items-baseline justify-center">
              <span class="text-5xl font-bold text-white">R${{ p.price }}</span>
              <span class="text-xl font-medium text-primary-200 ml-2">BRL</span>
            </div>
            <p class="text-primary-200 text-sm mt-2">Cobrança mensal</p>
          </div>

          <ul class="mt-6 space-y-4 flex-1">
            <li v-for="f in p.features || []" :key="f" class="flex items-center text-primary-100">
              <i class="fas fa-check-circle text-primary-400 mr-3 text-lg"></i>
              <span>{{ f }}</span>
            </li>
          </ul>

          <button @click="subscribe(p.id)" :class="[
            'mt-8 w-full rounded-lg px-6 py-4 text-center text-base font-semibold transition-all duration-200 transform hover:scale-105',
            p.highlight
              ? 'bg-gradient-to-r from-primary-500 to-primary-600 text-white shadow-lg hover:from-primary-600 hover:to-primary-700'
              : 'bg-primary-600 text-white hover:bg-primary-700'
          ]">
            <i class="fas fa-sync-alt mr-2"></i>
            Atualizar Plano
          </button>
        </div>
      </div>
    </section>

    <!-- FAQ -->
    <section class="max-w-4xl mx-auto mt-16 mb-16 px-6">
      <div class="bg-white/10 backdrop-blur-sm rounded-3xl p-8 border border-primary-200/20">
        <h2 class="text-3xl font-bold mb-8 text-center text-white">
          <i class="fas fa-question-circle mr-3 text-primary-400"></i>
          Perguntas frequentes
        </h2>
        <div class="space-y-4">
          <div v-for="(faq, idx) in faqs" :key="idx" class="border-b border-primary-200/20 py-6 last:border-b-0">
            <button @click="toggleFAQ(idx)" class="flex w-full justify-between items-center text-left group">
              <span class="text-lg font-medium text-white group-hover:text-primary-300 transition-colors duration-200">{{
                faq.q }}</span>
              <div
                class="flex items-center justify-center w-8 h-8 rounded-full bg-primary-600 text-white transition-all duration-200 group-hover:bg-primary-500">
                <i :class="openFAQ === idx ? 'fas fa-minus' : 'fas fa-plus'" class="text-sm"></i>
              </div>
            </button>
            <div v-if="openFAQ === idx" class="mt-4 text-primary-200 leading-relaxed">
              {{ faq.a }}
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Modal -->
    <div v-if="showModal" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div class="bg-white p-6 rounded-lg shadow-lg">
        <p class="text-secondary-900">{{ modalMessage }}</p>
        <button @click="showModal = false" class="mt-4 bg-primary-600 text-white px-4 py-2 rounded hover:bg-primary-700 transition-colors">
          OK
        </button>
      </div>
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getPlans } from '../services/plans'
import { updateUser } from '../services/auth'

const openFAQ = ref(null)
const plans = ref([])
const showModal = ref(false)
const modalMessage = ref('')

onMounted(async () => {
  try {
    const plansData = await getPlans()
    plans.value = plansData.map(p => {
      let features = []
      let highlight = false
      if (p.id === 1) {
        features = ['Até 5 correções por mês', 'Assistente de IA básico', 'Correção gramatical automática']
      } else if (p.id === 2) {
        features = ['Até 15 correções por mês', 'Assistente de IA intermediário', 'Correção gramatical e estilística']
      } else if (p.id === 3) {
        features = ['Correções ilimitadas', 'Assistente de IA avançado', 'Correção gramatical e estilística', 'Templates personalizados', 'Análise de plágio integrada', 'Suporte prioritário 24/7']
        highlight = true
      }
      return { ...p, features, highlight }
    })
  } catch (err) {
    console.error('Erro ao carregar planos:', err)
  }
})

const toggleFAQ = (i) => {
  openFAQ.value = openFAQ.value === i ? null : i
}

const subscribe = async (planId) => {
  try {
    await updateUser({ planId })
    modalMessage.value = 'Plano atualizado com sucesso!'
    showModal.value = true
  } catch (err) {
    console.error('Erro ao atualizar plano:', err)
    modalMessage.value = 'Erro ao atualizar plano.'
    showModal.value = true
  }
}

const faqs = [
  { q: "Como funciona a cobrança?", a: "Você pode escolher entre pagamento mensal ou anual. O valor será cobrado automaticamente conforme sua escolha." },
  { q: "Posso cancelar a qualquer momento?", a: "Sim! Você pode cancelar sua assinatura quando quiser, sem taxas extras." },
  { q: "Quais métodos de pagamento são aceitos?", a: "Aceitamos cartões de crédito, débito e boleto bancário." },
  { q: "Existe período de teste?", a: "Sim, oferecemos 7 dias grátis para você experimentar todos os recursos." },
  { q: "Posso mudar de plano depois?", a: "Claro! Você pode alterar seu plano a qualquer momento pelo painel do usuário." },
  { q: "O suporte está disponível em quais horários?", a: "Nosso suporte funciona de segunda a sexta, das 8h às 18h." },
]
</script>