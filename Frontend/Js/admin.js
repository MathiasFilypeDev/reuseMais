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
    tbody.innerHTML = ""; // limpa tabela

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

function validarCamposLogin(email, senha) {
    if (!email || !senha) {
        alert("Preencha email e senha!");
        return false;
    }
    return true;
}

function mostrarMensagem(msg, tipo = "info") {
    const div = document.createElement("div");
    div.textContent = msg;
    div.style.color = tipo === "erro" ? "red" : "green";
    document.body.appendChild(div);
}

if (response.ok) {
    mostrarMensagem("Item cadastrado com sucesso!", "info");
} else {
    mostrarMensagem("Erro ao cadastrar item!", "erro");
}

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
    document.getElementById("estatisticas").innerHTML =
        `Total de Entradas: ${data.totalEntradas}<br>
     Total de Saídas: ${data.totalSaidas}`;
}

let grafico; // variável global para armazenar o gráfico

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

    // Atualiza texto
    document.getElementById("estatisticas").innerHTML =
        `Total de Entradas: ${data.totalEntradas}<br>
     Total de Saídas: ${data.totalSaidas}`;

    // Atualiza gráfico
    const ctx = document.getElementById("graficoMovimentacoes").getContext("2d");

    if (grafico) {
        grafico.destroy(); // remove gráfico anterior
    }

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

function handleCredentialResponse(response) {
    // Token JWT do Google
    const googleToken = response.credential;

    // Enviar para o backend para trocar por JWT interno
    fetch("/api/externalauth/google", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ token: googleToken })
    })
        .then(res => res.json())
        .then(data => {
            localStorage.setItem("token", data.jwt);
            alert("Login com Google realizado!");
        });
}
