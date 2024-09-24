import { createStore } from 'vuex'; 

export default createStore({
  state: {
    // get the data in storage or assaign the default
    profile: JSON.parse(sessionStorage.getItem('profile')) || null,
    userCredentials: JSON.parse(sessionStorage.getItem('userCredentials')) || {
      userId: '',
      timeOfLogIn: '',
      userType: '',
    },
  },
  mutations: {
    setProfile(state, profile) {
      state.profile = profile;
      // save data in storage
      sessionStorage.setItem('profile', JSON.stringify(profile)); 
    },
    clearProfile(state) {
      state.profile = null;
      // clear profile data in storage
      sessionStorage.removeItem('profile');
    },
    logInUser(state, payload) {
      state.userCredentials = payload;
      sessionStorage.setItem('userCredentials', JSON.stringify(payload));  
    },
    clearUserCredentials(state) {
      state.userCredentials = { userId: '', timeOfLogIn: '', userType: '' };
       //  clear credentail  data in storage
      sessionStorage.removeItem('userCredentials'); 
    }
  },
  actions: {
    logIn({ commit }, { profile, credentials }) {
      commit('setProfile', profile);
      commit('logInUser', credentials);
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
    getUserType: (state) => state.userCredentials.userType,
  }
});