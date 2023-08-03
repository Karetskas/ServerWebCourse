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
                <tr v-for="(contactDto, i) in contacts" :key="contactDto.id">
                    <td v-text="i + 1"></td>
                    <td v-text="contactDto.LastName"></td>
                    <td v-text="contactDto.FirstName"></td>
                    <td v-text="contactDto.SecondName"></td>
                    <td>
                        <div v-for="phoneNumber in contactDto.phoneNumbers" :key="phoneNumber.id">
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
