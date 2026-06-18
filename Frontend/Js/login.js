document.getElementById("formLogin").addEventListener("submit", async (e) => {
    e.preventDefault();

    const username = document.getElementById("username").value.trim();
    const password = document.getElementById("password").value.trim();
    const role = document.querySelector('input[name="tipoUsuario"]:checked').value;

    // Validação simples
    if (!username || !password) {
        showMessage("Preencha todos os campos!", "warning");
        return;
    }

    // Spinner no botão
    const btn = e.target.querySelector("button[type='submit']");
    btn.disabled = true;
    btn.innerHTML = `<span class="spinner-border spinner-border-sm"></span> Entrando...`;

    try {
        const response = await fetch("http://localhost:5000/api/auth/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ username, password, role })
        });

        console.log("Status da resposta:", response.status);
        console.log("Headers:", response.headers);

        if (response.ok) {
            const data = await response.json();
            console.log("Token recebido:", data.token ? "✅ Sim" : "❌ Não");

            localStorage.setItem("token", data.token);

            if (role === "admin") {
                window.location.href = "admin.html";
            } else {
                window.location.href = "principal.html";
            }
        } else {
            // ✅ MELHORADO: Mostrar erro detalhado do servidor
            let errorMessage = "Usuário ou senha inválidos.";
            try {
                const errorData = await response.json();
                errorMessage = errorData.message || errorMessage;
                console.error("Erro do servidor:", errorData);
            } catch {
                console.error("Status de erro:", response.status);
            }
            showMessage(errorMessage, "danger");
        }
    } catch (error) {
        console.error("Erro de conexão completo:", error);
        showMessage(`Erro de conexão com o servidor: ${error.message}`, "danger");
    } finally {
        btn.disabled = false;
        btn.innerHTML = "Entrar";
    }
});

// Função para mostrar mensagens Bootstrap
function showMessage(message, type) {
    const container = document.createElement("div");
    container.className = `alert alert-${type} text-center`;
    container.textContent = message;

    const msgArea = document.getElementById("msgArea");
    msgArea.innerHTML = ""; // limpa mensagens anteriores
    msgArea.appendChild(container);

    // Remove após 3s
    setTimeout(() => container.remove(), 3000);
}