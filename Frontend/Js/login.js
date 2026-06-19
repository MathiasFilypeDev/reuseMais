document.getElementById("formLogin").addEventListener("submit", async (e) => {
    e.preventDefault();

    const username = document.getElementById("username").value.trim();
    const password = document.getElementById("password").value.trim();
    const role = document.querySelector('input[name="tipoUsuario"]:checked').value;

    if (!username || !password) {
        showMessage("Preencha todos os campos!", "warning");
        return;
    }

    const btn = e.target.querySelector("button[type='submit']");
    btn.disabled = true;
    btn.innerHTML = `<span class="spinner-border spinner-border-sm"></span> Entrando...`;

    try {
        const response = await fetch("http://localhost:5069/api/users/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ username, password })
        });

        if (!response.ok) {
            const error = await response.json();
            showMessage(error.message || "Usuário ou senha inválidos.", "danger");
            return;
        }

        const data = await response.json();

        // ✅ Salvar o JWT Token
        localStorage.setItem("token", data.token);
        localStorage.setItem("user", JSON.stringify({
            id: data.id,
            nome: data.nome,
            email: data.email
        }));
        localStorage.setItem("role", role);

        showMessage("Login realizado com sucesso!", "success");

        setTimeout(() => {
            if (role === "admin") {
                window.location.href = "admin.html";
            } else {
                window.location.href = "principal.html";
            }
        }, 1000);

    } catch (error) {
        showMessage("Erro de conexão com o servidor.", "danger");
        console.error(error);
    } finally {
        btn.disabled = false;
        btn.innerHTML = "Entrar";
    }
});

function showMessage(message, type) {
    const container = document.createElement("div");
    container.className = `alert alert-${type} text-center`;
    container.textContent = message;

    const msgArea = document.getElementById("msgArea");
    msgArea.innerHTML = "";
    msgArea.appendChild(container);

    setTimeout(() => container.remove(), 3000);
}