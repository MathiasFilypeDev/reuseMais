function verificarLogin() {
    const user = JSON.parse(localStorage.getItem("user"));
    if (!user) {
        alert("Você precisa estar logado!");
        window.location.href = "login.html";
    }
}
