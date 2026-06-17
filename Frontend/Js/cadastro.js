document.getElementById("formCadastro").addEventListener("submit", async (e) => {
    e.preventDefault();

    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;

    const response = await fetch("/api/auth/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password })
    });

    if (response.ok) {
        const data = await response.json();
        alert(data.message);
        window.location.href = "login.html"; // redireciona para login após cadastro
    } else {
        alert("Erro ao cadastrar usuário. Verifique os dados.");
    }
});
