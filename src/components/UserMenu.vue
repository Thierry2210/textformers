<template>
  <div class="relative" v-if="isLoggedIn">
    <span class="mr-2 text-sm font-medium text-black" v-if="user">
      {{ user.name }}
    </span>

    <button @click="toggleMenu"
      class="h-8 w-8 rounded-full overflow-hidden border-2 border-primary-200 focus:outline-none focus:ring-2 focus:ring-primary-500">
      <img
        src="https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-1.2.1&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80"
        alt="User Avatar" class="h-full w-full object-cover" />
    </button>

    <div v-if="menuOpen"
      class="absolute right-0 mt-2 w-48 rounded-md bg-white shadow-lg ring-1 ring-black ring-opacity-5 z-50">
      <div class="py-1">
        <div v-if="user" class="px-4 py-2 text-xs text-secondary-500 border-b border-secondary-100">
          Logado como<br />
          <span class="font-semibold text-secondary-800">{{ user.name }}</span>
        </div>

        <router-link to="/profile" class="block px-4 py-2 text-sm text-secondary-700 hover:bg-primary-50">
          Perfil
        </router-link>

        <button @click="logout" class="w-full text-left block px-4 py-2 text-sm text-red-600 hover:bg-red-50">
          Sair
        </button>
      </div>
    </div>
  </div>

  <div v-else class="text-sm text-white">
    <router-link to="/login" class="text-black hover:text-primary-200">Entrar</router-link>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'
import { useRouter } from 'vue-router'
import { logoutUser, getCurrentUser, isLoggedIn } from '../services/auth'

const menuOpen = ref(false)
const router = useRouter()
const error = ref('')
const user = ref(null)

function toggleMenu() {
  if (!isLoggedIn.value) return
  menuOpen.value = !menuOpen.value
}

async function loadUser() {
  try {
    if (!isLoggedIn.value) {
      console.log('Nenhum usuário logado (sem token)')
      return
    }

    const data = await getCurrentUser()
    console.log('Usuário carregado:', data)
    user.value = data
  } catch (err) {
    console.error('Erro ao buscar usuário:', err.response?.data || err)
    error.value = err.response?.data?.message || 'Erro ao carregar usuário.'
  }
}

async function logout() {
  try {
    await logoutUser()
    console.log('Usuário deslogado')

    user.value = null
    menuOpen.value = false

    router.push('/login')
  } catch (err) {
    console.error('Erro ao deslogar:', err.response?.data || err)
    error.value = err.response?.data?.message || 'Erro ao dar logout.'
  }
}

onMounted(() => {
  loadUser()
})

watch(isLoggedIn, (newVal) => {
  if (newVal) {
    loadUser()
  } else {
    user.value = null
  }
})
</script>
