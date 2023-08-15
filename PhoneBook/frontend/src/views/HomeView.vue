<template>
    <div class="home">
        <img alt="Vue logo" src="../assets/logo.png">

        <table>
            <thead>
                <tr>
                    <th>№</th>
                    <th>Фамилия</th>
                    <th>Имя</th>
                    <th>Отчество</th>
                    <th>Телефон</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(contact, i) in contacts" :key="contact.id">
                    <td v-text="i + 1"></td>
                    <td v-text="contact.lastName"></td>
                    <td v-text="contact.firstName"></td>
                    <td v-text="contact.secondName"></td>
                    <td>
                        <div v-for="phoneNumber in contact.phoneNumbers" :key="phoneNumber.id">
                            {{ phoneNumber.phone }}
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
    export default {
        created() {
            this.$store.dispatch("loadContacts");
        },

        computed: {
            contacts() {
                return this.$store.state.contacts;
            }
        }
    }
</script>
