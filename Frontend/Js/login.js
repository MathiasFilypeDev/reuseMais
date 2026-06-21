document.getElementById("formLogin").addEventListener("submit", async (e) => {
    e.preventDefault();

    const username = document.getElementById("username").value.trim();
    const password = document.getElementById("password").value.trim();
    const role = document.querySelector('input[name="tipoUsuario"]:checked').value;

    // Validação
    if (!username || !password) {
        showMessage("Preencha todos os campos!", "warning");
        return;
    }

    const btn = e.target.querySelector("button[type='submit']");
    const originalText = btn.innerHTML;
    btn.disabled = true;
    btn.innerHTML = `<span class="spinner-border spinner-border-sm"></span> Entrando...`;

    try {
        const response = await fetch("http://localhost:5069/api/users/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ username, password })
        });

        // Se a resposta não for OK (4xx ou 5xx)
        if (!response.ok) {
            let errorMessage = "Erro ao fazer login. Tente novamente.";

            try {
                // Tenta ler como JSON
                const contentType = response.headers.get("content-type");
                if (contentType && contentType.includes("application/json")) {
                    const error = await response.json();
                    errorMessage = error.message || error.error || errorMessage;
                } else {
                    // Se não for JSON, lê como texto
                    const text = await response.text();
                    errorMessage = text || errorMessage;
                }
            } catch (parseError) {
                console.error("Erro ao processar resposta:", parseError);
            }

            showMessage(errorMessage, "danger");
            return;
        }

        // Parse da resposta bem-sucedida
        const data = await response.json();

        if (!data.token || !data.id) {
            showMessage("Resposta do servidor inválida.", "danger");
            return;
        }

        // ✅ Salvar dados no localStorage
        localStorage.setItem("token", data.token);
        localStorage.setItem("user", JSON.stringify({
            id: data.id,
            nome: data.nome || "",
            email: data.email || ""
        }));
        localStorage.setItem("role", role);

        showMessage("Login realizado com sucesso!", "success");

        // Redirecionar após 1 segundo
        setTimeout(() => {
            window.location.href = role === "admin" ? "admin.html" : "principal.html";
        }, 1000);

    } catch (error) {
        console.error("Erro de conexão:", error);
        showMessage("Erro de conexão com o servidor. Verifique se o servidor está ativo.", "danger");
    } finally {
        btn.disabled = false;
        btn.innerHTML = originalText;
    }
});

function showMessage(message, type) {
    const container = document.createElement("div");
    container.className = `alert alert-${type} alert-dismissible fade show`;
    container.role = "alert";
    container.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;

    const msgArea = document.getElementById("msgArea");
    msgArea.innerHTML = "";
    msgArea.appendChild(container);

    // Auto-remover após 5 segundos
    setTimeout(() => {
        if (container.parentNode) {
            container.remove();
        }
    }, 5000);
}