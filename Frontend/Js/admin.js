function verificarLogin() {
    const token = localStorage.getItem("token");
    const role = localStorage.getItem("role");

    if (!token) {
        alert("Você precisa estar logado!");
        window.location.href = "login.html";
    } else if (role !== "admin") {
        alert("Acesso restrito a administradores!");
        window.location.href = "principal.html";
    }
}

async function consultarRelatorio(tipo) {
    const token = localStorage.getItem("token");

    const response = await fetch("http://localhost:5069/api/movimentacoes", {
        headers: { "Authorization": "Bearer " + token }
    });

    if (!response.ok) {
        alert("Acesso negado. Apenas admin pode ver relatórios.");
        return;
    }

    const data = await response.json();
    const tbody = document.querySelector("#tabelaRelatorio tbody");
    tbody.innerHTML = "";

    // filtramos por tipo (entrada/saida) se existir
    const filtrados = data.filter(mov => mov.tipo === tipo);

    filtrados.forEach(mov => {
        const row = document.createElement("tr");
        row.innerHTML = `
            <td>${mov.id}</td>
            <td>${mov.tipo || "N/A"}</td>
            <td>${mov.itemId}</td>
            <td>${mov.quantidade || 1}</td>
        `;
        tbody.appendChild(row);
    });
}

let grafico;

async function consultarEstatisticas() {
    const token = localStorage.getItem("token");

    const response = await fetch("http://localhost:5069/api/movimentacoes", {
        headers: { "Authorization": "Bearer " + token }
    });

    if (!response.ok) {
        alert("Erro ao consultar estatísticas!");
        return;
    }

    const data = await response.json();

    // contamos entradas e saídas
    const totalEntradas = data.filter(m => m.tipo === "entrada").length;
    const totalSaidas = data.filter(m => m.tipo === "saida").length;

    const estatisticas = document.getElementById("estatisticas");
    estatisticas.innerHTML = `Total de Entradas: ${totalEntradas}<br>Total de Saídas: ${totalSaidas}`;

    const ctx = document.getElementById("graficoMovimentacoes").getContext("2d");
    if (grafico) grafico.destroy();

    grafico = new Chart(ctx, {
        type: "bar",
        data: {
            labels: ["Entradas", "Saídas"],
            datasets: [{
                label: "Quantidade",
                data: [totalEntradas, totalSaidas],
                backgroundColor: ["green", "red"]
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: { display: false },
                title: { display: true, text: "Estatísticas de Movimentações" }
            }
        }
    });
}

function logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    window.location.href = "login.html";
}
