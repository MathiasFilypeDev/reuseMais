document.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById("formCadastro");
    const nomeInput = document.getElementById("cadUser");
    const emailInput = document.getElementById("cadEmail");
    const senhaInput = document.getElementById("cadSenha");
    const confirmaSenhaInput = document.getElementById("cadConfirmaSenha");
    const msgCadastro = document.getElementById("msgCadastro");

    if (!form) return;

    function validarEmail(email) {
        const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return regex.test(email);
    }

    function validarSenha(senha) {
        const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$/;
        return regex.test(senha);
    }

    function mostrarMensagem(text, tipo = "info") {
        if (!msgCadastro) {
            alert(text);
            return;
        }

        msgCadastro.textContent = text;
        msgCadastro.className = tipo === "erro" ? "text-danger" : "text-success";
    }

    form.addEventListener("submit", async (e) => {
        e.preventDefault();
        const nome = nomeInput?.value.trim() || "";
        const email = emailInput?.value.trim() || "";
        const senha = senhaInput?.value || "";
        const confirmaSenha = confirmaSenhaInput?.value || "";
        const tipo = document.querySelector("input[name='tipo']:checked")?.value || "usuario";

        if (!nome || !email || !senha || !confirmaSenha) {
            mostrarMensagem("Preencha todos os campos.", "erro");
            return;
        }

        if (!validarEmail(email)) {
            mostrarMensagem("Informe um email válido.", "erro");
            return;
        }

        if (!validarSenha(senha)) {
            mostrarMensagem("A senha deve ter pelo menos 6 caracteres, uma letra maiúscula, uma letra minúscula e um número.", "erro");
            return;
        }

        if (senha !== confirmaSenha) {
            mostrarMensagem("As senhas não coincidem.", "erro");
            return;
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
                    return response;
                } catch (err) {
                    console.warn(`Falha ao conectar ${baseUrl}${path}:`, err);
                    lastError = err;
                }
            }
            throw lastError || new Error("Não foi possível conectar a nenhum backend disponível.");
        }

        try {
            const response = await apiFetch("/api/auth/register", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ Nome: nome, Email: email, Senha: senha })
            });

            if (response.ok) {
                const data = await response.json();
                mostrarMensagem(data?.message || "Cadastro realizado com sucesso!", "info");
                form.reset();
                setTimeout(() => {
                    window.location.href = "login.html";
                }, 800);
                return;
            }

            const errorData = await response.json().catch(() => null);
            mostrarMensagem(errorData?.message || "Erro ao cadastrar.", "erro");
        } catch (error) {
            console.error(error);
            mostrarMensagem("Erro de conexão com o servidor. Verifique se o backend está rodando em http://localhost:5000 ou https://localhost:5001.", "erro");
        }
    });
});
