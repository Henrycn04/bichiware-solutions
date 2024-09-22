import { createStore } from 'vuex'; 

export default createStore({
  state: {
    profile: null, 
  },
  mutations: {
    setProfile(state, profile) {
      state.profile = profile;
    },
    clearProfile(state) {
      state.profile = null;
    },
  },
  actions: {
    logIn({ commit }, profile) {
      commit('setProfile', profile);
    },
    logOut({ commit }) {
      commit('clearProfile');
    },
  },
  getters: {
    isLoggedIn: (state) => !!state.profile,
    getProfile: (state) => state.profile,
  },
});