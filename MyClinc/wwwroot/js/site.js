
document.addEventListener("DOMContentLoaded", function () {
    // Initialize phone input for consultation booking
    var phoneInput = document.getElementById("phoneNumber");
    if (phoneInput) {
        var iti = window.intlTelInput(phoneInput, {
            initialCountry: "auto",
            geoIpLookup: function (callback) {
                fetch("https://ipapi.co/json")
                    .then(function (res) { return res.json(); })
                    .then(function (data) { callback(data.country_code); })
                    .catch(function () { callback("us"); });
            },
            utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/18.2.1/js/utils.js"
        });

        phoneInput.addEventListener("change", function() {
            if (iti.isValidNumber()) {
                document.getElementById("fullPhoneNumber").value = iti.getNumber();
                document.getElementById("phoneError").style.display = "none";
            } else {
                document.getElementById("fullPhoneNumber").value = "";
                document.getElementById("phoneError").textContent = "رقم الهاتف غير صالح.";
                document.getElementById("phoneError").style.display = "block";
            }
        });

        phoneInput.addEventListener("keyup", function() {
            if (iti.isValidNumber()) {
                document.getElementById("fullPhoneNumber").value = iti.getNumber();
                document.getElementById("phoneError").style.display = "none";
            } else {
                document.getElementById("fullPhoneNumber").value = "";
                document.getElementById("phoneError").textContent = "رقم الهاتف غير صالح.";
                document.getElementById("phoneError").style.display = "block";
            }
        });

        // Set initial value if already present
        if (phoneInput.value) {
            if (iti.isValidNumber()) {
                document.getElementById("fullPhoneNumber").value = iti.getNumber();
            }
        }
    }

    // Initialize phone input for doctor join request
    var phoneInputDoctor = document.getElementById("phoneNumberDoctor");
    if (phoneInputDoctor) {
        var itiDoctor = window.intlTelInput(phoneInputDoctor, {
            initialCountry: "auto",
            geoIpLookup: function (callback) {
                fetch("https://ipapi.co/json")
                    .then(function (res) { return res.json(); })
                    .then(function (data) { callback(data.country_code); })
                    .catch(function () { callback("us"); });
            },
            utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/18.2.1/js/utils.js"
        });

        phoneInputDoctor.addEventListener("change", function() {
            if (itiDoctor.isValidNumber()) {
                document.getElementById("fullPhoneNumberDoctor").value = itiDoctor.getNumber();
                document.getElementById("phoneErrorDoctor").style.display = "none";
            } else {
                document.getElementById("fullPhoneNumberDoctor").value = "";
                document.getElementById("phoneErrorDoctor").textContent = "رقم الهاتف غير صالح.";
                document.getElementById("phoneErrorDoctor").style.display = "block";
            }
        });

        phoneInputDoctor.addEventListener("keyup", function() {
            if (itiDoctor.isValidNumber()) {
                document.getElementById("fullPhoneNumberDoctor").value = itiDoctor.getNumber();
                document.getElementById("phoneErrorDoctor").style.display = "none";
            } else {
                document.getElementById("fullPhoneNumberDoctor").value = "";
                document.getElementById("phoneErrorDoctor").textContent = "رقم الهاتف غير صالح.";
                document.getElementById("phoneErrorDoctor").style.display = "block";
            }
        });

        // Set initial value if already present
        if (phoneInputDoctor.value) {
            if (itiDoctor.isValidNumber()) {
                document.getElementById("fullPhoneNumberDoctor").value = itiDoctor.getNumber();
            }
        }
    }

    // Initialize phone input for partnership
    var phoneInputPartnership = document.getElementById("phoneNumberPartnership");
    if (phoneInputPartnership) {
        var itiPartnership = window.intlTelInput(phoneInputPartnership, {
            initialCountry: "auto",
            geoIpLookup: function (callback) {
                fetch("https://ipapi.co/json")
                    .then(function (res) { return res.json(); })
                    .catch(function () { callback("us"); });
            },
            utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/18.2.1/js/utils.js"
        });

        phoneInputPartnership.addEventListener("change", function() {
            if (itiPartnership.isValidNumber()) {
                document.getElementById("fullPhoneNumberPartnership").value = itiPartnership.getNumber();
                document.getElementById("phoneErrorPartnership").style.display = "none";
            } else {
                document.getElementById("fullPhoneNumberPartnership").value = "";
                document.getElementById("phoneErrorPartnership").textContent = "رقم الهاتف غير صالح.";
                document.getElementById("phoneErrorPartnership").style.display = "block";
            }
        });

        phoneInputPartnership.addEventListener("keyup", function() {
            if (itiPartnership.isValidNumber()) {
                document.getElementById("fullPhoneNumberPartnership").value = itiPartnership.getNumber();
                document.getElementById("phoneErrorPartnership").style.display = "none";
            } else {
                document.getElementById("fullPhoneNumberPartnership").value = "";
                document.getElementById("phoneErrorPartnership").textContent = "رقم الهاتف غير صالح.";
                document.getElementById("phoneErrorPartnership").style.display = "block";
            }
        });

        // Set initial value if already present
        if (phoneInputPartnership.value) {
            if (itiPartnership.isValidNumber()) {
                document.getElementById("fullPhoneNumberPartnership").value = itiPartnership.getNumber();
            }
        }
    }
});


