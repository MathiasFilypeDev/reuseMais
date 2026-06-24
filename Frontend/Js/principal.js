function verificarLogin() {
    const user = JSON.parse(localStorage.getItem("user") || "{}");
    if (!user.id) {
        // Se não há login, redireciona para login.html
        window.location.href = "login.html";
    }
}

function logout() {
    // Limpa dados de sessão e volta para login
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    window.location.href = "login.html";
}

// Função utilitária para calcular prazo de 30 dias
function calcularPrazo() {
    const hoje = new Date();
    const prazo = new Date(hoje);
    prazo.setDate(hoje.getDate() + 30);
    return prazo.toLocaleDateString("pt-BR");
}

// Itens disponíveis (estoque geral)
const itensDisponiveis = [
    { id: 1, nome: "Camisa Reuse+", descricao: "Camisa básica sustentável, feita com algodão reciclado.", quantidade: 10, imagem: "assets/camisa.png", prazo: calcularPrazo() },
    { id: 2, nome: "Telha Plástica Reuse+", descricao: "Telha ecológica produzida a partir de plástico reciclado.", quantidade: 25, imagem: "assets/telhaP.png", prazo: calcularPrazo() }
];

// Meus itens (cadastrados pelo usuário)
let meusItens = [
    { id: 101, nome: "Livro Reaproveitado", descricao: "Livro usado em bom estado.", quantidade: 3, imagem: "assets/logo-reuseMais.png", prazo: calcularPrazo() }
];

function carregarTabelaDisponiveis() {
    const tbody = document.querySelector("#tabelaItensDisponiveis tbody");
    tbody.innerHTML = "";
    itensDisponiveis.forEach(i => {
        tbody.innerHTML += `
      <tr>
        <td>${i.id}</td>
        <td><img src="${i.imagem}" alt="${i.nome}" width="60"></td>
        <td>${i.nome}</td>
        <td>${i.descricao}</td>
        <td>${i.quantidade}</td>
        <td>${i.prazo}</td>
      </tr>`;
    });
}

function carregarTabelaMeusItens() {
    const tbody = document.querySelector("#tabelaMeusItens tbody");
    tbody.innerHTML = "";
    meusItens.forEach(i => {
        tbody.innerHTML += `
      <tr>
        <td>${i.id}</td>
        <td><img src="${i.imagem}" alt="${i.nome}" width="60"></td>
        <td>${i.nome}</td>
        <td>${i.descricao}</td>
        <td>${i.quantidade}</td>
        <td>${i.prazo}</td>
      </tr>`;
    });
}

// Adicionar novo item via modal
document.addEventListener("DOMContentLoaded", () => {
    document.getElementById("formAddItem").addEventListener("submit", (e) => {
        e.preventDefault();
        const novoId = meusItens.length ? meusItens[meusItens.length - 1].id + 1 : 1;
        const nome = document.getElementById("nomeItem").value;
        const descricao = document.getElementById("descricaoItem").value;
        const quantidade = document.getElementById("quantidadeItem").value;
        const categoria = document.getElementById("categoriaItem").value;
        const prazo = calcularPrazo();

        // Se o usuário anexar uma foto, usar como imagem
        const inputFile = document.getElementById("imagemItem");
        let imagem = "assets/logo-reuseMais.png"; // padrão
        if (inputFile.files.length > 0) {
            imagem = URL.createObjectURL(inputFile.files[0]); // gera URL temporária
        }

        meusItens.push({ id: novoId, nome, descricao, quantidade, imagem, prazo, categoria });
        carregarTabelaMeusItens();

        // Fechar modal e resetar formulário
        const modal = bootstrap.Modal.getInstance(document.getElementById("modalAddItem"));
        modal.hide();
        e.target.reset();
    });

    // Botão "+"
    document.getElementById("btnAddItem").addEventListener("click", () => {
        const modal = new bootstrap.Modal(document.getElementById("modalAddItem"));
        modal.show();
    });

    // Inicialização
    verificarLogin();
    carregarTabelaDisponiveis();
    carregarTabelaMeusItens();
    document.getElementById("anoAtual").textContent = new Date().getFullYear();
});
