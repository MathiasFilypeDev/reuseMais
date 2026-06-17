function verificarLogin() {
    const token = localStorage.getItem("token");
    if (!token) {
        alert("Você precisa estar logado!");
        window.location.href = "login.html";
    }
}

document.getElementById("formItem").addEventListener("submit", async (e) => {
    e.preventDefault();

    const nome = document.getElementById("nome").value;
    const descricao = document.getElementById("descricao").value;
    const quantidade = document.getElementById("quantidade").value;

    const token = localStorage.getItem("token");

    const response = await fetch("/api/item", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify({ nome, descricao, quantidade })
    });

    if (response.ok) {
        const data = await response.json();
        alert("Item cadastrado com sucesso!");
        carregarItens();
        document.getElementById("formItem").reset();
    } else {
        alert("Erro ao cadastrar item. Verifique se você tem permissão.");
    }
});

async function carregarItens() {
    const token = localStorage.getItem("token");

    const response = await fetch("/api/item", {
        headers: { "Authorization": "Bearer " + token }
    });

    if (!response.ok) {
        alert("Erro ao carregar itens.");
        return;
    }

    const data = await response.json();
    const tbody = document.querySelector("#tabelaItens tbody");
    tbody.innerHTML = "";

    data.forEach(item => {
        const row = document.createElement("tr");
        row.innerHTML = `
      <td>${item.id}</td>
      <td>${item.nome}</td>
      <td>${item.descricao}</td>
      <td>${item.quantidade}</td>
    `;
        tbody.appendChild(row);
    });
}

function logout() {
    localStorage.removeItem("token");
    window.location.href = "login.html";
}

// Carregar itens ao abrir a página
window.onload = carregarItens;
