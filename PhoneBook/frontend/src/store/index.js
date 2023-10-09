import Vue from "vue";
import Vuex from "vuex";

Vue.use(Vuex);

import axios from "axios";

export default new Vuex.Store({
    state: {
        errorMessage: {
            enabled: false,
            message: ""
        },

        isLoading: false,
        contacts: [],

        searchFilterText: "",

        page: {
            pageNumber: 1,
            rowsCount: 3
        },

        pagesCount: 0,
        contactsCount: 0
    },

    mutations: {
        setPageNumber(state, value) {
            state.page.pageNumber = value;
        },

        setRowsCount(state, value) {
            state.page.rowsCount = value;
        },

        setContactsCount(state, value) {
            state.contactsCount = value;
        },

        setSearchFilterText(state, text) {
            state.searchFilterText = text;
        },

        setIsLoading(state, value) {
            state.isLoading = value;
        },

        setPagesCount(state, value) {
            state.pagesCount = Math.ceil(value / state.page.rowsCount);
        },

        setContacts(state, contacts) {
            let contactsList = [];
            let counter = 1 + (state.page.pageNumber - 1) * state.page.rowsCount;

            for (let i = 0; i < contacts.length; i++) {
                contactsList.push({
                    serialNumber: counter++,
                    id: contacts[i].id,
                    lastName: contacts[i].lastName,
                    firstName: contacts[i].firstName,
                    phoneNumbers: contacts[i].phoneNumbers
                });
            }

            state.contacts = contactsList;
        },

        enableErrorMessage(state, value) {
            state.errorMessage.enabled = true;
            state.errorMessage.message = value;
        },

        disableErrorMessage(state) {
            state.errorMessage.enabled = false;
            state.errorMessage.message = "";
        }
    },

    actions: {
        loadContacts({ dispatch, commit }, page) {
            commit("setIsLoading", true);

            return axios.get("/api/PhoneBook/GetContacts",
                {
                    params: {
                        searchFilterText: page.searchFilterText,
                        pageNumber: page.pageNumber,
                        rowsCount: page.rowsCount
                    }
                })
                .then(response => {
                    commit("setSearchFilterText", page.searchFilterText);
                    commit("setPageNumber", page.pageNumber);
                    commit("setRowsCount", page.rowsCount);

                    dispatch("getContactsCount");

                    commit("setContacts", response.data);
                })
                .catch(error => commit("enableErrorMessage", error))
                .finally(() => commit("setIsLoading", false));
        },

        addContact({ commit }, contact) {
            return axios.post("/api/PhoneBook/AddContact", contact)
                .then(resolve => resolve.data)
                .catch(error => commit("enableErrorMessage", error));
        },

        getContactsCount({ state, commit }) {
            return axios.get("/api/PhoneBook/GetContactsCount",
                {
                    params: {
                        searchFilterText: state.searchFilterText
                    }
                })
                .then(resolve => {
                    commit("setContactsCount", resolve.data);
                    commit("setPagesCount", resolve.data);
                })
                .catch(error => commit("enableErrorMessage", error));
        },

        deleteContacts({ state, dispatch, commit }, contactsId) {
            return axios.post("/api/PhoneBook/deleteContacts", contactsId)
                .then(() => {
                    dispatch("loadContacts",
                        {
                            searchFilterText: state.searchFilterText,
                            pageNumber: 1,
                            rowsCount: state.page.rowsCount
                        });
                })
                .catch(error => commit("enableErrorMessage", error));
        },

        downloadExcelFile({ state, commit }) {
            return axios.get("/api/PhoneBook/downloadExcelFile",
                {
                    responseType: "blob",

                    params: {
                        searchFilterText: state.searchFilterText
                    }
                })
                .then(resolve => resolve)
                .catch(error => commit("enableErrorMessage", error));
        }
    }
});
