document.getElementById("formLogin").addEventListener("submit", async (e) => {
    e.preventDefault();

    const username = document.getElementById("username").value.trim();
    const password = document.getElementById("password").value.trim();
    const roleElement = document.querySelector('input[name="tipoUsuario"]:checked');

    if (!username || !password) {
        return showMessage("Preencha todos os campos!", "warning");
    }
    if (!roleElement) {
        return showMessage("Selecione um tipo de usuário!", "warning");
    }

    const role = roleElement.value;
    const btn = e.target.querySelector("button[type='submit']");
    const originalText = btn.innerHTML;
    btn.disabled = true;
    btn.innerHTML = `<span class="spinner-border spinner-border-sm"></span> Entrando...`;

    try {

        if (username === "User" && password === "user123") {
            const defaultUser = {
                id: 1,
                nome: "Usuário Padrão",
                email: "user@reuse.com",
                role: "user"
            };
            localStorage.setItem("token", "user-token");
            localStorage.setItem("user", JSON.stringify(defaultUser));

            showMessage("Login realizado com sucesso! 🎉", "success");
            setTimeout(() => window.location.href = "principal.html", 1000);
            return;
        }

        // Administrador mock
        if (username === "Admin" && password === "admin123") {
            const adminUser = {
                id: 0,
                nome: "Administrador",
                email: "admin@reuse.com",
                role: "admin"
            };
            localStorage.setItem("token", "admin-token");
            localStorage.setItem("user", JSON.stringify(adminUser));

            showMessage("Login de administrador realizado com sucesso!", "success");
            setTimeout(() => window.location.href = "admin.html", 1000);
            return;
        }

        // Login via API
        const response = await fetch("http://localhost:5069/api/users/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ username, password }),
        });

        const data = await response.json();

        if (!response.ok) {
            return showMessage(data.message || "Usuário ou senha incorretos!", "danger");
        }

        if (!data.token || !data.id) {
            return showMessage("Erro: resposta inválida do servidor.", "danger");
        }

        const userData = {
            id: data.id,
            nome: data.nome || username,
            email: data.email || "",
            role: data.role || role
        };

        localStorage.setItem("token", data.token);
        localStorage.setItem("user", JSON.stringify(userData));

        showMessage("Login realizado com sucesso! 🎉", "success");

        setTimeout(() => {
            if (userData.role === "admin") {
                window.location.href = "admin.html";
            } else {
                window.location.href = "principal.html";
            }
        }, 1000);

    } catch (error) {
        console.error("Erro detalhado:", error);
        showMessage("Erro de conexão com o servidor. Verifique se está ativo.", "danger");
    } finally {
        btn.disabled = false;
        btn.innerHTML = originalText;
    }
});

function showMessage(message, type) {
    const container = document.createElement("div");
    container.className = `alert alert-${type} alert-dismissible fade show mt-2`;
    container.role = "alert";
    container.innerHTML = `
        <strong>${type === "success" ? "✅" : type === "warning" ? "⚠️" : "❌"}</strong> ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;

    const msgArea = document.getElementById("msgArea");
    msgArea.innerHTML = "";
    msgArea.appendChild(container);

    setTimeout(() => {
        if (container.parentNode) {
            container.remove();
        }
    }, 5000);
}
