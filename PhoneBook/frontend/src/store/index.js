import Vue from "vue";
import Vuex from "vuex";

Vue.use(Vuex);

import axios from "axios";

export default new Vuex.Store({
    state: {
        isLoading: false,
        contacts: [],
        errorMessage: {
            enabled: false,
            message: ""
        }
    },

    getters: {
    },

    mutations: {
        setIsLoading(state, value) {
            state.isLoading = value;
        },

        setContacts(state, contacts) {
            let contactsList = [];
            let counter = 1;

            for (let i = 0; i < contacts.length; i++) {
                contactsList.push({
                    serialNumber: counter++,
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
        loadContacts({ commit }) {
            commit("setIsLoading", true);

            return axios.get("/api/PhoneBook/GetContacts")
                .then(response => {
                    commit("setContacts", response.data);
                })
                .catch((error) => commit("enableErrorMessage", error))
                .finally(() => commit("setIsLoading", false));
        },

        addContact({ commit }, contact) {
            return axios.post("/api/PhoneBook/AddContact", contact)
                .then((resolve) => resolve.data)
                .catch((error) => commit("enableErrorMessage", error));
        }
    },

    modules: {

    }
});
