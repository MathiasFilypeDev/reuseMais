document.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById("formItens");

    form.addEventListener("submit", (event) => {
        event.preventDefault();

        const novoItem = {
            titulo: form.categoria.value + " - Item novo",
            descricao: form.descricao.value,
            categoria: form.categoria.value,
            disponibilidade: "Imediata",
            prazo: form.prazo.value + " dias",
            localizacao: form.local.value,
            quantidade: form.quantidade.value,
            valor: form.valor.value,
            status: "Sem propostas",
            imagem: "assets/default.png" 
        };

        let itens = JSON.parse(localStorage.getItem("itens")) || [];

        itens.push(novoItem);

        localStorage.setItem("itens", JSON.stringify(itens));

        alert("Item cadastrado com sucesso!");
        form.reset();
    });
});

const btnAddItem = document.getElementById("btn-add-item");
btnAddItem.addEventListener("click", () => {
    window.location.href = "principal.html";    
});