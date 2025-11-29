<template>
  <header class="sticky top-0 z-50 bg-white/95 backdrop-blur-md border-b border-primary-200/50 shadow-soft">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex items-center justify-between h-16">
        <router-link to="/" class="flex items-center gap-3 group">
          <div class="relative">
            <div
              class="w-10 h-10 rounded-xl bg-gradient-to-br from-primary-600 via-primary-700 to-primary-800 text-white flex items-center justify-center font-bold text-lg shadow-medium group-hover:shadow-strong transition-all duration-200 group-hover:scale-105">
              T
            </div>
          </div>
          <div class="hidden sm:block">
            <span
              class="text-xl font-bold text-secondary-900 group-hover:text-primary-600 transition-colors duration-200">
              TextFormers
            </span>
            <div class="text-xs text-secondary-500 -mt-1">Assistente de Redação com IA</div>
          </div>
        </router-link>

        <nav class="hidden sm:flex items-center gap-1">
          <router-link v-if="!isLoggedIn" :class="navClass('/')" to="/">Início</router-link>
          <router-link v-if="isLoggedIn" :class="navClass('/dashboard')" to="/dashboard">Dashboard</router-link>
          <router-link v-if="isLoggedIn" :class="navClass('/essay')" to="/essay">Redação</router-link>
          <router-link v-if="isLoggedIn" :class="navClass('/plans')" to="/plans">Planos</router-link>
          <router-link v-if="isLoggedIn" :class="navClass('/history')" to="/history">Histórico</router-link>
        </nav>

        <UserMenu />
      </div>
    </div>

    <div v-if="isOpen" ref="menuRef" class="sm:hidden border-t border-primary-200/50 bg-white/95 backdrop-blur-md">
      <div class="max-w-7xl mx-auto px-4 py-4">
        <nav class="flex flex-col gap-1">
          <router-link v-if="!isLoggedIn" @click="isOpen = false" :class="mobileNavClass('/')" to="/">Início</router-link>
          <router-link v-if="isLoggedIn" @click="isOpen = false" :class="mobileNavClass('/dashboard')"
            to="/dashboard">Dashboard</router-link>
          <router-link v-if="isLoggedIn" @click="isOpen = false" :class="mobileNavClass('/essay')" to="/essay">Redação</router-link>
          <router-link v-if="isLoggedIn" @click="isOpen = false" :class="mobileNavClass('/plans')" to="/plans">Planos</router-link>
          <router-link v-if="isLoggedIn" @click="isOpen = false" :class="mobileNavClass('/history')" to="/history">Histórico</router-link>
        </nav>
      </div>
    </div>
  </header>
</template>

<script setup>
import { ref, watch, computed } from 'vue'
import { useRoute } from 'vue-router'
import UserMenu from '../components/UserMenu.vue'

const isOpen = ref(false)
const route = useRoute()

const isLoggedIn = computed(() => !!localStorage.getItem('token'))

watch(() => route.fullPath, () => (isOpen.value = false))

function navClass(prefix) {
  const base = 'px-4 py-2 rounded-lg text-sm font-medium transition-all duration-200 hover:bg-primary-50'
  return route.path.startsWith(prefix)
    ? base + ' text-primary-600 bg-primary-100'
    : base + ' text-secondary-700 hover:text-primary-600'
}

function mobileNavClass(prefix) {
  const base = 'px-4 py-3 rounded-lg text-sm font-medium transition-all duration-200 hover:bg-primary-50'
  return route.path.startsWith(prefix)
    ? base + ' text-primary-600 bg-primary-100'
    : base + ' text-secondary-700 hover:text-primary-600'
}
</script>