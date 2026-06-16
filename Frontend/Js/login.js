document.addEventListener("DOMContentLoaded", () => {
    const usuarioInput = document.getElementById("loginUser");
    const senhaInput = document.getElementById("loginPass");
    const formLogin = document.getElementById("formLogin");
    const erroDiv = document.getElementById("loginError");
    let btnLogin = document.getElementById("btnLogin");
    if (!btnLogin) btnLogin = formLogin.querySelector("button[type=submit]");

    function tipoSelecionado() {
        const tipo = document.querySelector("input[name='tipo']:checked");
        return tipo ? tipo.value : null;
    }

    function mostrarErro(msg) {
        erroDiv.textContent = msg;
        erroDiv.classList.remove("d-none");
    }

    function limparErro() {
        erroDiv.classList.add("d-none");
        erroDiv.textContent = "";
    }

    const apiBaseUrls = [
        "http://localhost:5000",
        "https://localhost:5001"
    ];

    async function apiFetch(path, options) {
        let lastError = null;
        for (const baseUrl of apiBaseUrls) {
            try {
                const response = await fetch(`${baseUrl}${path}`, options);
                return { response, url: `${baseUrl}${path}` };
            } catch (err) {
                console.warn(`Falha ao conectar ${baseUrl}${path}:`, err);
                lastError = err;
            }
        }
        throw lastError || new Error("Não foi possível conectar a nenhum backend disponível.");
    }

    async function realizarLogin() {
        const usuario = usuarioInput.value.trim();
        const senha = senhaInput.value.trim();
        const tipo = tipoSelecionado();

        if (!usuario || !senha || !tipo) {
            mostrarErro("Preencha todos os campos e selecione o tipo.");
            return;
        }

        if (senha.length < 6) {
            mostrarErro("A senha deve ter pelo menos 6 caracteres.");
            return;
        }

        if (btnLogin) {
            btnLogin.disabled = true;
            btnLogin.textContent = "Entrando...";
        }

        try {
            const { response, url } = await apiFetch("/api/auth/login", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ Email: usuario, Senha: senha, Tipo: tipo })
            });

            let data = {};
            try {
                data = await response.json();
            } catch (err) {
                data = {};
            }

            console.log("Resposta da API:", data, "URL:", url);
            console.log('HTTP status:', response.status, 'ok:', response.ok);

            if (response.ok) {
                if (data.token) {
                    localStorage.setItem("token", data.token);
                } else {
                    localStorage.setItem("token", "local-fallback");
                }

                const apiRole = (data.role || data.Role || data.tipo || data.Tipo || data.roleName || "").toString();
                if (apiRole && apiRole.toLowerCase() !== (tipo || "").toLowerCase()) {
                    mostrarErro("Tipo de conta não corresponde ao cadastro.");
                    return;
                }

                if ((tipo || "").toLowerCase() === "admin") {
                    window.location.href = "admin.html";
                } else {
                    window.location.href = "principal.html";
                }
            } else {
                let text = null;
                try {
                    text = await response.text();
                } catch (e) {
                    text = null;
                }
                console.warn('Login falhou. status:', response.status, 'body:', text, 'json:', data);
                mostrarErro((data && (data.message || data.error)) || text || `Erro ${response.status}: Não foi possível autenticar.`);
            }
        } catch (error) {
            mostrarErro("Erro de conexão com o servidor. Verifique se o backend está rodando em http://localhost:5000 ou https://localhost:5001.");
            console.error("Erro no login:", error);
        } finally {
            if (btnLogin) {
                btnLogin.disabled = false;
                btnLogin.textContent = "Entrar";
            }
        }
    }

    formLogin.addEventListener("submit", (event) => {
        event.preventDefault();
        limparErro();
        realizarLogin();
    });

    usuarioInput.addEventListener("input", limparErro);
    senhaInput.addEventListener("input", limparErro);
});
