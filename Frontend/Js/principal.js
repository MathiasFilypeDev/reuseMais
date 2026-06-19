function verificarLogin() {
    const user = JSON.parse(localStorage.getItem("user"));
    if (!user) {
        alert("Você precisa estar logado!");
        window.location.href = "login.html";
    }
}

// Chamamos a verificação logo ao carregar a página
window.onload = () => {
    verificarLogin();
    carregarUsuarios();
    carregarItens();
    carregarMovimentacoes();
};

const API_URL = "http://localhost:5069/api";

async function carregarUsuarios() {
    const res = await fetch(`${API_URL}/users`);
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
    const res = await fetch(`${API_URL}/items`);
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
    const res = await fetch(`${API_URL}/movimentacoes`);
    const data = await res.json();
    const lista = document.getElementById("listaMovimentacoes");
    lista.innerHTML = "";
    data.forEach(m => {
        const li = document.createElement("li");
        li.textContent = `Mov ${m.id} - User ${m.userId} - Item ${m.itemId} - ${m.data}`;
        lista.appendChild(li);
    });
}
