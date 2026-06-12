document.getElementById("formLogin").addEventListener("submit", (event) => {
    event.preventDefault();

    const usuario = document.getElementById("loginUser").value;
    const senha = document.getElementById("loginPass").value;

    // Admin padrão
    if (usuario === "Admin" && senha === "ReuseMaisAdmin") {
        alert("Login de administrador realizado com sucesso!");
        window.location.href = "principal.html";
        return;
    }

    alert("Usuário ou senha inválidos.");
});

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
function validarEmail(email) {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(email);
}

function validarSenha(senha) {
    const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$/;
    return regex.test(senha);
}

function validarEmail(email) {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(email);
}

function validarSenha(senha) {
    const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$/;
    return regex.test(senha);
}

function adicionarValidacaoTempoReal() {
    const emailInput = document.getElementById("email");
    const senhaInput = document.getElementById("senha");
    const emailError = document.getElementById("emailError");
    const senhaError = document.getElementById("senhaError");

    emailInput.addEventListener("input", () => {
        if (!validarEmail(emailInput.value)) {
            emailError.style.display = "block";
            emailError.textContent = "O email deve ser o exemplo nome@example.com";
        } else {
            emailError.style.display = "none";
        }
    });

    senhaInput.addEventListener("input", () => {
        if (!validarSenha(senhaInput.value)) {
            senhaError.style.display = "block";
            senhaError.textContent = "A senha deve conter pelo menos 6 caracteres, incluindo uma letra maiúscula, uma minúscula e um número.";
        } else {
            senhaError.style.display = "none";
        }
    });

    document.getElementById("formCadastro").addEventListener("submit", (event) => {
        if (!validarEmail(emailInput.value) || !validarSenha(senhaInput.value)) {
            event.preventDefault(); // impede envio se inválido
        }
    });
}

// Chama a função ao carregar a página
window.onload = adicionarValidacaoTempoReal;

