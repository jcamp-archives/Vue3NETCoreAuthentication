<template>
  <h2>Personal Data</h2>

  <div class="row">
    <div class="col-md-8">
      <p>
        Your account contains personal data that you have given us. This page allows you to download or delete that
        data.
      </p>
      <p>
        <strong>Deleting this data will permanently remove your account, and this cannot be recovered.</strong>
      </p>
      <p>
        <router-link to="/account/manage/personaldata/download" class="mt-4 btn">Download</router-link>
        <router-link to="/account/manage/personaldata/delete" class="ml-4 bg-red-500 btn">Delete</router-link>
      </p>
    </div>
  </div>
</template>
