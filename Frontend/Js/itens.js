// itens.js

function renderItens() {
    const lista = document.getElementById("listaItens");
    lista.innerHTML = "";

    // Recupera itens do localStorage
    const itens = JSON.parse(localStorage.getItem("itens")) || [];

    itens.forEach(item => {
        const card = `
      <div class="col-6 col-md-4">
        <div class="card shadow h-100">
          <img src="${item.imagem}" class="img-fluid" alt="${item.titulo}">
          <div class="card-body">
            <h5 class="card-title">${item.titulo}</h5>
            <p class="card-text">${item.descricao}</p>
            <p class="card-text"><strong>Categoria:</strong> ${item.categoria}</p>
            <p class="card-text"><strong>Disponibilidade:</strong> ${item.disponibilidade}</p>
            <p class="card-text"><strong>Prazo:</strong> ${item.prazo}</p>
            <p class="card-text"><strong>Localização:</strong> ${item.localizacao}</p>
            <p class="card-text"><strong>Qtd:</strong> ${item.quantidade}</p>
            <p class="card-text"><strong>Valor estimado:</strong> R$ ${item.valor}</p>
            <p class="card-text"><strong>Status:</strong> ${item.status}</p>
            <button class="btn btn-outline-primary w-100">Tenho interesse</button>
          </div>
        </div>
      </div>
    `;
        lista.innerHTML += card;
    });
}

document.addEventListener("DOMContentLoaded", renderItens);
