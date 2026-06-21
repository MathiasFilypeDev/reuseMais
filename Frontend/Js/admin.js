// ✅ Verificar se é admin
function verificarLogin() {
    const user = JSON.parse(localStorage.getItem("user") || "{}");

    if (user.role !== "admin") {
        alert("Acesso negado!");
        window.location.href = "login.html";
    }

    carregarProdutos();
}

// ✅ Função auxiliar para requisições com token
async function fetchComToken(url, options = {}) {
    const token = localStorage.getItem("token");

    const headers = {
        "Content-Type": "application/json",
        ...options.headers
    };

    if (token) {
        headers["Authorization"] = `Bearer ${token}`;
    }

    return fetch(url, {
        ...options,
        headers
    });
}

// ✅ CARREGAR PRODUTOS
async function carregarProdutos() {
    try {
        const response = await fetchComToken("http://localhost:5069/api/produtos");

        if (!response.ok) throw new Error("Erro ao carregar produtos");

        const produtos = await response.json();
        exibirProdutosNaTabela(produtos);
    } catch (error) {
        console.error("Erro:", error);
        showMessage("Erro ao carregar produtos", "danger");
    }
}

// ✅ EXIBIR PRODUTOS
function exibirProdutosNaTabela(produtos) {
    const tbody = document.querySelector("#tabelaProdutos tbody");
    tbody.innerHTML = "";

    if (produtos.length === 0) {
        tbody.innerHTML = "<tr><td colspan='7' class='text-center'>Nenhum produto encontrado</td></tr>";
        return;
    }

    produtos.forEach(produto => {
        const data = new Date(produto.dataCriacao).toLocaleDateString("pt-BR");
        const tr = document.createElement("tr");
        tr.innerHTML = `
            <td>${produto.id}</td>
            <td>${produto.nome}</td>
            <td>${produto.categoria}</td>
            <td>${produto.quantidade}</td>
            <td><strong>${produto.criadoPorNome}</strong></td>
            <td>${data}</td>
            <td>
                <button class="btn btn-sm btn-danger" onclick="deletarProduto(${produto.id})">🗑️ Deletar</button>
            </td>
        `;
        tbody.appendChild(tr);
    });
}

// ✅ DELETAR PRODUTO
async function deletarProduto(id) {
    if (!confirm("Tem certeza que deseja deletar este produto?")) return;

    try {
        const response = await fetchComToken(`http://localhost:5069/api/produtos/${id}`, {
            method: "DELETE"
        });

        if (!response.ok) throw new Error("Erro ao deletar produto");

        showMessage("Produto deletado!", "success");
        carregarProdutos();
    } catch (error) {
        showMessage("Erro ao deletar produto", "danger");
    }
}

// ✅ LOGOUT
function logout() {
    localStorage.clear();
    window.location.href = "login.html";
}

// ✅ MENSAGENS
function showMessage(message, type) {
    const container = document.createElement("div");
    container.className = `alert alert-${type} text-center`;
    container.textContent = message;

    const alertContainer = document.createElement("div");
    alertContainer.style.position = "fixed";
    alertContainer.style.top = "80px";
    alertContainer.style.right = "20px";
    alertContainer.style.zIndex = "1000";
    alertContainer.appendChild(container);
    document.body.appendChild(alertContainer);

    setTimeout(() => alertContainer.remove(), 3000);
}