<template>
  <div class="max-w-7xl mx-auto px-4 py-8">
    <div class="mb-8">
      <div class="flex items-center gap-4 mb-2">
        <div class="p-3 bg-gradient-to-r from-red-600 to-red-700 rounded-xl">
          <i class="fas fa-cog text-2xl text-white"></i>
        </div>
        <div>
          <h1 class="text-3xl font-bold text-white">Administração</h1>
          <p class="text-red-100">Gerencie usuários e planos</p>
        </div>
      </div>
    </div>

    <div class="grid grid-cols-1 lg:grid-cols-2 gap-8">
      <!-- Gerenciar Planos -->
      <div class="bg-white rounded-2xl shadow-strong border border-red-200 overflow-hidden">
        <div class="bg-gradient-to-r from-red-600 to-red-700 px-6 py-4">
          <h2 class="text-xl font-bold text-white flex items-center gap-3">
            <i class="fas fa-tags text-2xl"></i>
            Gerenciar Planos
          </h2>
        </div>
        <div class="p-6">
          <div v-if="loadingPlans" class="text-center py-4">Carregando planos...</div>
          <div v-else-if="errorPlans" class="text-red-600 bg-red-50 border border-red-200 rounded-lg px-4 py-3 mb-4">{{ errorPlans }}</div>
          <div v-else class="grid grid-cols-1 gap-4">
            <div v-for="plan in plans" :key="plan.id" class="border border-gray-200 rounded-lg p-4 hover:shadow-md transition-shadow">
              <div class="flex justify-between items-center mb-2">
                <h3 class="font-semibold text-lg">{{ plan.name }}</h3>
                <button @click="editPlan(plan)" class="text-blue-600 hover:text-blue-800 transition-colors">
                  <i class="fas fa-edit"></i> Editar
                </button>
              </div>
              <p class="text-sm text-gray-600 mb-1">Limite: {{ plan.maxCorrections }} correções</p>
              <p class="text-sm text-gray-600">Preço: R$ {{ plan.price }}</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Gerenciar Usuários -->
      <div class="bg-white rounded-2xl shadow-strong border border-red-200 overflow-hidden">
        <div class="bg-gradient-to-r from-red-600 to-red-700 px-6 py-4">
          <h2 class="text-xl font-bold text-white flex items-center gap-3">
            <i class="fas fa-users text-2xl"></i>
            Gerenciar Usuários
          </h2>
        </div>
        <div class="p-6">
          <div v-if="loadingUsers" class="text-center py-4">Carregando usuários...</div>
          <div v-else-if="errorUsers" class="text-red-600 bg-red-50 border border-red-200 rounded-lg px-4 py-3 mb-4">{{ errorUsers }}</div>
          <div v-else class="space-y-4 max-h-96 overflow-y-auto">
            <div v-for="user in users" :key="user.id" class="border border-gray-200 rounded-lg p-4">
              <div class="flex justify-between items-start mb-2">
                <div>
                  <h3 class="font-semibold">{{ user.name }}</h3>
                  <p class="text-sm text-gray-600">{{ user.email }}</p>
                </div>
                <div class="flex gap-2">
                  <select v-model="user.userPermission" @change="updateUserPerm(user)" class="text-sm border border-gray-300 rounded px-2 py-1">
                    <option value="User">Usuário</option>
                    <option value="Admin">Admin</option>
                  </select>
                  <select v-model="user.planId" @change="updateUserPlan(user)" class="text-sm border border-gray-300 rounded px-2 py-1">
                    <option value="">Nenhum</option>
                    <option v-for="plan in plans" :key="plan.id" :value="plan.id">{{ plan.name }}</option>
                  </select>
                </div>
              </div>
              <p class="text-sm text-gray-600">Plano: {{ user.planName || 'Nenhum' }}</p>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal para editar plano -->
    <div v-if="editingPlan" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div class="bg-white rounded-2xl p-6 w-full max-w-md">
        <h3 class="text-xl font-bold mb-4">Editar Plano</h3>
        <div class="space-y-4">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Nome</label>
            <input v-model="editingPlan.name" type="text" class="w-full border border-gray-300 rounded-lg px-3 py-2" />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Preço (R$)</label>
            <input v-model="editingPlan.price" type="number" step="0.01" class="w-full border border-gray-300 rounded-lg px-3 py-2" />
          </div>
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">Limite de Correções</label>
            <input v-model="editingPlan.maxCorrections" type="number" class="w-full border border-gray-300 rounded-lg px-3 py-2" />
          </div>
        </div>
        <div class="flex gap-4 mt-6">
          <button @click="savePlan" :disabled="savingPlan" class="flex-1 bg-red-600 text-white py-2 rounded-lg hover:bg-red-700 disabled:opacity-50">
            {{ savingPlan ? 'Salvando...' : 'Salvar' }}
          </button>
          <button @click="cancelEdit" class="flex-1 bg-gray-500 text-white py-2 rounded-lg hover:bg-gray-600">Cancelar</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getPlans, changePlan } from '../../services/plans.js'
import { getAllUsers, updateUserPermission, updateUser } from '../../services/auth.js'

const plans = ref([])
const users = ref([])
const loadingPlans = ref(false)
const loadingUsers = ref(false)
const errorPlans = ref('')
const errorUsers = ref('')
const editingPlan = ref(null)
const savingPlan = ref(false)

onMounted(async () => {
  await Promise.all([loadPlans(), loadUsers()])
})

async function loadPlans() {
  try {
    loadingPlans.value = true
    errorPlans.value = ''
    plans.value = await getPlans()
  } catch (error) {
    console.error('Erro ao carregar planos:', error)
    errorPlans.value = 'Erro ao carregar planos'
  } finally {
    loadingPlans.value = false
  }
}

async function loadUsers() {
  try {
    loadingUsers.value = true
    errorUsers.value = ''
    users.value = await getAllUsers()
  } catch (error) {
    console.error('Erro ao carregar usuários:', error)
    errorUsers.value = 'Erro ao carregar usuários'
  } finally {
    loadingUsers.value = false
  }
}

function editPlan(plan) {
  editingPlan.value = { ...plan }
}

async function savePlan() {
  try {
    savingPlan.value = true
    await changePlan(editingPlan.value.id, {
      name: editingPlan.value.name,
      price: editingPlan.value.price,
      maxCorrections: editingPlan.value.maxCorrections
    })
    await loadPlans()
    editingPlan.value = null
  } catch (error) {
    console.error('Erro ao salvar plano:', error)
    alert('Erro ao salvar plano')
  } finally {
    savingPlan.value = false
  }
}

async function updateUserPerm(user) {
  try {
    await updateUserPermission(user.id, user.userPermission)
    alert('Permissão do usuário atualizada com sucesso')
  } catch (error) {
    console.error('Erro ao atualizar permissão do usuário:', error)
    alert('Erro ao atualizar permissão do usuário')
  }
}

async function updateUserPlan(user) {
  try {
    await updateUser({ planId: user.planId })
    alert('Plano do usuário atualizado com sucesso')
  } catch (error) {
    console.error('Erro ao atualizar plano do usuário:', error)
    alert('Erro ao atualizar plano do usuário')
  }
}

function cancelEdit() {
  editingPlan.value = null
}

</script>


