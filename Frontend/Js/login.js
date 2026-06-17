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
        // Ajuste a porta conforme seu backend ASP.NET Core
        const response = await fetch("http://localhost:5000/api/auth/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ username, password, role })
        });

        if (response.ok) {
            const data = await response.json();
            localStorage.setItem("token", data.token);

            if (role === "admin") {
                window.location.href = "admin.html";
            } else {
                window.location.href = "principal.html";
            }
        } else {
            showMessage("Usuário ou senha inválidos.", "danger");
        }
    } catch (error) {
        showMessage("Erro de conexão com o servidor.", "danger");
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
