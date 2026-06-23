document.getElementById('anoAtual').textContent = new Date().getFullYear();
document.getElementById("formRegistro").addEventListener("submit", async (e) => {
    e.preventDefault();

    const nome = document.getElementById("nome").value.trim();
    const email = document.getElementById("email").value.trim();
    const senha = document.getElementById("senha").value;
    const confirmarSenha = document.getElementById("confirmarSenha").value;

    if (!nome || !email || !senha || !confirmarSenha) {
        return showMessage("Preencha todos os campos!", "warning");
    }
    if (senha !== confirmarSenha) {
        return showMessage("As senhas não coincidem!", "danger");
    }
    if (senha.length < 6) {
        return showMessage("Senha deve ter pelo menos 6 caracteres!", "warning");
    }

    const btn = e.target.querySelector("button[type='submit']");
    const originalText = btn.innerHTML;
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
            return showMessage(data.message || "Erro ao cadastrar", "danger");
        }

        showMessage("Cadastro realizado com sucesso! Redirecionando...", "success");

        setTimeout(() => {
            window.location.href = "login.html";
        }, 2000);

    } catch (error) {
        console.error(error);
        showMessage("Erro de conexão com o servidor.", "danger");
    } finally {
        btn.disabled = false;
        btn.innerHTML = originalText;
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
