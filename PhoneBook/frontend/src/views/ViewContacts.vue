<template>
    <v-container fill-height 
                 fluid 
                 class="pa-0">
        <v-row class="d-flex flex-column">
            <v-col>
                <h1 class="text-center deep-purple--text text--darken-1">View contacts</h1>
            </v-col>

            <v-col>
                <v-data-table v-model="selectedContacts"
                              :headers="headers"
                              :items="checkContacts"
                              :items-per-page="5"
                              :footer-props="{
                                showFirstLastPage: false,
                                itemsPerPageOptions: [5, 10, 50],
                                showCurrentPage: true
                              }"
                              :loading="$store.state.isLoading"
                              loading-text="Loading... Please wait!"
                              item-key="serialNumber"
                              class="elevation-1 deep-purple--text text--darken-3 indigo lighten-5"
                              show-select
                              hide-default-footer>
                    <template v-slot:header.serialNumber="{ header }">
                        <span class="deep-purple--text text--darken-3 text-subtitle-1 font-weight-bold">{{header.text}}</span>
                    </template>

                    <template v-slot:header.lastName="{ header }">
                        <span class="deep-purple--text text--darken-3 text-subtitle-1 font-weight-bold">{{header.text}}</span>
                    </template>

                    <template v-slot:header.firstName="{ header }">
                        <span class="deep-purple--text text--darken-3 text-subtitle-1 font-weight-bold">{{header.text}}</span>
                    </template>

                    <template v-slot:header.phoneNumbers="{ header }">
                        <span class="deep-purple--text text--darken-3 text-subtitle-1 font-weight-bold">{{header.text}}</span>
                    </template>

                    <template v-slot:header.action="{ header }">
                        <span class="deep-purple--text text--darken-3 text-subtitle-1 font-weight-bold">{{header.text}}</span>
                    </template>

                    <template v-slot:item.action="{  }">
                        <v-btn class="indigo lighten-4 deep-purple--text text--darken-1 font-weight-black"
                               elevation="3"
                               block>
                            <v-icon class="mdi mdi-delete-forever"></v-icon>
                        </v-btn>
                    </template>

                    <template v-slot:item.phoneNumbers="{ item }">
                        <div class="d-flex flex-column">
                            <v-chip v-for="phone in item.phoneNumbers"
                                    :key="phone.id"
                                    :color="getColor(phone.phoneType)"
                                    class="my-1 mx-auto pa-2"
                                    outlined>
                                <v-icon :class="getIcon(phone.phoneType)"
                                        class="mr-1"></v-icon>
                                {{phone.phone}}
                            </v-chip>
                        </div>
                    </template>

                    <template v-slot:header.data-table-select="{ on, props }">
                        <v-simple-checkbox v-bind="props"
                                           v-on="on"
                                           color="deep-purple lighten-2">
                        </v-simple-checkbox>
                    </template>


                    <template v-slot:item.data-table-select="{ isSelected, select }">
                        <v-simple-checkbox :value="isSelected"
                                           @input="select($event)"
                                           color="green lighten-2">
                        </v-simple-checkbox>
                    </template>

                    <template v-slot:progress>
                        <v-progress-linear :height="10"
                                           color="deep-purple dark-5"
                                           indeterminate>
                        </v-progress-linear>
                    </template>

                    <template v-slot:no-results>
                        <v-icon class="mr-1 mdi mdi-emoticon-sad-outline"
                                color="red lighten-2"></v-icon>
                        <span class="red--text text--lighten-2 font-weight-black">NOT FOUND!</span>
                        <v-icon class="ml-1 mdi mdi-emoticon-sad-outline"
                                color="red lighten-2"></v-icon>
                    </template>

                    <template v-slot:no-data>
                        <v-icon class="mr-1 mdi mdi-alert-outline"
                                color="red lighten-2"></v-icon>
                        <span class="red--text text--lighten-2 font-weight-black">NO DATA HERE!</span>
                        <v-icon class="ml-1 mdi mdi-alert-outline"
                                color="red lighten-2"></v-icon>
                    </template>

                    <template v-slot:top>
                        <v-container>
                            <v-row>
                                <v-col cols="3" class="d-flex align-center">
                                    <v-btn :disabled="disabledDeleteButton"
                                           elevation="5"
                                           block
                                           class="indigo lighten-4 deep-purple--text text--darken-1 font-weight-black">
                                        <v-icon class="mdi mdi-delete"></v-icon>
                                    </v-btn>
                                </v-col>

                                <v-col cols="9" class="d-flex align-center">
                                    <v-text-field v-model.trim="searchFilter"
                                                  clearable
                                                  outlined
                                                  dense
                                                  label="Search filter"
                                                  background-color="indigo lighten-5"
                                                  color="deep-purple darken-5"
                                                  placeholder="Find a phone or a person"
                                                  hide-details
                                                  class="text-field-color font-weight-bold rounded-r-0 rounded-xl">
                                    </v-text-field>

                                    <v-btn :disabled="disabledSearchButton"
                                           elevation="0" 
                                           small 
                                           height="100%"
                                           class="green lighten-3 deep-purple--text text--darken-1 font-weight-black rounded-l-0 rounded-xl px-0">
                                        <v-icon class="mdi mdi-account-search"></v-icon>
                                    </v-btn>
                                </v-col>
                            </v-row>
                        </v-container>
                    </template>

                    <!--created v-if-->
                    <template v-slot:footer>
                        <v-pagination v-model="pagination.page"
                                      :length="pagination.length"
                                      total-visible="7"
                                      color="indigo lighten-4"
                                      navigation-text-color="deep-purple darken-1"
                                      class="pagination-text-color">
                        </v-pagination>
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
                selectedContacts: [],
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
                    },
                    {
                        text: "Action",
                        value: "action",
                        align: "center"
                    }
                ],
                
                disabledDeleteButton: true,
                disabledSearchButton: true,

                searchFilter: "",

                pagination: {
                    page: 1,
                    length: 15
                }
            }
        },

        computed: {
            checkContacts() {
                return this.$store.state.contacts;
            }
        },

        watch: {
            selectedContacts(contacts) {
                this.disabledDeleteButton = contacts.length > 0 ? false : true;
            },

            searchFilter(text) {
                this.disabledSearchButton = text.length > 0 ? false : true;
            }
        },

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

<style scoped>
    .text-field-color >>> .v-text-field__slot input {
        color: rebeccapurple;
    }

    .pagination-text-color >>> .v-pagination__item {
        color: rebeccapurple;
    }
</style>