async function login() {
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;

    const response = await fetch("https://localhost:5001/api/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password })
    });

    if (response.ok) {
        const data = await response.json();
        localStorage.setItem("user", JSON.stringify(data.user));
        alert("Bem-vindo, " + data.user.username);

        if (data.user.role === "admin") {
            window.location.href = "admin.html";
        } else {
            window.location.href = "principal.html";
        }
    } else {
        alert("Usuário ou senha inválidos");
    }
}
