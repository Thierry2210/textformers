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
          <div>
            <label class="block text-sm font-medium text-secondary-700 mb-2">Senha</label>
            <input v-model="password" type="password" required
              class="w-full text-black bg-primary-50 border border-primary-200 rounded-lg px-4 py-3 focus:ring-2 focus:ring-primary-500 focus:border-transparent transition-all duration-200" />
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
