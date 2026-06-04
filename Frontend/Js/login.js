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

        // Redireciona conforme o tipo escolhido
        if (tipo === "admin") {
            window.location.href = "admin.html";
        } else {
            window.location.href = "principal.html";
        }
    } else {
        alert("Login inválido!");
    }
});

function handleCredentialResponse(response) {
    const googleToken = response.credential;

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

document.getElementById("formLogin").addEventListener("submit", async (e) => {
    e.preventDefault();
    const email = document.getElementById("loginUser").value;
    const senha = document.getElementById("loginPass").value;
    const tipo = document.querySelector("input[name='tipo']:checked").value;

    // Validação simples de email
    const regexEmail = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!regexEmail.test(email)) {
        alert("Por favor, insira um email válido (ex: nome@dominio.com).");
        return;
    }

    const response = await fetch("http://localhost:5000/api/cadastro/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ Email: email, Senha: senha, Tipo: tipo })
    });

    if (response.ok) {
        const data = await response.json();
        alert(data.message);

        if (tipo === "admin") {
            window.location.href = "admin.html";
        } else {
            window.location.href = "principal.html";
        }
    } else {
        alert("Login inválido!");
    }
});

function validarSenha(senha) {
    const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$/;
    return regex.test(senha);
}

document.getElementById("formCadastro").addEventListener("submit", function (event) {
    const senha = document.getElementById("senha").value;

    if (!validarSenha(senha)) {
        event.preventDefault(); // impede envio
        alert("A senha deve ter pelo menos uma letra maiúscula, uma minúscula, um número e no mínimo 6 caracteres.");
    }
});
