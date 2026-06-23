function verificarLogin() {
    const user = JSON.parse(localStorage.getItem("user") || "{}");
    const token = localStorage.getItem("token");

    if (!user.id || !token) {
        alert("Você precisa estar logado!");
        window.location.href = "login.html";
    }
}

document.getElementById("formMovimentacao").addEventListener("submit", async (e) => {
    e.preventDefault();

    const tipo = document.getElementById("tipo").value;
    const itemId = document.getElementById("itemId").value;
    const quantidade = document.getElementById("quantidade").value;
    const token = localStorage.getItem("token");

    try {
        const response = await fetch("http://localhost:5069/api/movimentacoes", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify({ tipo, itemId, quantidade })
        });

        if (!response.ok) throw new Error();

        alert("Movimentação registrada com sucesso!");
        document.getElementById("formMovimentacao").reset();
        await carregarMovimentacoes();
    } catch {
        alert("Erro ao registrar movimentação. Verifique se você tem permissão.");
    }
});

async function carregarMovimentacoes() {
    const token = localStorage.getItem("token");
    const response = await fetch("http://localhost:5069/api/movimentacoes", {
        headers: { "Authorization": `Bearer ${token}` }
    });

    if (response.ok) {
        const data = await response.json();
        const lista = document.getElementById("listaMovimentacoes");
        lista.innerHTML = "";
        data.forEach(mov => {
            const li = document.createElement("li");
            li.textContent = `Mov ${mov.id} - ${mov.tipo} - Item ${mov.itemId} - Qtd: ${mov.quantidade}`;
            lista.appendChild(li);
        });
    }
}

function logout() {
    localStorage.clear();
    window.location.href = "login.html";
}
