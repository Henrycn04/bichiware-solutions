import Vuex from 'vuex';


// This might require change considering how other pages will be restructured
export default new Vuex.Store({
    state:
    {
        userCredentials: {
            userId:         '16',
            password:       '',
            timeOfLogIn:    ''
        }
    },


    mutations:
    {
        logInUser(state, payload) {
            state.userCredentials.userId            = payload.userId;
            state.userCredentials.password          = payload.password;
            state.userCredentials.timeOfLogIn       = payload.timeOfLogIn;   
        }
    },


    actions:
    {

    },


    modules:
    {
        
    },


    getters:
    {
        getUserId(state) {
            return state.userCredentials.userId;
        }
    }
});
