document.getElementById("formRegistro").addEventListener("submit", async (e) => {
    e.preventDefault();

    const nome = document.getElementById("nome").value.trim();
    const email = document.getElementById("email").value.trim();
    const senha = document.getElementById("senha").value;
    const confirmarSenha = document.getElementById("confirmarSenha").value;

    // Validações
    if (!nome || !email || !senha || !confirmarSenha) {
        showMessage("Preencha todos os campos!", "warning");
        return;
    }

    if (senha !== confirmarSenha) {
        showMessage("As senhas não coincidem!", "danger");
        return;
    }

    if (senha.length < 6) {
        showMessage("Senha deve ter pelo menos 6 caracteres!", "warning");
        return;
    }

    const btn = e.target.querySelector("button[type='submit']");
    btn.disabled = true;
    btn.innerHTML = `<span class="spinner-border spinner-border-sm"></span> Cadastrando...`;

    try {
        const response = await fetch("http://localhost:5069/api/users/register", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ nome, email, senha })
        });

        const data = await response.json();

        if (!response.ok) {
            showMessage(data.message || "Erro ao cadastrar", "danger");
            return;
        }

        showMessage("Cadastro realizado com sucesso! Redirecionando...", "success");

        setTimeout(() => {
            window.location.href = "login.html";
        }, 2000);

    } catch (error) {
        showMessage("Erro de conexão com o servidor.", "danger");
        console.error(error);
    } finally {
        btn.disabled = false;
        btn.innerHTML = "Cadastrar";
    }
});

function showMessage(message, type) {
    const container = document.createElement("div");
    container.className = `alert alert-${type} text-center`;
    container.textContent = message;

    const msgArea = document.getElementById("msgArea");
    msgArea.innerHTML = "";
    msgArea.appendChild(container);

    setTimeout(() => container.remove(), 4000);
}