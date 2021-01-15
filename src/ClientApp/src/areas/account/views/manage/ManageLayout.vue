<template>
  <h2>Manage your account</h2>

  <div>
    <h4>Change your account settings</h4>
    <hr />
    <div class="row">
      <div class="col-md-3">
        <ManageNav />
      </div>
      <div class="col-md-9">
        <router-view />
      </div>
    </div>
  </div>
</template>

<script setup>
import ManageNav from './ManageNav.vue'
</script>
