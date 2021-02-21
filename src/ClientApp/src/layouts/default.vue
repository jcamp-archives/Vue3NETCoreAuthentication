<template>
  <div class="flex flex-col w-full h-screen md:flex-row">
    <NavMenu />
    <div class="flex flex-col flex-grow w-full bg-white">
      <div
        class="flex-row items-center justify-end flex-shrink-0 hidden px-4 pr-8 text-blue-500 bg-gray-100 border-b border-gray-300 md:flex h-14"
      >
        <a href="https://github.com/SoftwareAteliers/asp-net-core-vue-starter" target="_blank" class="ml-md-auto"
          >About</a
        >
      </div>
      <div class="flex-grow py-5 pl-8 pr-6">
        <router-view />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts"></script>
