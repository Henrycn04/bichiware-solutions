import { createStore } from 'vuex'; 

export default createStore({
  state: {
    // get the data in storage or assign the default
    profile: JSON.parse(sessionStorage.getItem('profile')) || null,
    userCredentials: JSON.parse(sessionStorage.getItem('userCredentials')) || {
      userId: '',
      timeOfLogIn: '',
      userType: '',
    },
    idCompany: JSON.parse(sessionStorage.getItem('idCompany')) || null, // Agregar el estado para idCompany
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
      // clear credential data in storage
      sessionStorage.removeItem('userCredentials'); 
    },
    // save active company in storage
    setIdCompany(state, idCompany) { 
      state.idCompany = idCompany;
      sessionStorage.setItem('idCompany', JSON.stringify(idCompany)); 
    },
    // clear credential data in storage
    clearIdCompany(state) { 
      state.idCompany = null;
      sessionStorage.removeItem('idCompany'); 
    },
  },
  actions: {
    logIn({ commit }, { profile, credentials}) { 
      commit('setProfile', profile);
      commit('logInUser', credentials);
    },
    openCompany({ commit }, idCompany){
      console.log('Opening company with ID:', idCompany);
      commit('setIdCompany', idCompany); 
    },
    logOut({ commit }) {
      commit('clearProfile');
      commit('clearUserCredentials');
      commit('clearIdCompany');
    },
    closeCompany({ commit }){
      commit('clearIdCompany'); 
    },
  },
  getters: {
    isLoggedIn: (state) => !!state.profile,
    getProfile: (state) => state.profile,
    getUserId: (state) => state.userCredentials.userId,
    getUserType: (state) => state.userCredentials.userType,
    getIdCompany: (state) => state.idCompany, 
  }
});
