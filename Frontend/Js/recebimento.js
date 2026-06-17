function verificarLogin() {
    const token = localStorage.getItem("token");
    if (!token) {
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

    const response = await fetch("/api/relatorio", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify({ tipo, itemId, quantidade })
    });

    if (response.ok) {
        const data = await response.json();
        alert(data.message);
        document.getElementById("formMovimentacao").reset();
    } else {
        alert("Erro ao registrar movimentação. Verifique se você tem permissão.");
    }
});

function logout() {
    localStorage.removeItem("token");
    window.location.href = "login.html";
}
