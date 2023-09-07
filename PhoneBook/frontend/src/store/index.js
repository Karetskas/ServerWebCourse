import Vue from "vue";
import Vuex from "vuex";

Vue.use(Vuex);

import axios from "axios";

export default new Vuex.Store({
    state: {
        isLoading: false,
        contacts: []
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
        }
    },

    actions: {
        loadContacts({ commit }) {
            commit("setIsLoading", true);

            return axios.get("/api/PhoneBook/GetContacts")
                .then(response => {
                    commit("setContacts", response.data);
                })
                .catch(() => {
                    alert("Failed to load contacts!");
                })
                .then(() => {
                    commit("setIsLoading", false);
                });
        }
    },

    modules: {

    }
});
