<template>
  <div class="min-h-screen flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-md w-full">
      <div class="bg-white shadow-strong rounded-2xl p-6 sm:p-8 border border-primary-200">
        <h1 class="text-2xl font-semibold mb-6 text-secondary-900">Recuperar senha</h1>
        <form @submit.prevent="handleSubmit" class="space-y-6">
          <p class="text-sm text-secondary-600 mb-4">Digite seu e-mail para receber um link de recuperação de senha.</p>
          <div>
            <label class="block text-sm font-medium text-secondary-700 mb-2">E-mail</label>
            <input v-model="email" type="email" required
              class="w-full text-black bg-primary-50 border border-primary-200 rounded-lg px-4 py-3 focus:ring-2 focus:ring-primary-500 focus:border-transparent transition-all duration-200" />
          </div>
          <button
            class="w-full bg-gradient-to-r from-primary-600 to-primary-700 text-white py-3 rounded-lg font-medium hover:from-primary-700 hover:to-primary-800 transition-all duration-200 transform hover:scale-105 shadow-medium">Enviar
            link</button>
        </form>
        <div class="mt-6 text-sm text-center">
          <router-link class="text-primary-600 hover:text-primary-700 transition-colors duration-200" to="/login">Voltar
            ao login</router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'

const email = ref('')

function handleSubmit() {
  console.log('Tentativa de recuperar senha (visual):', email.value)
  alert('Funcionalidade desativada (apenas visual).')
}
</script>
