<template>
    <v-container>
        <v-row>
            <v-col>
                <h1 class="text-center deep-purple--text text--darken-1">Add contact</h1>
            </v-col>
        </v-row>

        <v-row>
            <v-col class="pa-0">
                <v-text-field outlined
                              class="text-field-border-color text-field-color font-weight-bold"
                              label="First name"
                              placeholder="Enter first name..."
                              maxlength="255"
                              counter="255"
                              :rules="rulesForTextField">
                </v-text-field>
            </v-col>
        </v-row>

        <v-row>
            <v-col class="pa-0">
                <v-text-field outlined
                              class="text-field-border-color text-field-color font-weight-bold"
                              label="Last name"
                              placeholder="Enter last name..."
                              maxlength="255"
                              counter="255"
                              :rules="rulesForTextField">
                </v-text-field>
            </v-col>
        </v-row>

        <v-row>
            <v-col class="pa-0">
                <v-text-field outlined
                              class="text-field-border-color text-field-color font-weight-bold"
                              label="Phone number"
                              placeholder="Enter phone number..."
                              maxlength="255"
                              counter="255"
                              :rules="[...rulesForPhoneNumber, ...rulesForTextField]">
                </v-text-field>
            </v-col>
        </v-row>

        <v-row>
            <v-col class="text-center pa-0">
                <v-btn class="indigo lighten-4 deep-purple--text text--darken-1 font-weight-black"
                       elevation="5"
                       large>
                    Add contact
                </v-btn>
            </v-col>
        </v-row>
    </v-container>
</template>

<script>
    export default {
        data: () => ({
            rulesForTextField: [
                value => !value?.startsWith(" ") || "Space at the beginning of the line!",
                value => !value?.endsWith(" ") || "Space at the end of the line!"
            ],
            rulesForPhoneNumber: [
                value => !(!/[0-9]+/.test(value) && value?.length !== 0) || "The phone number must contain at least 1 digit!",
                value => !(/[^0-9-+.() ]/.test(value)) || "A phone can contain: (, ), +, -, numbers, dots and spaces!"
            ]
        })
    }
</script>

<style scoped>
    .text-field-border-color >>> fieldset {
        border-color: mediumpurple;
    }

    .text-field-color >>> .v-text-field__slot input {
        color: rebeccapurple;
    }
</style>