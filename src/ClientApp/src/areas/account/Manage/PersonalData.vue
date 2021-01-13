<template>
  <h4>Personal Data</h4>

  <div class="row">
    <div class="col-md-8">
      <p>
        Your account contains personal data that you have given us. This page allows you to download
        or delete that data.
      </p>
      <p>
        <strong
          >Deleting this data will permanently remove your account, and this cannot be
          recovered.</strong
        >
      </p>
      <p>
        <router-link to="/account/manage/personaldata/download" class="btn btn-primary"
          >Download</router-link
        >
      </p>
      <p>
        <router-link to="/account/manage/personaldata/delete" class="btn btn-secondary"
          >Delete</router-link
        >
      </p>
    </div>
  </div>
</template>
