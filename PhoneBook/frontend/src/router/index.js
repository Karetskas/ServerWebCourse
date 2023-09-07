import Vue from "vue"
import VueRouter from "vue-router"

Vue.use(VueRouter);

const routes = [
    {
        path: "/",
        name: "home",
        component: () => import("../views/HomeView.vue")
    },
    {
        path: "/AddContact",
        name: "addingContact",
        component: () => import("../views/AddContact.vue")
    },
    {
        path: "/ViewContacts",
        name: "viewContacts",
        component: () => import("../views/ViewContacts.vue")
    }
];

const router = new VueRouter({
    mode: "history",
    base: process.env.BASE_URL,
    routes
});

export default router
