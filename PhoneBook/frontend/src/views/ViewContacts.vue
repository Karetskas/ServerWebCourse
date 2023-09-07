<template>
    <v-container>
        <v-row class="d-flex flex-column">
            <v-col>
                <h1 class="text-center deep-purple--text text--darken-1">View contacts</h1>
            </v-col>

            <v-col>
                <v-data-table :headers="headers"
                              :items="checkContacts"
                              class="elevation-1 deep-purple--text text--darken-3 indigo lighten-5" 
                              :items-per-page="4"
                              :loading="$store.state.isLoading"
                              loading-text="Loading... Please wait!"
                              :footer-props="{
                                showFirstLastPage: false,
                                itemsPerPageOptions: [4, 5, 10, 50],
                                showCurrentPage: true
                              }">
                    <template v-slot:item.phoneNumbers="{item}">
                        <div class="d-flex flex-column">
                            <v-chip v-for="phone in item.phoneNumbers"
                                    :key="phone.serialNumber"
                                    :color="getColor(phone.phoneType)"
                                    class="my-1 mx-auto pa-2"
                                    outlined>
                                <v-icon class="mr-1"
                                        :class="getIcon(phone.phoneType)"></v-icon>
                                {{phone.phone}}
                            </v-chip>
                        </div>
                    </template>
                </v-data-table>
            </v-col>
        </v-row>
    </v-container>
</template>

<script>
    export default {
        data() {
            return {
                headers: [
                    {
                        text: "#",
                        value: "serialNumber",
                        align: "center"
                    },
                    {
                        text: "Last name",
                        value: "lastName",
                        align: "center"
                    },
                    {
                        text: "First name",
                        value: "firstName",
                        align: "center"
                    },
                    {
                        text: "Phone number",
                        value: "phoneNumbers",
                        align: "center"
                    }
                ],

                //contacts: []
            }
        },

        computed: {
            checkContacts() {
                return this.$state.store.contacts;
            }
        },

        //watch: {
        //    checkContacts(items){
        //        this.contacts = items;
        //    }
        //},

        mounted() {
            this.getContacts();
        },

        methods: {
            getColor(phoneType) {
                switch (phoneType) {
                    case "Home":
                        return "green lighten-1";
                    case "Work":
                        return "red lighten-1";
                    default:
                        return "blue lighten-1";
                }
            },

            getIcon(phoneType) {
                switch (phoneType) {
                    case "Home":
                        return "mdi mdi-home-outline";
                    case "Work":
                        return "mdi mdi-briefcase-outline";
                    default:
                        return "mdi mdi-cellphone-basic";
                }
            },

            getContacts() {
                this.$store.dispatch("loadContacts");
            }
        }
    }
</script>

<style>
    .v-data-table > .v-data-table__wrapper > table > thead > tr > th {
        font-size: 100% !important;
        color: #5A1FAC !important;
    }
</style>