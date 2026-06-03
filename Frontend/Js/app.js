// app.js ou cadastroItens.js
document.addEventListener("DOMContentLoaded", () => {
    document.getElementById("btn-home").addEventListener("click", () => {
        window.location.href = "index.html";
    });

    document.getElementById("btn-principal").addEventListener("click", () => {
        window.location.href = "principal.html";
    });

    
    document.getElementById("btn-sobre").addEventListener("click", () => {
        window.location.href = "about.html";
    });
});


const btnExplorar = document.getElementById("btn-explorar").addEventListener("click", () => {
    window.location.href = "login.html";
});

function getStoredItems() {
    const stored = JSON.parse(localStorage.getItem("itens")) || [];

return stored.map((item, index) => ({
    ...item,
    id: item.id ?? index + 1,
    status: item.status || "disponível",
    bids: item.bids || []
}));


}

function saveItems(items) {
    localStorage.setItem("itens", JSON.stringify(items));
}

function initializeItems() {
    const stored = getStoredItems();

    if (stored.length === 0) {
        saveItems(itensIniciais);
    }
}

function formatCurrency(value) {
    return Number(value || 0).toLocaleString(
        "pt-BR",
        {
            style: "currency",
            currency: "BRL"
        }
    );
}

function renderCategoryFilter(items) {
    const filter = document.getElementById("categoryFilter");


    if (!filter) return;

    const categorias = [
        ...new Set(
            items
                .map(item => item.categoria)
                .filter(Boolean)
        )
    ];

    filter.innerHTML = `
        < option value = "" > Todas as categorias</option >
            ${categorias
            .map(cat => `<option value="${cat}">${cat}</option>`)
            .join("")
        }
    `;

}

function createCarousel(item) {


    if (!item.fotos || item.fotos.length === 0) {
        return "";
    }

    return `
        < div id = "carouselItem${item.id}"
    class="carousel slide"
    data - bs - ride="carousel" >

        <div class="carousel-inner">

            ${item.fotos.map((foto, index) => `
                <div class="carousel-item ${index === 0 ? "active" : ""}">
                    <img
                        src="${foto}"
                        alt="${item.nome || item.descricao}">
                </div>
            `).join("")}

        </div>

        ${item.fotos.length > 1 ? `
            <button
                class="carousel-control-prev"
                type="button"
                data-bs-target="#carouselItem${item.id}"
                data-bs-slide="prev">

                <span class="carousel-control-prev-icon"></span>

            </button>

            <button
                class="carousel-control-next"
                type="button"
                data-bs-target="#carouselItem${item.id}"
                data-bs-slide="next">

                <span class="carousel-control-next-icon"></span>

            </button>
        ` : ""
        }
    </div >
        `;


}

function renderItems(items) {


    const lista = document.getElementById("listaItens");

    if (!lista) return;

    if (items.length === 0) {
        lista.innerHTML = `
        < div class="col-12" >
            <div class="alert alert-warning text-center">
                Nenhum item encontrado.
            </div>
        </div>
        `;
        return;
    }

    lista.innerHTML = items.map(item => {

        const bidsCount = item.bids?.length || 0;

        return `
        < div class="col-sm-6 col-lg-4" >

            <div class="card item-card h-100">

                ${createCarousel(item)}

                <div class="card-body d-flex flex-column">

                    <div>

                        <span class="badge bg-info text-dark mb-2">
                            ${item.categoria}
                        </span>

                        <h5 class="card-title">
                            ${item.nome}
                        </h5>

                        <p class="card-text text-secondary">
                            ${item.descricao}
                        </p>

                        <p class="card-text">
                            <strong>Local:</strong>
                            ${item.local}
                        </p>

                        <p class="card-text">
                            <strong>Quantidade:</strong>
                            ${item.quantidade}
                        </p>

                        <p class="card-text">
                            <strong>Valor:</strong>
                            ${formatCurrency(item.valor)}
                        </p>

                        <div class="mb-3">

                            <span class="badge bg-secondary">
                                ${item.status}
                            </span>

                            <span class="badge bg-dark">
                                ${item.prazo || "Sem prazo"}
                            </span>

                        </div>

                    </div>

                    <div class="mt-auto">

                        <div class="d-flex justify-content-between mb-3">

                            <span class="badge bg-success">
                                #${item.id}
                            </span>

                            <span class="badge bg-secondary">
                                ${bidsCount} proposta(s)
                            </span>

                        </div>

                        <button
                            class="btn btn-primary w-100"
                            onclick="window.location.href='recebimento.html?itemId=${item.id}'">

                            Tenho interesse

                        </button>

                    </div>

                </div>

            </div>

        </div >
        `;
    }).join("");


}

function filterItems() {


    const search =
        document
            .getElementById("searchInput")
            ?.value
            .toLowerCase() || "";

    const category =
        document
            .getElementById("categoryFilter")
            ?.value || "";

    const items = getStoredItems();

    const filtered = items.filter(item => {

        const text = [
            item.nome,
            item.descricao,
            item.categoria,
            item.local
        ]
            .filter(Boolean)
            .join(" ")
            .toLowerCase();

        const matchesSearch =
            text.includes(search);

        const matchesCategory =
            !category ||
            item.categoria === category;

        return matchesSearch && matchesCategory;
    });

    renderItems(filtered);


}

window.addEventListener("DOMContentLoaded", () => {

    initializeItems();

    const items = getStoredItems();

    renderCategoryFilter(items);
    renderItems(items);

    document
        .getElementById("searchInput")
        ?.addEventListener("input", filterItems);

    document
        .getElementById("categoryFilter")
        ?.addEventListener("change", filterItems);

    const year = document.getElementById("currentYear");

    if (year) {
        year.textContent = new Date().getFullYear();
    }


});
