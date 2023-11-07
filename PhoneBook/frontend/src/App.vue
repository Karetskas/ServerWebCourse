<template>
    <v-app>
        <v-card>
            <v-tabs v-model="tabs"
                    grow
                    show-arrows
                    background-color="indigo lighten-4"
                    color="deep-purple darken-4"
                    slider-size="4">
                <v-tab v-for="item in items"
                       :key="item.tab"
                       :to="item.route"
                       class="font-weight-bold">
                    <v-icon :class="item.icon"
                            class="mr-1"></v-icon>
                    {{item.tab}}
                </v-tab>
            </v-tabs>

            <main class="ma-2">
                <v-alert :value="$store.state.errorMessage.enabled"
                         @input="$store.commit('disableErrorMessage')"
                         shaped
                         dismissible
                         type="error"
                         elevation="10">
                    {{$store.state.errorMessage.message}}
                </v-alert>

                <v-snackbar v-model="$store.state.toast.enabled"
                            :timeout="2000"
                            :color="$store.state.toast.color"
                            elevation="10"
                            centered
                            transition="scale-transition">
                    {{$store.state.toast.text}}
                </v-snackbar>

                <router-view></router-view>
            </main>
        </v-card>
    </v-app>
</template>

<script>
    export default {
        data: () => {
            return {
                tabs: null,
                items: [
                    { tab: "HOME", route: "/", icon: "mdi mdi-home" },
                    { tab: "ADD", route: "/AddContact", icon: "mdi mdi-account-plus" },
                    { tab: "VIEW", route: "/ViewContacts", icon: "mdi mdi-card-account-details" },
                ]
            }
        }
    }
</script>

<style scoped>
    @media(max-width: 600px) {
        .button-font {
            font-size: 11px;
        }
    }

    .v-snack >>> .v-snack__content {
        font-size: 16px;
        font-weight: 700;
        text-align: center;
    }
</style>