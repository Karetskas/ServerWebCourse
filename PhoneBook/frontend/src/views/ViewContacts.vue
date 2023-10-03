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
                              :items-per-page="rowsCount"
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
                                <v-col cols="12" sm="2" class="d-flex flex-column pb-0 pb-sm-3">
                                    <v-btn :disabled="disabledDeleteButton"
                                           elevation="5"
                                           block
                                           small
                                           class="indigo lighten-4 deep-purple--text text--darken-1 font-weight-black mb-1">
                                        <v-icon class="mdi mdi-delete"></v-icon>
                                    </v-btn>

                                    <v-btn elevation="5"
                                           block
                                           small
                                           class="indigo lighten-4 deep-purple--text text--darken-1 font-weight-black">
                                        <v-icon class="mdi mdi-download"></v-icon>
                                    </v-btn>
                                </v-col>

                                <v-col cols="12" sm="10" class="d-flex align-center my-3">
                                    <v-text-field v-model.trim="enteredFilterText"
                                                  outlined
                                                  dense
                                                  label="Search filter"
                                                  background-color="indigo lighten-5"
                                                  color="deep-purple darken-5"
                                                  placeholder="Find a phone or a person"
                                                  hide-details
                                                  class="text-field-color font-weight-bold rounded-r-0 rounded-xl">
                                    </v-text-field>

                                    <v-btn @click="clearSearchFilter()"
                                           elevation="0"
                                           tile
                                           x-small
                                           height="100%"
                                           class="indigo lighten-4 deep-purple--text text--darken-1 font-weight-black px-0">
                                        <v-icon class="mdi mdi-window-close"></v-icon>
                                    </v-btn>

                                    <v-btn :disabled="disabledSearchButton"
                                           @click="filterContacts"
                                           elevation="0"
                                           x-small
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
                        <v-container>
                            <v-row>
                                <v-col cols="12" sm="9" class="d-flex align-center justify-center">
                                    <v-pagination v-model="pageNumber"
                                                  :length="$store.state.pagesCount"
                                                  :total-visible="7"
                                                  color="indigo lighten-4"
                                                  navigation-text-color="deep-purple darken-1"
                                                  class="pagination-text-color">
                                    </v-pagination>
                                </v-col>

                                <v-spacer></v-spacer>

                                <v-col cols="12" sm="2" class="d-flex align-center">
                                    <v-autocomplete v-model="rowsCount"
                                                    :items="itemsPerPageCount"
                                                    @change="getContacts(undefined, 1, undefined)"
                                                    color="deep-purple darken-1"
                                                    background-color="indigo lighten-5"
                                                    item-color="deep-purple darken-1"
                                                    outlined
                                                    dense
                                                    hide-details></v-autocomplete>
                                </v-col>
                            </v-row>
                        </v-container>
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
                itemsPerPageCount: [3, 5, 10, 50],
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

                enteredFilterText: "",
            }
        },

        computed: {
            checkContacts() {
                return this.$store.state.contacts;
            },

            rowsCount: {
                get() {
                    return this.$store.state.page.rowsCount;
                },

                set(value) {
                    this.$store.commit('setRowsCount', value);
                }
            },

            pageNumber: {
                get() {
                    return this.$store.state.page.pageNumber;
                },

                set(value) {
                    this.$store.commit("setPageNumber", value);

                    this.getContacts(undefined, value, undefined);
                }
            }
        },

        watch: {
            selectedContacts(contacts) {
                this.disabledDeleteButton = contacts.length > 0 ? false : true;
            },

            enteredFilterText(text) {
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

            getContacts(searchFilterText = this.$store.state.searchFilterText, 
                pageNumber = this.$store.state.page.pageNumber, 
                rowsCount = this.$store.state.page.rowsCount) {
                let page = {
                    searchFilterText: searchFilterText,
                    pageNumber: pageNumber,
                    rowsCount: rowsCount
                }

                this.$store.dispatch("loadContacts", page);
            },

            clearSearchFilter() {
                this.enteredFilterText = "";

                this.getContacts(this.enteredFilterText, 1, undefined);
            },

            filterContacts() {
                this.getContacts(this.enteredFilterText, 1, undefined);
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

    .v-list >>> .v-list-item__title {
        color: rebeccapurple;
    }

    .v-autocomplete >>> input {
        color: rebeccapurple;
    }
</style>