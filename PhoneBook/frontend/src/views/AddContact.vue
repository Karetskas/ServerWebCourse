<template>
    <v-container fill-height
                 fluid
                 class="pa-0">
        <v-row>
            <v-col>
                <h1 class="text-center deep-purple--text text--darken-1">Add contact</h1>
            </v-col>
        </v-row>

        <v-row class="d-flex justify-center">
            <v-col class="py-0 col-sm-8 col-11">
                <v-badge :content="charactersInFirstNameCount"
                         :color="colorFirstName"
                         overlap
                         offset-x="62"
                         offset-y="58"
                         class="d-flex"
                         value="true">
                    <v-text-field v-model="firstNameTextField.text"
                                  :rules="rulesForTextField"
                                  :error-messages="firstNameTextField.errorMessage"
                                  outlined
                                  dense
                                  class="text-field-border-color text-field-color font-weight-black"
                                  label="First name"
                                  placeholder="Enter first name..."
                                  color="deep-purple darken-5"
                                  background-color="indigo lighten-5"
                                  maxlength="255">
                        <template v-slot:message="{ message }">
                            <div class="text-subtitle-2 font-weight-bold text-center fixed-width-message">
                                {{message}}
                            </div>
                        </template>
                    </v-text-field>
                </v-badge>
            </v-col>
        </v-row>

        <v-row class="d-flex justify-center">
            <v-col class="py-0 col-sm-8 col-11">
                <v-badge :content="charactersInLastNameCount"
                         :color="colorLastName"
                         overlap
                         offset-x="62"
                         offset-y="58"
                         class="d-flex"
                         value="true">
                    <v-text-field v-model="lastNameTextField.text"
                                  :rules="rulesForTextField"
                                  :error-messages="lastNameTextField.errorMessage"
                                  outlined
                                  dense
                                  class="text-field-border-color text-field-color font-weight-black"
                                  label="Last name"
                                  placeholder="Enter last name..."
                                  color="deep-purple darken-5"
                                  background-color="indigo lighten-5"
                                  maxlength="255">
                        <template v-slot:message="{ message }">
                            <div class="text-subtitle-2 font-weight-bold text-center fixed-width-message">
                                {{message}}
                            </div>
                        </template>
                    </v-text-field>
                </v-badge>
            </v-col>
        </v-row>

        <v-row class="d-flex justify-center">
            <v-col class="py-0 col-sm-8 col-11">
                <v-badge :content="charactersInHomePhoneCount"
                         :color="colorHomePhone"
                         overlap
                         offset-x="62"
                         offset-y="58"
                         class="d-flex badge-for-phone-number"
                         value="true">
                    <v-checkbox v-model="homePhoneTextField.disabled"
                                :disabled="isDisabledCheckBoxHomePhone"
                                class="pt-0 pr-1 mt-3"
                                color="deep-purple accent-3">
                    </v-checkbox>

                    <v-text-field v-model="homePhoneTextField.phone"
                                  :rules="[...rulesForPhoneNumber, ...rulesForTextField]"
                                  :error-messages="homePhoneTextField.errorMessage"
                                  :disabled="!homePhoneTextField.disabled"
                                  outlined
                                  dense
                                  class="text-field-border-color text-field-color font-weight-black"
                                  label="Home phone"
                                  placeholder="Enter your home phone number..."
                                  color="deep-purple darken-5"
                                  background-color="indigo lighten-5"
                                  maxlength="255">
                        <template v-slot:message="{ message }">
                            <div class="text-subtitle-2 font-weight-bold text-center fixed-width-message">
                                {{message}}
                            </div>
                        </template>
                    </v-text-field>
                </v-badge>
            </v-col>
        </v-row>

        <v-row class="d-flex justify-center">
            <v-col class="py-0 col-sm-8 col-11">
                <v-badge :content="charactersInWorkPhoneCount"
                         :color="colorWorkPhone"
                         overlap
                         offset-x="62"
                         offset-y="58"
                         class="d-flex badge-for-phone-number"
                         value="true">
                    <v-checkbox v-model="workPhoneTextField.disabled"
                                :disabled="isDisabledCheckBoxWorkPhone"
                                class="pt-0 pr-1 mt-3"
                                color="deep-purple accent-3">
                    </v-checkbox>

                    <v-text-field v-model="workPhoneTextField.phone"
                                  :rules="[...rulesForPhoneNumber, ...rulesForTextField]"
                                  :error-messages="workPhoneTextField.errorMessage"
                                  :disabled="!workPhoneTextField.disabled"
                                  outlined
                                  dense
                                  class="text-field-border-color text-field-color font-weight-black"
                                  label="Work phone"
                                  placeholder="Enter your work phone number..."
                                  color="deep-purple darken-5"
                                  background-color="indigo lighten-5"
                                  maxlength="255">
                        <template v-slot:message="{ message }">
                            <div class="text-subtitle-2 font-weight-bold text-center fixed-width-message">
                                {{message}}
                            </div>
                        </template>
                    </v-text-field>
                </v-badge>
            </v-col>
        </v-row>

        <v-row class="d-flex justify-center">
            <v-col class="py-0 col-sm-8 col-11">
                <v-badge :content="charactersInMobilePhoneCount"
                         :color="colorMobilePhone"
                         overlap
                         offset-x="62"
                         offset-y="58"
                         class="d-flex badge-for-phone-number"
                         value="true">
                    <v-checkbox v-model="mobilePhoneTextField.disabled"
                                :disabled="isDisabledCheckBoxMobilePhone"
                                class="pt-0 pr-1 mt-3"
                                color="deep-purple accent-3">
                    </v-checkbox>

                    <v-text-field v-model="mobilePhoneTextField.phone"
                                  :rules="[...rulesForPhoneNumber, ...rulesForTextField]"
                                  :error-messages="mobilePhoneTextField.errorMessage"
                                  :disabled="!mobilePhoneTextField.disabled"
                                  outlined
                                  dense
                                  class="text-field-border-color text-field-color font-weight-black"
                                  label="Mobile phone"
                                  placeholder="Enter your mobile phone number..."
                                  color="deep-purple darken-5"
                                  background-color="indigo lighten-5"
                                  maxlength="255">
                        <template v-slot:message="{ message }">
                            <div class="text-subtitle-2 font-weight-bold text-center fixed-width-message">
                                {{message}}
                            </div>
                        </template>
                    </v-text-field>
                </v-badge>
            </v-col>
        </v-row>

        <v-row>
            <v-col class="text-center py-0 mb-5">
                <v-btn :disabled="areInvalidContactFields"
                       @click="addContact"
                       class="indigo lighten-4 deep-purple--text text--darken-1 font-weight-black"
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
        data() {
            return {
                onePercent255: 100 / 255,

                firstNameTextField: {
                    text: "",
                    errorMessage: ""
                },

                lastNameTextField: {
                    text: "",
                    errorMessage: ""
                },

                rulesForTextField: [
                    value => !value?.startsWith(" ") || "Space at the beginning of the line!",
                    value => !value?.endsWith(" ") || "Space at the end of the line!"
                ],
                rulesForPhoneNumber: [
                    value => !(/[^0-9-+.() ]/.test(value)) || "Allowed characters: (, ), +, -, 0-9, dots and spaces!",
                    value => !(!/[0-9]+/.test(value) && value?.length !== 0) || "The phone number must contain at least 1 digit!"
                ],

                homePhoneTextField: {
                    phone: "",
                    phoneType: "Home",
                    disabled: true,
                    errorMessage: ""
                },

                workPhoneTextField: {
                    phone: "",
                    phoneType: "Work",
                    disabled: false,
                    errorMessage: ""
                },

                mobilePhoneTextField: {
                    phone: "",
                    phoneType: "Mobile",
                    disabled: false,
                    errorMessage: ""
                },
            }
        },

        computed: {
            charactersInFirstNameCount() {
                return this.firstNameTextField.text.length + " / 255";
            },

            charactersInLastNameCount() {
                return this.lastNameTextField.text.length + " / 255";
            },

            charactersInHomePhoneCount() {
                return this.homePhoneTextField.phone.length + " / 255";
            },

            charactersInWorkPhoneCount() {
                return this.workPhoneTextField.phone.length + " / 255";
            },

            charactersInMobilePhoneCount() {
                return this.mobilePhoneTextField.phone.length + " / 255";
            },

            progressFirstName() {
                return this.getProgress(this.firstNameTextField.text.length);
            },

            progressLastName() {
                return this.getProgress(this.lastNameTextField.text.length);
            },

            progressHomePhone() {
                return this.getProgress(this.homePhoneTextField.phone.length);
            },

            progressWorkPhone() {
                return this.getProgress(this.workPhoneTextField.phone.length);
            },

            progressMobilePhone() {
                return this.getProgress(this.mobilePhoneTextField.phone.length);
            },

            colorFirstName() {
                return this.getColor(this.firstNameTextField.text.length);
            },

            colorLastName() {
                return this.getColor(this.lastNameTextField.text.length);
            },

            isDisabledCheckBoxHomePhone() {
                return this.countActiveTextFields === 1 && this.homePhoneTextField.disabled;
            },

            isDisabledCheckBoxWorkPhone() {
                return this.countActiveTextFields === 1 && this.workPhoneTextField.disabled;
            },

            isDisabledCheckBoxMobilePhone() {
                return this.countActiveTextFields === 1 && this.mobilePhoneTextField.disabled;
            },

            colorHomePhone() {
                return !this.homePhoneTextField.disabled ? "grey darken-1" : this.getColor(this.homePhoneTextField.phone.length);
            },

            colorWorkPhone() {
                return !this.workPhoneTextField.disabled ? "grey darken-1" : this.getColor(this.workPhoneTextField.phone.length);
            },

            colorMobilePhone() {
                return !this.mobilePhoneTextField.disabled ? "grey darken-1" : this.getColor(this.mobilePhoneTextField.phone.length);
            },

            countActiveTextFields() {
                return this.homePhoneTextField.disabled + this.workPhoneTextField.disabled + this.mobilePhoneTextField.disabled;
            },

            areInvalidContactFields() {
                const fieldsContentArray = [
                    this.firstNameTextField.text,
                    this.lastNameTextField.text,
                    this.homePhoneTextField.phone,
                    this.workPhoneTextField.phone,
                    this.mobilePhoneTextField.phone
                ];

                return this.firstNameTextField.text === ""
                    || this.firstNameTextField.errorMessage.length > 0
                    || this.lastNameTextField.text === ""
                    || this.lastNameTextField.errorMessage.length > 0
                    || (this.homePhoneTextField.phone === "" && this.homePhoneTextField.disabled)
                    || this.homePhoneTextField.errorMessage.length > 0
                    || (this.workPhoneTextField.phone === "" && this.workPhoneTextField.disabled)
                    || this.workPhoneTextField.errorMessage.length > 0
                    || (this.mobilePhoneTextField.phone === "" && this.mobilePhoneTextField.disabled)
                    || this.mobilePhoneTextField.errorMessage.length > 0
                    || this.isValidatedText(this.rulesForTextField, fieldsContentArray)
                    || this.isValidatedText(this.rulesForPhoneNumber, fieldsContentArray.slice(2));
            }
        },

        watch: {
            "homePhoneTextField.disabled"(value) {
                if (!value) {
                    this.homePhoneTextField.phone = "";
                    this.homePhoneTextField.errorMessage = "";
                }
            },

            "workPhoneTextField.disabled"(value) {
                if (!value) {
                    this.workPhoneTextField.phone = "";
                    this.workPhoneTextField.errorMessage = "";
                }
            },

            "mobilePhoneTextField.disabled"(value) {
                if (!value) {
                    this.mobilePhoneTextField.phone = "";
                    this.mobilePhoneTextField.errorMessage = "";
                }
            },

            "firstNameTextField.text"() {
                this.firstNameTextField.errorMessage = "";
            },

            "lastNameTextField.text"() {
                this.lastNameTextField.errorMessage = "";
            },

            "homePhoneTextField.phone"() {
                this.homePhoneTextField.errorMessage = "";
            },

            "workPhoneTextField.phone"() {
                this.workPhoneTextField.errorMessage = "";
            },

            "mobilePhoneTextField.phone"() {
                this.mobilePhoneTextField.errorMessage = "";
            }
        },

        methods: {
            getProgress(value) {
                return Math.min(100, value * this.onePercent255);
            },

            getColor(value) {
                const maxColorComponentValue = 255;
                const redComponent = 98;
                const redComponentDivider = redComponent - maxColorComponentValue;
                const red = this.getComponentColor(redComponent, redComponentDivider, value);

                const greenComponent = 37;
                const green = this.getComponentColor(greenComponent, greenComponent, value);

                const blueComponent = 178;
                const blue = this.getComponentColor(blueComponent, blueComponent, value);

                return ["rgb(" + red + ", " + green + ", " + blue + ", 1)"][0];
            },

            getComponentColor(initialColor, colorDivider, value) {
                const maxColorComponentValue = 255;

                return Math.floor(initialColor - (colorDivider / maxColorComponentValue) * value);
            },

            isValidatedText(arrayRules, fieldsContentArray) {
                let isValidatedText = false;

                for (let i = 0; i < arrayRules.length; i++) {
                    for (let j = 0; j < fieldsContentArray.length; j++) {
                        isValidatedText = arrayRules[i](fieldsContentArray[j]) !== true;

                        if (isValidatedText) {
                            return isValidatedText;
                        }
                    }

                    if (isValidatedText) {
                        return isValidatedText;
                    }
                }

                return isValidatedText;
            },

            addContact() {
                const contact = {
                    lastName: this.lastNameTextField.text,
                    firstName: this.firstNameTextField.text,
                    phoneNumbers: []
                };

                const phoneNumbers = [this.homePhoneTextField, this.workPhoneTextField, this.mobilePhoneTextField];

                for (let i = 0; i < phoneNumbers.length; i++) {
                    if (phoneNumbers[i].disabled === true) {
                        contact.phoneNumbers.push({
                            phoneType: phoneNumbers[i].phoneType,
                            phone: phoneNumbers[i].phone
                        })
                    }
                }

                this.$store.dispatch("addContact", contact)
                    .then(errorMessages => {
                        let isError = false;
                        for (let i = 0; i < errorMessages.length; i++) {
                            if (errorMessages[i].message.length > 0) {
                                this.showErrorMessage(errorMessages[i]);

                                isError = true;
                            }
                        }

                        if (!isError) {
                            this.firstNameTextField.text = "";
                            this.lastNameTextField.text = "";
                            this.homePhoneTextField.phone = "";
                            this.homePhoneTextField.disabled = true;
                            this.workPhoneTextField.phone = "";
                            this.workPhoneTextField.disabled = false;
                            this.mobilePhoneTextField.phone = "";
                            this.mobilePhoneTextField.disabled = false;

                            this.$store.dispatch("showToast", {
                                enabled: true,
                                text: "Contact has been added.",
                                color: "green lighten-1"
                            });
                        }
                    })
                    .catch(reject => {
                        this.$store.commit("enableErrorMessage", reject);
                    });
            },

            showErrorMessage(textField) {
                const textFieldType = {
                    firstName: "FirstName",
                    lastName: "LastName",
                    homePhone: "HomePhone",
                    workPhone: "WorkPhone",
                    mobilePhone: "MobilePhone"
                };

                switch (textField.fieldType) {
                    case textFieldType.firstName:
                        this.firstNameTextField.errorMessage = textField.message;
                        break;
                    case textFieldType.lastName:
                        this.lastNameTextField.errorMessage = textField.message;
                        break;
                    case textFieldType.homePhone:
                        this.homePhoneTextField.errorMessage = textField.message;
                        break;
                    case textFieldType.workPhone:
                        this.workPhoneTextField.errorMessage = textField.message;
                        break;
                    default:
                        this.mobilePhoneTextField.errorMessage = textField.message;
                        break;
                }

                this.$store.dispatch("showToast", {
                    enabled: true,
                    text: "Contact not added.",
                    color: "red lighten-1"
                });
            }
        }
    }
</script>

<style scoped>
    .text-field-color >>> .v-text-field__slot input {
        color: rebeccapurple;
    }

    .v-input >>> .v-text-field__details {
        display: flex;
        align-items: center;
        height: 22px;
        padding: 0px;
    }

    .badge-for-phone-number .v-input >>> .v-text-field__details {
        min-height: 40px;
    }

    .v-badge >>> .v-badge__badge {
        width: 63px;
    }
</style>