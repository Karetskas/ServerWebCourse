<template>
    <v-app>
        <v-app-bar app height="60" class="pa-0">
            <v-container class="pa-0">
                <v-row class="d-flex align-center ">
                    <v-col cols="4"
                           class="pa-0">
                        <v-btn block
                               text
                               tile
                               class="pa-0 rounded-l-xl button-font deep-purple--text text--darken-1 font-weight-black"
                               @click="changeHomeMenuTabColor"
                               :class="changeMenuTabColor(isChangedHomeMenuTabColor)">
                            <v-icon class="mdi mdi-home mr-1"></v-icon>
                            Home
                        </v-btn>
                    </v-col>

                    <v-col cols="4"
                           class="pa-0">
                        <v-btn block
                               text
                               tile
                               class="pa-0 button-font deep-purple--text text--darken-1 font-weight-black"
                               @click="changeAddMenuTabColor"
                               :class="changeMenuTabColor(isChangedAddMenuTabColor)">
                            <v-icon class="mdi mdi-account-plus mr-1"></v-icon>
                            Add
                        </v-btn>
                    </v-col>

                    <v-col cols="4"
                           class="pa-0">
                        <v-btn block
                               text
                               tile
                               class="pa-0 rounded-r-xl button-font deep-purple--text text--darken-1 font-weight-black"
                               @click="changeViewMenuTabColor"
                               :class="changeMenuTabColor(isChangedViewMenuTabColor)">
                            <v-icon class="mdi mdi-card-account-details mr-1"></v-icon>
                            View
                        </v-btn>
                    </v-col>
                </v-row>
            </v-container>
        </v-app-bar>
        <v-main class="mx-2">
            <router-view></router-view>
        </v-main>
    </v-app>
</template>

<script>
    export default {
        data: () => {
            return {
                isChangedHomeMenuTabColor: true,
                isChangedAddMenuTabColor: false,
                isChangedViewMenuTabColor: false
            }
        },

        computed: {

        },

        watch: {
            isChangedHomeMenuTabColor(val) {
                if (val) {
                    this.isChangedAddMenuTabColor = false;
                    this.isChangedViewMenuTabColor = false;
                }
            },

            isChangedAddMenuTabColor(val) {
                if (val) {
                    this.isChangedHomeMenuTabColor = false;
                    this.isChangedViewMenuTabColor = false;
                }
            },

            isChangedViewMenuTabColor(val) {
                if (val) {
                    this.isChangedHomeMenuTabColor = false;
                    this.isChangedAddMenuTabColor = false;
                }
            }
        },

        methods: {
            changeHomeMenuTabColor() {
                this.isChangedHomeMenuTabColor = true;
                this.followLink("/");
            },

            changeAddMenuTabColor() {
                this.isChangedAddMenuTabColor = true;
                this.followLink("/AddContact");
            },

            changeViewMenuTabColor() {
                this.isChangedViewMenuTabColor = true;

                //this.$store.dispatch("loadContacts");

                this.followLink("/ViewContacts");
            },

            changeMenuTabColor(selectedTab) {
                return selectedTab ? "green lighten-4" : "indigo lighten-4"
            },

            followLink(link) {
                this.$router.push(link)
                    .catch(error => {
                        if (error.name != "NavigationDublicated" && !error.message.includes('Avoided redundant navigation to current location')) {
                            console.log(error);
                        }
                    });
            }
        }
    }
</script>

<style scoped>
    @media(max-width: 600px){
        .button-font {
            font-size: 11px;
        }
    }
</style>