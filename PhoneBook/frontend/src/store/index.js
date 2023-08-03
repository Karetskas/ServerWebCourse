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
            state.contacts = contacts;
        }
    },

    actions: {
        loadContacts({ comit }) {
            comit("setIsLoading", true);

            return axios.get("/api/PhoneBook/GetContacts")
                .then(response => {
                    comit("setContacts", response.data);
                })
                .catch(() => {
                    alert("Не удалось загрузить контакты");
                })
                .then(() => {
                    comit("setIsLoading", false);
                });
        }
    },

    modules: {

    }
});
