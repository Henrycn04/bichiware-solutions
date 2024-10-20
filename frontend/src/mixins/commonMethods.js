import { mapGetters, mapState, mapActions } from 'vuex';
import axios from "axios";
export default {
    computed: {
        ...mapGetters(['isLoggedIn']), 
        ...mapState(['userCredentials']),
    },
    data() {
        return {
            isCompaniesDropdownVisible: false,
            userCompanies: [],
            searchQuery: '',
            isProfileMenuVisible: false,
            isAdminOrEntrepreneur: false,
            searchResults: [],
        }
    },
    methods: {
        ...mapActions(['openCompany']),
        ...mapActions(['closeCompany']),
        getUserCompanies() {
            axios.get("https://localhost:7263/api/CompanyProfileData/UserCompanies", {
                params: {
                    userID: this.userCredentials.userId
                }
            })
                .then((response) => {
                    this.userCompanies = response.data;
                   
                })
                .catch((error) => {
                    console.error("Error obtaining user companies:", error);
                });
        },
        toggleCompaniesDropdown() {
            this.isCompaniesDropdownVisible = !this.isCompaniesDropdownVisible;
        },
        selectCompany(companyID) {
            this.openCompany(companyID);
            this.$router.push(`/companyProfile`);
        },
        ...mapGetters(["getUserType"]),
        performSearch() {
            const perishableRequest = axios.get("https://localhost:7263/api/products/search_perishable", {
                params: { searchTerm: this.searchQuery }
            });
        
            const nonPerishableRequest = axios.get("https://localhost:7263/api/products/search_non-perishable", {
                params: { searchTerm: this.searchQuery }
            });
        
            Promise.all([perishableRequest, nonPerishableRequest])
                .then(([perishableResponse, nonPerishableResponse]) => {
                    this.searchResults = [
                        ...perishableResponse.data,
                        ...nonPerishableResponse.data
                    ];
                    console.log(this.searchResults);
                })
                .catch((error) => {
                    console.error("Error during combined product search:", error);
                });
            this.$router.push(`/SearchPage`);
            
        },
        goToProfile() {
            this.$router.push('/profile');
        },
        goToCart() {
            this.$router.push('/cart');
        },
        goToHome() {
            this.$router.push('/');
        },
        toggleProfileMenu() { 
            this.isProfileMenuVisible = !this.isProfileMenuVisible;
        },
        goToAccount() {
            this.$router.push('/account');
            this.isProfileMenuVisible = false;
        },
        goToRegisterCompany() {
            this.$router.push('/register-company');
            this.isProfileMenuVisible = false;
        },
        goTologout() {
            console.log('Logout');
            this.$store.dispatch('logOut');
            this.isProfileMenuVisible = false;
            this.closeCompany();
        },
        goToLogin() {
            this.$router.push('/login'); 
        },
        handleClickOutside(event) {
            const profileContainer = this.$refs.profileContainer;
            if (profileContainer && !profileContainer.contains(event.target)) {
                this.isProfileMenuVisible = false;
            }
        }
    },
    mounted() {
        document.addEventListener('click', this.handleClickOutside);
       
        var userType = Number(this.getUserType()); 
        console.log(userType);
        this.isAdminOrEntrepreneur = userType === 2 || userType === 3;
        if (this.isAdminOrEntrepreneur) {
            this.getUserCompanies();
        }
    },
    beforeUnmount() {
        document.removeEventListener('click', this.handleClickOutside);
    }
}