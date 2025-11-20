<template>
  <div class="bg-gradient-to-br from-secondary-900 via-primary-900 to-secondary-900 text-white min-h-screen">
    <AppHeader />
    <main class="flex-1">
      <router-view />
    </main>
    <footer class="border-t border-primary-200/20 bg-white/90 backdrop-blur-sm text-sm text-secondary-600 p-6 text-center">
      <div class="max-w-7xl mx-auto">
        <p>© {{ new Date().getFullYear() }} TextFormers — Transformando ideias em textos incríveis</p>
        <div class="mt-2 flex justify-center gap-6 text-xs">
          <a href="#" class="hover:text-primary-600 transition-colors duration-200">Privacidade</a>
          <a href="#" class="hover:text-primary-600 transition-colors duration-200">Termos</a>
          <a href="#" class="hover:text-primary-600 transition-colors duration-200">Suporte</a>
        </div>
      </div>
    </footer>
  </div>
</template>

<script setup>
import AppHeader from './components/AppHeader.vue'
</script>

<style>
/* Global styles */
html {
  scroll-behavior: smooth;
}

body {
  font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
  font-feature-settings: 'cv02', 'cv03', 'cv04', 'cv11';
  font-variation-settings: normal;
  text-rendering: optimizeLegibility;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

/* Custom scrollbar */
::-webkit-scrollbar {
  width: 8px;
}

::-webkit-scrollbar-track {
  background: rgba(255, 255, 255, 0.1);
}

::-webkit-scrollbar-thumb {
  background: rgba(168, 85, 247, 0.5);
  border-radius: 4px;
}

::-webkit-scrollbar-thumb:hover {
  background: rgba(168, 85, 247, 0.7);
}

/* Focus styles */
*:focus {
  outline: 2px solid rgba(168, 85, 247, 0.5);
  outline-offset: 2px;
}

/* Selection styles */
::selection {
  background-color: rgba(168, 85, 247, 0.3);
  color: white;
}
</style>
