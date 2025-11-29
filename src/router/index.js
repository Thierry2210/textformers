import { createRouter, createWebHistory } from 'vue-router'

const Home = () =>
    import('../pages/Home.vue')
const Login = () =>
    import('../pages/auth/Login.vue')
const Register = () =>
    import('../pages/auth/Register.vue')
const ForgotPassword = () =>
    import('../pages/auth/ForgotPassword.vue')
const Dashboard = () =>
    import('@/pages/Dashboard.vue')
const Essay = () =>
    import('../pages/Essay.vue')
const Plans = () =>
    import('../pages/Plans.vue')
const History = () =>
    import('../pages/History.vue')
const Profile = () =>
    import('../pages/Profile.vue')
const Admin = () =>
    import('../pages/admin/Admin.vue')

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        { path: '/', name: 'home', component: Home },
        { path: '/login', name: 'login', component: Login },
        { path: '/register', name: 'register', component: Register },
        { path: '/forgot', name: 'forgot', component: ForgotPassword },
        { path: '/dashboard', name: 'dashboard', component: Dashboard, meta: { requiresAuth: true } },
        { path: '/essay', name: 'essay', component: Essay, meta: { requiresAuth: true } },
        { path: '/plans', name: 'plans', component: Plans, meta: { requiresAuth: true } },
        { path: '/history', name: 'history', component: History, meta: { requiresAuth: true } },
        { path: '/profile', name: 'profile', component: Profile, meta: { requiresAuth: true } },
        { path: '/admin', name: 'admin', component: Admin, meta: { requiresAuth: true, requiresAdmin: true } },
    ],
})

router.beforeEach((to, from, next) => {
    const isLoggedIn = !!localStorage.getItem('token')

    if (to.meta.requiresAuth && !isLoggedIn) {
        next('/login')
    } else if (to.meta.requiresAdmin && (!isLoggedIn || !localStorage.getItem('isAdmin'))) {
        next('/dashboard')
    } else {
        next()
    }
})

export default router
