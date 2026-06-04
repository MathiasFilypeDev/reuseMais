document.getElementById("formCadastro").addEventListener("submit", async (e) => {
    e.preventDefault();
    const nome = document.getElementById("cadastroNome").value;
    const email = document.getElementById("cadastroEmail").value;
    const senha = document.getElementById("cadastroSenha").value;
    const tipo = document.querySelector("input[name='tipo']:checked").value;

    const response = await fetch("http://localhost:5000/api/cadastro", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ Nome: nome, Email: email, Senha: senha, Tipo: tipo })
    });

    if (response.ok) {
        const data = await response.json();
        alert(data.message);
        window.location.href = "login.html";
    } else {
        alert("Erro ao cadastrar!");
    }
});

function validarSenha(senha) {
    const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$/;
    return regex.test(senha);
}

document.getElementById("formCadastro").addEventListener("submit", function (event) {
    const senha = document.getElementById("senha").value;

    if (!validarSenha(senha)) {
        event.preventDefault(); // impede envio
        alert("A senha deve ter pelo menos uma letra maiúscula, uma minúscula, um número e no mínimo 6 caracteres.");
    }
});
