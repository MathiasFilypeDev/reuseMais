function verificarLogin() {
    const user = JSON.parse(localStorage.getItem("user") || "{}");
    const token = localStorage.getItem("token");

    if (!user.id || !token) {
        alert("Você precisa estar logado!");
        window.location.href = "login.html";
    }
}

async function fetchComToken(endpoint) {
    const token = localStorage.getItem("token");
    return fetch(`${API_URL}${endpoint}`, {
        headers: { "Authorization": `Bearer ${token}` }
    });
}

async function carregarMensagens() {
    try {
        const res = await fetchComToken("/mensagens");
        if (!res.ok) throw new Error("Erro ao carregar mensagens");
        const data = await res.json();

        const lista = document.getElementById("listaMensagens");
        lista.innerHTML = "";

        if (!data.length) {
            lista.innerHTML = "<li class='list-group-item text-center'>Nenhuma mensagem encontrada</li>";
            return;
        }

        data.forEach(msg => {
            const li = document.createElement("li");
            li.className = "list-group-item";
            li.textContent = `${msg.id} - ${msg.remetente}: ${msg.conteudo}`;
            lista.appendChild(li);
        });
    } catch (error) {
        showMessage("Erro ao carregar mensagens", "danger");
    }
}

window.onload = () => {
    verificarLogin();
    carregarMensagens();
};
