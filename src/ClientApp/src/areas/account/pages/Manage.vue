<template>
  <h1>Manage your account</h1>

  <TwCard title="Change your account settings">
    <div class="flex flex-row">
      <div>
        <ManageNav></ManageNav>
      </div>
      <div class="w-full pl-5 pr-10">
        <router-view />
      </div>
    </div>
  </TwCard>
</template>

<script setup></script>

<route>
{meta: {auth: true}}
</route>
