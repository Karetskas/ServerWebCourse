const path = require("path");

module.exports = {
    outputDir: path.resolve(__dirname, "..", "wwwroot"),

    devServer: {
        proxy: {
            "^/api": {
                target: "https://localhost:44361"
            }
        }
    },

    transpileDependencies: [
        'vuetify'
    ]
};
