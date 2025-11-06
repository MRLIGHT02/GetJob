/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./CareerLinker.Mobile/**/*.{razor,html,cshtml,razor.css}",
        "./CareerLinker.Web/**/*.{razor,html,cshtml,razor.css}",
        "./CareerLinker.Client/**/*.{razor,html,cshtml,razor.css}",
        "./CareerLinker.Shared/**/*.{razor,html,cshtml,razor.css}",

        // Optional if you have wwwroot content
        "./wwwroot/**/*.{html,js,css}"
    ],
    theme: {
        extend: {},
    },
    plugins: [],
}
