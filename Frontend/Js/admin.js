function verificarLogin() {
    const token = localStorage.getItem("token");
    if (!token) {
        alert("Você precisa estar logado!");
        window.location.href = "login.html";
    }
}

async function consultarRelatorio(tipo) {
    const token = localStorage.getItem("token");

    const response = await fetch("/api/relatorio?tipo=" + tipo, {
        headers: { "Authorization": "Bearer " + token }
    });

    if (!response.ok) {
        alert("Acesso negado. Apenas admin pode ver relatórios.");
        return;
    }

    const data = await response.json();
    const tbody = document.querySelector("#tabelaRelatorio tbody");
    tbody.innerHTML = "";

    data.forEach(mov => {
        const row = document.createElement("tr");
        row.innerHTML = `
      <td>${mov.id}</td>
      <td>${mov.tipo}</td>
      <td>${mov.itemId}</td>
      <td>${mov.quantidade}</td>
    `;
        tbody.appendChild(row);
    });
}

let grafico;

async function consultarEstatisticas() {
    const token = localStorage.getItem("token");

    const response = await fetch("/api/relatorio/estatisticas", {
        headers: { "Authorization": "Bearer " + token }
    });

    if (!response.ok) {
        alert("Erro ao consultar estatísticas!");
        return;
    }

    const data = await response.json();
    const estatisticas = document.getElementById("estatisticas");
    estatisticas.innerHTML = `Total de Entradas: ${data.totalEntradas}<br>Total de Saídas: ${data.totalSaidas}`;

    const ctx = document.getElementById("graficoMovimentacoes").getContext("2d");
    if (grafico) grafico.destroy();

    grafico = new Chart(ctx, {
        type: "bar",
        data: {
            labels: ["Entradas", "Saídas"],
            datasets: [{
                label: "Quantidade",
                data: [data.totalEntradas, data.totalSaidas],
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
    window.location.href = "login.html";
}
