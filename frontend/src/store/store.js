import { createStore } from 'vuex'; 

export default createStore({
  state: {
    profile: null, 
    userCredentials: {
      userId: '16',
      password: '',
      timeOfLogIn: '',
      dateTimeLastRequestedCode: null,
    },
  },
  mutations: {
    setProfile(state, profile) {
      state.profile = profile;
    },
    clearProfile(state) {
      state.profile = null;
    },
    logInUser(state, payload) {
      state.userCredentials.userId = payload.userId;
      state.userCredentials.password = payload.password;
      state.userCredentials.timeOfLogIn = payload.timeOfLogIn;
    },
    clearUserCredentials(state) {
      state.userCredentials = { userId: '', password: '', timeOfLogIn: '' };
    },
    setDateTimeLastRequestedCode(state, dateTimeLastRequestedCode) {
      state.dateTimeLastRequestedCode = dateTimeLastRequestedCode;
    },
  },
  actions: {
    logIn({ commit }, { profile, credentials }) {
      commit('setProfile', profile);
      commit('logInUser', credentials);
    },
    logIn2({ commit },  profile ) {
      commit('setProfile', profile);
    },
    logOut({ commit }) {
      commit('clearProfile');
      commit('clearUserCredentials');
    },
  },
  getters: {
    isLoggedIn: (state) => !!state.profile,
    getProfile: (state) => state.profile,
    getUserId: (state) => state.userCredentials.userId,
    getDateTimeLastRequestedCode: (state) => state.dateTimeLastRequestedCode,
  }
});
