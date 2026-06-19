document.getElementById("formCadastro").addEventListener("submit", async (e) => {
    e.preventDefault();

    const nome = document.getElementById("username").value; // aproveitando o campo existente
    const email = document.getElementById("password").value; // aqui usamos como email só para simular

    const response = await fetch("http://localhost:5069/api/users", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ nome, email })
    });

    if (response.ok) {
        const data = await response.json();
        alert("Usuário cadastrado com sucesso: " + data.nome);
        window.location.href = "login.html"; // redireciona para login após cadastro
    } else {
        alert("Erro ao cadastrar usuário. Verifique os dados.");
    }
});
