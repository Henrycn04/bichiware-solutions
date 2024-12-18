import { createStore } from 'vuex'; 

export default createStore({
  state: {
    // get the data in storage or assign the default
    profile: JSON.parse(sessionStorage.getItem('profile')) || null,
    userCredentials: JSON.parse(sessionStorage.getItem('userCredentials')) || {
      userId: '',
      timeOfLogIn: '',
      userType: '',
      dateTimeLastRequestedCode: null,
    },
    idCompany: JSON.parse(sessionStorage.getItem('idCompany')) || null,
    idProduct: JSON.parse(sessionStorage.getItem('idProduct')) || null,  
    succesfulPayment: JSON.parse(sessionStorage.getItem('succesfulPayment')) || false,
    providedAddress: JSON.parse(sessionStorage.getItem('providedAddress')) || null,
    registerData: JSON.parse(sessionStorage.getItem('registerData')) || null,
    previousPage: JSON.parse(sessionStorage.getItem('previousPage')) || "",
    addressId: JSON.parse(sessionStorage.getItem('addressId')) || null,
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
     // save active company in storage
     setIdProduct(state, idProduct) { 
      state.idProduct = idProduct;
      sessionStorage.setItem('idProduct', JSON.stringify(idProduct)); 
    },
    // clear credential data in storage
    clearIdProduct(state) { 
      state.idProduct = null;
      sessionStorage.removeItem('idProduct');
    },
    setDateTimeLastRequestedCode(state, dateTimeLastRequestedCode) {
      state.dateTimeLastRequestedCode = dateTimeLastRequestedCode;
    },
    setBoolForPayment(state, payment) { 
     state.succesfulPayment = payment;
     sessionStorage.setItem('succesfulPayment', JSON.stringify(payment)); 
   },
    setProvidedAddress(state, providedAddress) {
      state.providedAddress = providedAddress;
      sessionStorage.setItem('providedAddress', JSON.stringify(providedAddress));
      console.log("setProvidedAddress", state.providedAddress);
    },
    setRegistrationData(state, registerData) {
      state.registerData = registerData;
      sessionStorage.setItem('registerData', JSON.stringify(registerData));
    },
    setPreviousPage(state, previousPage) {
      state.previousPage = previousPage;
      sessionStorage.setItem('previousPage', JSON.stringify(previousPage));
    },
    setAddressId(state, addressId) {
      state.addressId = addressId;
      sessionStorage.setItem('addressId', JSON.stringify(addressId));
    },
    clearAddressId(state) {
      state.addressId = null;
      sessionStorage.removeItem('addressId');
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
    openProduct({ commit }, idProduct){
      console.log('Opening product with ID:', idProduct);
      commit('setIdProduct', idProduct); 
    },
    logOut({ commit }) {
      commit('clearProfile');
      commit('clearUserCredentials');
      commit('clearIdCompany');
      commit('clearIdProduct'); 
    },
    closeCompany({ commit }){
      commit('clearIdCompany'); 
    },
    closeProduct({ commit }){
      commit('clearIdProduct'); 
    },
    paymentWasSuccesful({ commit }, payment) {
      commit('setBoolForPayment', payment);
    },
    saveAddress({ commit }, providedAddress) {
      commit('setProvidedAddress', providedAddress);
    },
    saveRegistrationData({ commit }, registrationData) {
      commit('setRegistrationData', registrationData);
    },
    setPrevPage({ commit }, previousPage) {
      commit('setPreviousPage', previousPage);
    }
  },
  getters: {
    isLoggedIn: (state) => !!state.profile,
    getProfile: (state) => state.profile,
    getUserId: (state) => state.userCredentials.userId,
    getDateTimeLastRequestedCode: (state) => state.dateTimeLastRequestedCode,
    getUserType: (state) => state.userCredentials.userType,
    getIdCompany: (state) => state.idCompany, 
    getIdProduct: (state) => state.idProduct, 
    getSuccesfulPayment: (state) => state.succesfulPayment,
    getSavedAddress: (state) => 
      state.providedAddress 
      ,
    getRegistrationData: (state) => state.registerData,
    getPreviousPage: (state) => state.previousPage,
    getAddressId: (state) => state.addressId,
  }
});