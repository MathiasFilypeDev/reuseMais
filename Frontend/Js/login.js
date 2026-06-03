document.getElementById("formLogin").addEventListener("submit", async (e) => {
    e.preventDefault();
    const email = document.getElementById("loginUser").value;
    const senha = document.getElementById("loginPass").value;
    const tipo = document.querySelector("input[name='tipo']:checked").value;

    const response = await fetch("http://localhost:5000/api/cadastro/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ Email: email, Senha: senha, Tipo: tipo })
    });

    if (response.ok) {
        const data = await response.json();
        alert(data.message);
        if (data.tipo === "admin") {
            window.location.href = "admin.html";
        } else {
            window.location.href = "principal.html";
        }
    } else {
        alert("Login inválido!");
    }
});

async function login(email, senha) {
    if (!validarCamposLogin(email, senha)) return;
}

function handleCredentialResponse(response) {
    // Token JWT do Google
    const googleToken = response.credential;

    // Enviar para o backend para trocar por JWT interno
    fetch("/api/externalauth/google", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ token: googleToken })
    })
        .then(res => res.json())
        .then(data => {
            localStorage.setItem("token", data.jwt);
            alert("Login com Google realizado!");
        });
}
