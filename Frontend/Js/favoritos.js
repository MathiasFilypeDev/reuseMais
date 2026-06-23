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

async function carregarFavoritos() {
    try {
        const res = await fetchComToken("/favoritos");
        if (!res.ok) throw new Error("Erro ao carregar favoritos");
        const data = await res.json();

        const lista = document.getElementById("listaFavoritos");
        lista.innerHTML = "";

        if (!data.length) {
            lista.innerHTML = "<li class='list-group-item text-center'>Nenhum favorito encontrado</li>";
            return;
        }

        data.forEach(fav => {
            const li = document.createElement("li");
            li.className = "list-group-item";
            li.textContent = `${fav.itemId} - ${fav.nomeItem}`;
            lista.appendChild(li);
        });
    } catch (error) {
        showMessage("Erro ao carregar favoritos", "danger");
    }
}

window.onload = () => {
    verificarLogin();
    carregarFavoritos();
};
