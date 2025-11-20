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
          <div class="flex rounded-full bg-secondary-800 p-1 text-sm font-semibold border border-primary-700">
            <button @click="billing = 'monthly'"
              :class="[billing === 'monthly' ? 'bg-primary-600 text-white shadow-lg' : 'text-primary-200 hover:text-white', 'rounded-full px-6 py-2 transition-all duration-200']">
              Mensal
            </button>
            <button @click="billing = 'annually'"
              :class="[billing === 'annually' ? 'bg-primary-600 text-white shadow-lg' : 'text-primary-200 hover:text-white', 'rounded-full px-6 py-2 transition-all duration-200']">
              Anual
            </button>
          </div>
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
              <span class="text-5xl font-bold text-white">R${{ billing === 'monthly' ? p.monthly : p.annually }}</span>
              <span class="text-xl font-medium text-primary-200 ml-2">BRL</span>
            </div>
            <p class="text-primary-200 text-sm mt-2">Cobrança {{ billing === 'monthly' ? 'mensal' : 'anual' }}</p>
          </div>

          <ul class="mt-6 space-y-4 flex-1">
            <li v-for="f in p.features" :key="f" class="flex items-center text-primary-100">
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
            <i class="fas fa-shopping-cart mr-2"></i>
            Assinar este plano
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
</template>

<script setup>
import { ref } from 'vue'

const billing = ref('monthly')
const openFAQ = ref(null)

const toggleFAQ = (i) => {
  openFAQ.value = openFAQ.value === i ? null : i
}

const subscribe = (planId) => {
  const plan = plans.find(p => p.id === planId)
  if (plan) {
    alert(`Você assinou o plano ${plan.name} por ${billing.value === 'monthly' ? 'R$' + plan.monthly + '/mês' : 'R$' + plan.annually + '/ano'}`)
  }
}

const plans = [
  {
    id: 'starter',
    name: 'Vestibulando',
    monthly: 19,
    annually: 190,
    features: [
      'Até 10 redações por mês',
      'Assistente de IA básico',
      'Correção gramatical automática',
      'Templates de redação simples'
    ],
  },
  {
    id: 'scale',
    name: 'ITA',
    monthly: 99,
    annually: 990,
    highlight: true,
    features: [
      'Redações ilimitadas',
      'Assistente de IA avançado',
      'Correção gramatical e estilística',
      'Templates personalizados',
      'Análise de plágio integrada',
      'Colaboração em tempo real',
      'Suporte prioritário 24/7'
    ],
  },
  {
    id: 'growth',
    name: 'MED',
    monthly: 49,
    annually: 490,
    features: [
      'Até 50 redações por mês',
      'Assistente de IA intermediário',
      'Correção gramatical e estilística',
      'Templates avançados',
      'Análise de plágio básica'
    ],
  },
]

const faqs = [
  { q: "Como funciona a cobrança?", a: "Você pode escolher entre pagamento mensal ou anual. O valor será cobrado automaticamente conforme sua escolha." },
  { q: "Posso cancelar a qualquer momento?", a: "Sim! Você pode cancelar sua assinatura quando quiser, sem taxas extras." },
  { q: "Quais métodos de pagamento são aceitos?", a: "Aceitamos cartões de crédito, débito e boleto bancário." },
  { q: "Existe período de teste?", a: "Sim, oferecemos 7 dias grátis para você experimentar todos os recursos." },
  { q: "Posso mudar de plano depois?", a: "Claro! Você pode alterar seu plano a qualquer momento pelo painel do usuário." },
  { q: "O suporte está disponível em quais horários?", a: "Nosso suporte funciona de segunda a sexta, das 8h às 18h." },
]
</script>