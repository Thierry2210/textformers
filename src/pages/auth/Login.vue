<template>
  <div class="min-h-screen flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-md w-full">
      <div class="bg-white shadow-strong rounded-2xl p-6 sm:p-8 border border-primary-200">
        <h1 class="text-2xl font-semibold mb-6 text-secondary-900">Entrar</h1>
        <form @submit.prevent="handleLogin" class="space-y-6">
          <p v-if="error" class="text-sm text-red-600 bg-red-50 border border-red-200 rounded-lg px-4 py-3">{{ error }}
          </p>
          <div>
            <label class="block text-sm font-medium text-secondary-700 mb-2">E-mail</label>
            <input v-model="email" type="email" required
              class="w-full text-black bg-primary-50 border border-primary-200 rounded-lg px-4 py-3 focus:ring-2 focus:ring-primary-500 focus:border-transparent transition-all duration-200" />
          </div>
          <div>
            <label class="block text-sm font-medium text-secondary-700 mb-2">Senha</label>
            <input v-model="password" type="password" required
              class="w-full text-black bg-primary-50 border border-primary-200 rounded-lg px-4 py-3 focus:ring-2 focus:ring-primary-500 focus:border-transparent transition-all duration-200" />
          </div>
          <router-link to="/essay">
            <button
              class="w-full mt-6 bg-gradient-to-r from-primary-600 to-primary-700 text-white py-3 rounded-lg font-medium hover:from-primary-700 hover:to-primary-800 transition-all duration-200 transform hover:scale-105 shadow-medium">Entrar</button>
          </router-link>
        </form>
        <div class="mt-6 grid grid-cols-1 gap-3">
          <button
            class="w-full border border-primary-200 py-3 rounded-lg text-secondary-700 hover:bg-primary-50 transition-all duration-200"
            @click="auth.socialLogin('google')">Continuar com Google <i class="fa-brands fa-google w-4 h-4"></i></button>
        </div>
        <div class="mt-6 text-sm flex justify-between">
          <router-link class="text-primary-600 hover:text-primary-700 transition-colors duration-200"
            to="/register">Criar
            conta</router-link>
          <router-link class="text-primary-600 hover:text-primary-700 transition-colors duration-200"
            to="/forgot">Esqueci a
            senha</router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue"
import { useRouter } from "vue-router"
import { loginUser } from '../../services/auth'

const email = ref("")
const password = ref("")
const error = ref("")
const router = useRouter()

async function handleLogin() {
  try {
    const data = {
      email: email.value,
      password: password.value
    };

    const res = await loginUser(data);
    console.log("Usuário logado:", res);
    router.push("/dashboard");
  } catch (err) {
    console.error("Erro:", err.response?.data || err);
    error.value = err.response?.data?.message || "Erro ao registrar usuário.";
  }
}
</script>