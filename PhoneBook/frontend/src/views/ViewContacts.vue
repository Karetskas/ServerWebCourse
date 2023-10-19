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
                              show-select
                              class="elevation-1 deep-purple--text text--darken-3 indigo lighten-5"
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

                    <template v-slot:header.data-table-select="{ on, props }">
                        <v-simple-checkbox v-bind="props"
                                           v-on="on"
                                           color="deep-purple lighten-2">
                        </v-simple-checkbox>
                    </template>

                    <template v-slot:item.data-table-select="{ isSelected, select }">
                        <v-simple-checkbox :value="isSelected"
                                           @input="select($event)"
                                           color="deep-purple accent-3">
                        </v-simple-checkbox>
                    </template>

                    <template v-slot:item.action="{ item }">
                        <v-btn @click="showDialogToDeleteContacts([item])"
                               class="indigo lighten-4 deep-purple--text text--darken-1 font-weight-black"
                               elevation="3"
                               block>
                            <v-icon class="mdi mdi-delete"></v-icon>
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

                    <template v-slot:progress>
                        <v-progress-linear :height="10"
                                           color="deep-purple dark-5"
                                           indeterminate>
                        </v-progress-linear>
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
                                    <v-btn :disabled="disabledDownloadButton"
                                           @click="downloadExcelFile"
                                           elevation="5"
                                           small
                                           class="indigo lighten-4 deep-purple--text text--darken-1 font-weight-black mb-3 mx-10 mx-sm-0">
                                        <v-icon class="mdi mdi-download"></v-icon>
                                    </v-btn>

                                    <v-badge :content="contactsCountToDelete"
                                             :value="contactsCountToDelete"
                                             color="deep-purple darken-1"
                                             overlap
                                             bottom
                                             class="mx-10 mx-sm-0">
                                        <v-btn :disabled="disabledDeleteButton"
                                               @click="showDialogToDeleteContacts(selectedContacts)"
                                               elevation="5"
                                               block
                                               small
                                               class="indigo lighten-4 deep-purple--text text--darken-1 font-weight-black">
                                            <v-icon class="mdi mdi-delete"></v-icon>
                                        </v-btn>
                                    </v-badge>
                                </v-col>

                                <v-col cols="12" sm="10" class="d-flex align-center my-4">
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

                        <v-dialog v-model="modalDialogForDeletingContacts"
                                  scrollable
                                  persistent
                                  max-width="350px">
                            <v-card color="indigo lighten-5">
                                <v-card-title class="text-center deep-purple--text text--darken-1 font-weight-bold pa-1">The following contacts will be deleted:</v-card-title>

                                <v-divider></v-divider>

                                <v-chip-group class="px-2"
                                              column>
                                    <v-chip v-for="contact in contactsToDelete"
                                            :key="contact.serialNumber"
                                            :ripple="false"
                                            class="font-weight-bold"
                                            color="deep-purple accent-3"
                                            text-color="deep-purple accent-3"
                                            outlined
                                            small>
                                        {{contact.lastName + " " + contact.firstName}}
                                    </v-chip>
                                </v-chip-group>

                                <v-divider></v-divider>

                                <v-card-actions class="d-flex justify-space-around">
                                    <v-btn @click="undeleteContacts"
                                           class="indigo lighten-4 deep-purple--text text--darken-1 font-weight-black"
                                           small
                                           width="75">
                                        <v-icon class="mdi mdi-window-close"></v-icon>
                                    </v-btn>

                                    <v-btn @click="deleteContacts"
                                           class="indigo lighten-4 deep-purple--text text--darken-1 font-weight-black"
                                           small
                                           width="75">
                                        <v-icon class="mdi mdi-check"></v-icon>
                                    </v-btn>
                                </v-card-actions>
                            </v-card>
                        </v-dialog>
                    </template>

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

                                <v-col cols="6" sm="2" class="d-flex align-center">
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
                contactsToDelete: [],

                headers: [
                    {
                        text: "#",
                        value: "serialNumber",
                        align: "center",
                        sortable: false
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
                        align: "center",
                        sortable: false
                    }
                ],

                contactsCountToDelete: 0,
                disabledDeleteButton: true,
                disabledSearchButton: true,

                enteredFilterText: "",

                modalDialogForDeletingContacts: false
            }
        },

        computed: {
            checkContacts() {
                return this.$store.state.contacts;
            },

            disabledDownloadButton() {
                return this.checkContacts.length === 0;
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
                this.disabledDeleteButton = contacts.length === 0;

                this.contactsCountToDelete = contacts.length;
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
                if (this.enteredFilterText === "") {
                    return;
                }

                this.enteredFilterText = "";

                this.getContacts(this.enteredFilterText, 1, undefined);
            },

            filterContacts() {
                this.getContacts(this.enteredFilterText, 1, undefined);
            },

            showDialogToDeleteContacts(contacts) {
                this.contactsToDelete = contacts;

                this.modalDialogForDeletingContacts = true;
            },

            undeleteContacts() {
                this.modalDialogForDeletingContacts = false;

                this.contactsToDelete = [];
            },

            deleteContacts() {
                this.modalDialogForDeletingContacts = false;

                this.$store.dispatch("deleteContacts", this.contactsToDelete.map(contact => contact.id))
                    .then(() => {
                        if (this.contactsToDelete.length > 1) {
                            this.selectedContacts = [];
                            this.contactsToDelete = [];

                            return;
                        }

                        let contacts = JSON.parse(JSON.stringify(this.selectedContacts));
                        this.selectedContacts = [];

                        this.selectedContacts = contacts
                            .filter(selectedContact => !this.contactsToDelete.find(contactToDelete => contactToDelete.id === selectedContact.id))
                            .map(selectedContact => {
                                if (selectedContact.serialNumber > this.contactsToDelete[0].serialNumber) {
                                    selectedContact.serialNumber = selectedContact.serialNumber - 1;
                                }

                                return selectedContact;
                            });

                        this.contactsToDelete = [];
                    });
            },

            downloadExcelFile() {
                this.$store.dispatch("downloadExcelFile")
                    .then(response => {
                        if (response === null) {
                            this.$store.commit("enableErrorMessage", "The file was not created!");

                            return;
                        }

                        let blob = new Blob([response.data], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
                        let url = window.URL.createObjectURL(blob);

                        let anchor = document.createElement("a");
                        anchor.style.display = "none";
                        anchor.href = url;
                        anchor.setAttribute("download", "PhoneBook.xlsx")
                        document.body.appendChild(anchor);
                        anchor.click();
                        anchor.remove();

                        URL.revokeObjectURL(url);
                    });
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

    .v-dialog__content >>> .v-card__title {
        word-break: break-word;
    }
</style>