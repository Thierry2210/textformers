<template>
  <div class="relative">
    <button @click="toggleMenu"
      class="h-8 w-8 rounded-full overflow-hidden border-2 border-primary-200 focus:outline-none focus:ring-2 focus:ring-primary-500">
      <img
        src="https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-1.2.1&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80"
        alt="User Avatar" class="h-full w-full object-cover" />
    </button>

    <div v-if="menuOpen"
      class="absolute right-0 mt-2 w-48 rounded-md bg-white shadow-lg ring-1 ring-black ring-opacity-5 z-50">
      <div class="py-1">
        <router-link to="/profile" class="block px-4 py-2 text-sm text-secondary-700 hover:bg-primary-50">
          Perfil
        </router-link>
        <router-link to="/settings" class="block px-4 py-2 text-sm text-secondary-700 hover:bg-primary-50">
          Configurações
        </router-link>
        <button @click="logout" class="w-full text-left block px-4 py-2 text-sm text-red-600 hover:bg-red-50">
          Sair
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { logoutUser } from '../services/auth'

const menuOpen = ref(false)
const router = useRouter()
const error = ref('')

function toggleMenu() {
  menuOpen.value = !menuOpen.value
}

// Logout
async function logout() {
  try {
    await logoutUser()
    console.log("Usuário deslogado")

    router.push("/login")
  } catch (err) {
    console.error("Erro ao deslogar:", err.response?.data || err)
    error.value = err.response?.data?.message || "Erro ao dar logout."
  }
}
</script>
