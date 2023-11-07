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
            pageSize: 3
        },

        pagesCount: 0,
        contactsCount: 0,

        toast: {
            enabled: false,
            text: "",
            color: null
        }
    },

    mutations: {
        setToast(state, toast) {
            state.toast.text = toast.text;
            state.toast.color = toast.color;
            state.toast.enabled = toast.enabled;
        },

        setPageNumber(state, value) {
            state.page.pageNumber = value;
        },

        setPageSize(state, value) {
            state.page.pageSize = value;
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
            state.pagesCount = Math.ceil(value / state.page.pageSize);
        },

        setContacts(state, contacts) {
            const contactsList = [];
            let counter = 1 + (state.page.pageNumber - 1) * state.page.pageSize;

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
                        pageSize: page.pageSize
                    }
                })
                .then(response => {
                    commit("setSearchFilterText", page.searchFilterText);
                    commit("setPageNumber", page.pageNumber);
                    commit("setPageSize", page.pageSize);

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
                            pageSize: state.page.pageSize
                        });
                })
                .catch(error => commit("enableErrorMessage", error));
        },

        showToast({ commit }, toast) {
            commit("setToast", toast);
        }
    }
});
