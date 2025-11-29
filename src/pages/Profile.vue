<template>
    <div class="min-h-screen bg-gradient-to-r from-primary-600 to-primary-700">
        <div class="max-w-4xl mx-auto px-4 py-8">
            <div class="mb-8">
                <div class="flex items-center gap-4 mb-2">
                    <div class="p-3 bg-gradient-to-r from-primary-300 to-primary-400 rounded-xl">
                        <i class="fas fa-user text-2xl text-white"></i>
                    </div>
                    <div>
                        <h1 class="text-3xl font-bold text-secondary-900">Perfil</h1>
                        <p class="text-secondary-100">Gerencie suas informações pessoais</p>
                    </div>
                </div>
            </div>

            <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
                <!-- Profile Info -->
                <div class="lg:col-span-1">
                    <div class="bg-white rounded-2xl shadow-strong border border-primary-200 overflow-hidden">
                        <div class="bg-gradient-to-r from-primary-600 to-primary-700 px-6 py-4">
                            <h2 class="text-xl font-bold text-white">Informações Pessoais</h2>
                        </div>
                        <div class="p-6 text-center">
                            <div
                                class="w-24 h-24 mx-auto mb-4 rounded-full overflow-hidden border-4 border-primary-200">
                                <img src="https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-1.2.1&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80"
                                    alt="User Avatar" class="w-full h-full object-cover" />
                            </div>
                            <h3 class="text-xl font-semibold text-secondary-900">{{ user?.name || 'Usuário' }}</h3>
                            <p class="text-secondary-600">{{ user?.email || 'email@example.com' }}</p>
                            <div class="mt-4">
                                <span
                                    class="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium bg-primary-100 text-primary-800">
                                    {{ plan?.name || 'Plano' }}
                                </span>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Profile Details -->
                <div class="lg:col-span-2">
                    <div class="bg-white rounded-2xl shadow-strong border border-primary-200 overflow-hidden">
                        <div class="bg-gradient-to-r from-primary-600 to-primary-700 px-6 py-4">
                            <h2 class="text-xl font-bold text-white">Informações do Perfil</h2>
                        </div>
                        <div class="p-6">
                            <div class="space-y-6">
                                <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                                    <div>
                                        <label class="block text-sm font-medium text-secondary-700 mb-2">Nome</label>
                                        <p class="text-secondary-900">{{ user?.name || 'Usuário' }}</p>
                                    </div>
                                    <div>
                                        <label class="block text-sm font-medium text-secondary-700 mb-2">E-mail</label>
                                        <p class="text-secondary-900">{{ user?.email || 'email@example.com' }}</p>
                                    </div>
                                    <div>
                                        <label class="block text-sm font-medium text-secondary-700 mb-2">Correções Restantes</label>
                                        <p class="text-secondary-900">{{ plan?.maxCorrections > 1000000 ? 'Ilimitadas' : (plan?.maxCorrections || 0) - essays.length }}</p>
                                    </div>
                                    <div>
                                        <label class="block text-sm font-medium text-secondary-700 mb-2">Valor do Plano</label>
                                        <p class="text-secondary-900">R$ {{ plan?.price || '0' }}</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getCurrentUser } from '../services/auth'
import { getPlan } from '../services/plans'
import { getEssays } from '../services/essay'

const user = ref(null)
const plan = ref(null)
const essays = ref([])
const error = ref('')

onMounted(async () => {
    try {
        const userData = await getCurrentUser()
        user.value = userData
        if (userData?.planId) {
            const planData = await getPlan(userData.planId)
            plan.value = planData
        }
        const essaysData = await getEssays()
        essays.value = essaysData
    } catch (err) {
        console.error('Erro ao carregar usuário:', err)
        error.value = 'Erro ao carregar dados do perfil.'
    }
})
</script>