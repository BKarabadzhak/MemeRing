function login(username, password) {
    if (!username) {
        username = document.getElementById("username").value;
    }
    if (!password) {
        password = document.getElementById("password").value;
    }

    const userForLogin = { "Username": username, "Password": password };

    $.ajax({
        method: "POST",
        url: "http://localhost:5000/api/Auth/Login",
        contentType: "application/json",
        data: JSON.stringify(userForLogin),
        crossDomain: true,
        xhrFields: {
            withCredentials: false
        },
        success: function (response) {
            const token = {
                "Token": response.token,
                "Items": []
            };
            
            window.localStorage.setItem('Token', token.Token);
            window.localStorage.setItem('CurrentUserId', response.user.id);
            window.localStorage.setItem('CurrentUserName', response.user.username);
            window.location.href = "HomePage.html";
        },
        error: function (response) {
            notify(response.responseJSON.message);
        }
    });

    document.getElementById("loginForm").reset();
}

function register() {
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;
    const confirmPassword = document.getElementById("confirmPassword").value;

    const userForRegister = { "Username": username, "Password": password, "ConfirmPassword": confirmPassword }

    $.ajax({
        method: "POST",
        url: "http://localhost:5000/api/Auth/Register",
        contentType: "application/json",
        data: JSON.stringify(userForRegister),
        success: function (response) {
            login(username, password);
        },
        error: function (response) {
            notify(response.responseJSON.message);
        }
    });

    document.getElementById("loginForm").reset();
}