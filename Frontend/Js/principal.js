function verificarLogin() {
    const user = JSON.parse(localStorage.getItem("user") || "{}");
    const token = localStorage.getItem("token");

    if (!user.id || !token) {
        alert("Você precisa estar logado!");
        window.location.href = "login.html";
        return;
    }

    if (user.role === "admin") {
        // Admin deve ir para admin.html
        window.location.href = "admin.html";
    }
}

window.onload = () => {
    verificarLogin();
    carregarUsuarios();
    carregarItens();
    carregarMovimentacoes();
};

const API_URL = "http://localhost:5069/api";

async function fetchComToken(endpoint) {
    const token = localStorage.getItem("token");
    return fetch(`${API_URL}${endpoint}`, {
        headers: { "Authorization": `Bearer ${token}` }
    });
}

async function carregarUsuarios() {
    const res = await fetchComToken("/users");
    const data = await res.json();
    const lista = document.getElementById("listaUsuarios");
    lista.innerHTML = "";
    data.forEach(u => {
        const li = document.createElement("li");
        li.textContent = `${u.id} - ${u.nome} (${u.email})`;
        lista.appendChild(li);
    });
}

async function carregarItens() {
    const res = await fetchComToken("/items");
    const data = await res.json();
    const lista = document.getElementById("listaItens");
    lista.innerHTML = "";
    data.forEach(i => {
        const li = document.createElement("li");
        li.textContent = `${i.id} - ${i.nome} [${i.categoria}]`;
        lista.appendChild(li);
    });
}

async function carregarMovimentacoes() {
    const res = await fetchComToken("/movimentacoes");
    const data = await res.json();
    const lista = document.getElementById("listaMovimentacoes");
    lista.innerHTML = "";
    data.forEach(m => {
        const li = document.createElement("li");
        li.textContent = `Mov ${m.id} - User ${m.userId} - Item ${m.itemId} - ${m.data}`;
        lista.appendChild(li);
    });
}
