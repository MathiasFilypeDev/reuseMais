function verificarLogin() {
    const user = JSON.parse(localStorage.getItem("user") || "{}");
    const token = localStorage.getItem("token");

    if (!user.id || !token) {
        alert("Você precisa estar logado para cadastrar itens!");
        window.location.href = "login.html";
    }
}

async function fetchComToken(endpoint, options = {}) {
    const token = localStorage.getItem("token");
    const headers = {
        "Content-Type": "application/json",
        ...options.headers,
        "Authorization": `Bearer ${token}`
    };
    return fetch(`${API_URL}${endpoint}`, { ...options, headers });
}

document.getElementById("formCadastroItem").addEventListener("submit", async (e) => {
    e.preventDefault();

    const nome = document.getElementById("nome").value.trim();
    const descricao = document.getElementById("descricao").value.trim();
    const categoria = document.getElementById("categoria").value.trim();
    const quantidade = document.getElementById("quantidade").value;
    const valor = document.getElementById("valor").value;
    const local = document.getElementById("local").value.trim();
    const prazo = document.getElementById("prazo").value;

    if (!nome || !descricao || !categoria || !quantidade || !valor || !local) {
        return showMessage("Preencha todos os campos obrigatórios!", "warning");
    }

    const btn = e.target.querySelector("button[type='submit']");
    const originalText = btn.innerHTML;
    btn.disabled = true;
    btn.innerHTML = `<span class="spinner-border spinner-border-sm"></span> Cadastrando...`;

    try {
        const response = await fetchComToken("/items", {
            method: "POST",
            body: JSON.stringify({ nome, descricao, categoria, quantidade, valor, local, prazo })
        });

        const data = await response.json();

        if (!response.ok) {
            return showMessage(data.message || "Erro ao cadastrar item", "danger");
        }

        showMessage("Item cadastrado com sucesso! 🎉", "success");
        document.getElementById("formCadastroItem").reset();

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

// ✅ Executa verificação ao carregar
window.onload = verificarLogin;
