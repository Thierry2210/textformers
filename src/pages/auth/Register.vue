<template>
  <div class="min-h-screen flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-md w-full">
      <div class="bg-white shadow-strong rounded-2xl p-6 sm:p-8 border border-primary-200">
        <h1 class="text-2xl font-semibold mb-6 text-secondary-900">Criar conta</h1>
        <form @submit.prevent="handleRegister" class="space-y-6">
          <p v-if="error" class="text-sm text-red-600 bg-red-50 border border-red-200 rounded-lg px-4 py-3">{{ error }}
          </p>
          <div>
            <label class="block text-sm font-medium text-secondary-700 mb-2">Nome</label>
            <input v-model="name" type="text" required
              class="w-full text-black bg-primary-50 border border-primary-200 rounded-lg px-4 py-3 focus:ring-2 focus:ring-primary-500 focus:border-transparent transition-all duration-200" />
          </div>
          <div>
            <label class="block text-sm font-medium text-secondary-700 mb-2">E-mail</label>
            <input v-model="email" type="email" required
              class="w-full text-black bg-primary-50 border border-primary-200 rounded-lg px-4 py-3 focus:ring-2 focus:ring-primary-500 focus:border-transparent transition-all duration-200" />
          </div>
          <div class="relative">
            <label class="block text-sm font-medium text-secondary-700 mb-2">Senha</label>
            <input v-model="password" :type="showPassword ? 'text' : 'password'" required
              class="w-full text-black bg-primary-50 border border-primary-200 rounded-lg px-4 py-3 pr-10 focus:ring-2 focus:ring-primary-500 focus:border-transparent transition-all duration-200" />
            <button type="button" @click="showPassword = !showPassword"
              class="absolute inset-y-0 right-0 pr-3 flex items-center text-gray-400 hover:text-gray-600">
              <svg v-if="showPassword" class="h-5 w-5 mt-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                  d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.878 9.878L3 3m6.878 6.878L21 21" />
              </svg>
              <svg v-else class="h-5 w-5 mt-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                  d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                  d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
              </svg>
            </button>
          </div>
          <button
            class="w-full bg-gradient-to-r from-primary-600 to-primary-700 text-white py-3 rounded-lg font-medium hover:from-primary-700 hover:to-primary-800 transition-all duration-200 transform hover:scale-105 shadow-medium">Cadastrar</button>
        </form>
        <div class="mt-6 text-sm text-center">
          <router-link class="text-primary-600 hover:text-primary-700 transition-colors duration-200" to="/login">Já tem
            conta? Entrar</router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue"
import { useRouter } from "vue-router"
import { registerUser } from '../../services/auth'

const name = ref("")
const email = ref("")
const password = ref("")
const error = ref("")
const showPassword = ref(false)
const router = useRouter()


async function handleRegister() {
  try {
    const data = {
      name: name.value,
      email: email.value,
      password: password.value,
      planId: 1,
      userPermission: "user"
    };

    const res = await registerUser(data);
    console.log("Usuário criado:", res);
    router.push("/login");
  } catch (err) {
    console.error("Erro:", err.response?.data || err);
    error.value = err.response?.data?.message || "Erro ao registrar usuário.";
  }
}


</script>
