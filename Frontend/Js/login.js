document.addEventListener("DOMContentLoaded", () => {
    const usuarioInput = document.getElementById("loginUser");
    const senhaInput = document.getElementById("loginPass");
    const emailError = document.getElementById("emailError");
    const senhaError = document.getElementById("senhaError");
    const formLogin = document.getElementById("formLogin");
    const erroDiv = document.getElementById("loginError");

    function tipoSelecionado() {
        return document.querySelector("input[name='tipo']:checked").value;
    }

    function mostrarErro(msg) {
        erroDiv.textContent = msg;
        erroDiv.classList.remove("d-none");
    }

    function validarUsuario(valor) {
        const regexEmail = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        const regexUsuario = /^[a-zA-Z0-9]{3,}$/;
        return regexEmail.test(valor) || regexUsuario.test(valor);
    }

    function validarSenha(senha) {
        const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$/;
        return regex.test(senha);
    }

    async function realizarLogin() {
        const usuario = usuarioInput.value;
        const senha = senhaInput.value;
        const tipo = tipoSelecionado();

        const response = await fetch("http://localhost:5000/api/auth/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ Email: usuario, Senha: senha, Tipo: tipo })
        });

        if (response.ok) {
            const data = await response.json();
            localStorage.setItem("token", data.token);

            if (data.role === "admin") {
                window.location.href = "admin.html";
            } else {
                window.location.href = "principal.html";
            }
        } else {
            mostrarErro("Credenciais inválidas. Tente novamente.");
        }
    }

    formLogin.addEventListener("submit", async (event) => {
        event.preventDefault();
        realizarLogin();
    });
});
