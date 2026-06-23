function verificarLogin() {
    const user = JSON.parse(localStorage.getItem("user") || "{}");
    const token = localStorage.getItem("token");

    if (!token || user.role !== "admin") {
        alert("Acesso negado! Somente para administradores.");
        window.location.href = "login.html";
        return;
    }
}

// Carregar resumos iniciais
async function carregarResumos() {
    const produtos = await carregarEstatisticas("items", "all");
    const usuarios = await carregarEstatisticas("users", "all");

    document.getElementById("resumoProdutos").textContent = produtos.total || 0;
    document.getElementById("resumoDisponiveis").textContent = produtos.disponiveis || 0;
    document.getElementById("resumoSaidas").textContent = produtos.saidas || 0;
    document.getElementById("resumoUsuarios").textContent = usuarios.logados || 0;
    document.getElementById("resumoUsuariosCadastrados").textContent = usuarios.cadastrados || 0;
}

// Buscar estatísticas
async function carregarEstatisticas(tipo, periodo) {
    try {
        const response = await fetchComToken(`/stats/${tipo}?period=${periodo}`);
        if (!response.ok) throw new Error("Erro ao carregar estatísticas");
        return await response.json();
    } catch {
        return { labels: [], values: [], total: 0, disponiveis: 0, saidas: 0, logados: 0, cadastrados: 0 };
    }
}

// Atualizar gráficos
async function atualizarGraficos() {
    const periodoProdutos = document.getElementById("filtroProdutos").value;
    const periodoUsuarios = document.getElementById("filtroUsuarios").value;

    const dadosProdutos = await carregarEstatisticas("items", periodoProdutos);
    const dadosUsuarios = await carregarEstatisticas("users", periodoUsuarios);

    renderGrafico("graficoProdutos", "Produtos", dadosProdutos);
    renderGrafico("graficoUsuarios", "Usuários", dadosUsuarios);
}

// Renderizar gráfico
function renderGrafico(canvasId, titulo, dados) {
    const ctx = document.getElementById(canvasId).getContext("2d");

    if (window[canvasId]) {
        window[canvasId].destroy();
    }

    window[canvasId] = new Chart(ctx, {
        type: "pie",
        data: {
            labels: dados.labels,
            datasets: [{
                data: dados.values,
                backgroundColor: [
                    "#007bff", "#28a745", "#ffc107", "#dc3545",
                    "#6f42c1", "#17a2b8", "#fd7e14"
                ]
            }]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: titulo
                }
            }
        }
    });
}

// Botões
document.getElementById("abrirModal").addEventListener("click", () => {
    const modal = new bootstrap.Modal(document.getElementById("modalGraficos"));
    modal.show();
    atualizarGraficos();
});

document.getElementById("abrirModalFiltros").addEventListener("click", () => {
    const modal = new bootstrap.Modal(document.getElementById("modalGraficos"));
    modal.show();
});

document.getElementById("btnFiltrar").addEventListener("click", atualizarGraficos);

document.getElementById("btnLogout").addEventListener("click", () => {
    localStorage.clear();
    window.location.href = "login.html";
});

// Inicialização
window.onload = () => {
    verificarLogin();
    carregarResumos();
};
