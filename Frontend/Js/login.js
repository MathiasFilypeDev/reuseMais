document.addEventListener("DOMContentLoaded", () => {
    const usuarioInput = document.getElementById("loginUser");
    const senhaInput = document.getElementById("loginPass");
    const formLogin = document.getElementById("formLogin");
    const erroDiv = document.getElementById("loginError");

    function tipoSelecionado() {
        return document.querySelector("input[name='tipo']:checked").value;
    }

    function mostrarErro(msg) {
        erroDiv.textContent = msg;
        erroDiv.classList.remove("d-none");
    }

    async function realizarLogin() {
        const usuario = usuarioInput.value.trim();
        const senha = senhaInput.value.trim();
        const tipo = tipoSelecionado();

        if (!usuario || !senha) {
            mostrarErro("Preencha todos os campos.");
            return;
        }

        try {
            const response = await fetch("http://localhost:5000/api/auth/login", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ Email: usuario, Senha: senha, Tipo: tipo })
            });

            if (response.ok) {
                const data = await response.json();
                localStorage.setItem("token", data.token);

                console.log("Role recebido:", data.role);

                if (data.role && data.role.toLowerCase() === "admin") {
                    window.location.assign("admin.html"); // garante redirecionamento
                } else {
                    window.location.assign("principal.html");
                }
            } else {
                mostrarErro("Credenciais inválidas. Tente novamente.");
            }
        } catch (error) {
            mostrarErro("Erro de conexão com o servidor.");
            console.error("Erro no login:", error);
        }
    }

    // Clique no botão de login
    formLogin.addEventListener("submit", (event) => {
        event.preventDefault();
        realizarLogin();
    });

    // Pressionar Enter também dispara login
    formLogin.addEventListener("keydown", (event) => {
        if (event.key === "Enter") {
            event.preventDefault();
            realizarLogin();
        }
    });
});

