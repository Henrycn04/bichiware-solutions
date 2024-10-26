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
            isAdmin: false
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
            this.$router.push({
                path: '/SearchPage',
                query: { search: this.searchQuery }
            });
           
        },
        goToProfile() {
            this.$router.push('/profile');
        },
        goToCart() {
            this.$router.push('/shoppingCart');
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
        this.isAdmin = userType === 3;
        if (this.isAdminOrEntrepreneur) {
            this.getUserCompanies();
        }
    },
    beforeUnmount() {
        document.removeEventListener('click', this.handleClickOutside);
    }
}