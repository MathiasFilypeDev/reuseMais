function showMessage(message, type) {
    const container = document.createElement("div");
    container.className = `alert alert-${type} alert-dismissible fade show mt-2`;
    container.role = "alert";
    container.innerHTML = `
        <strong>${type === "success" ? "✅" : type === "warning" ? "⚠️" : "❌"}</strong> ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;

    const msgArea = document.getElementById("msgArea");
    msgArea.innerHTML = "";
    msgArea.appendChild(container);

    setTimeout(() => {
        if (container.parentNode) {
            container.remove();
        }
    }, 5000);
}

async function fetchComToken(endpoint, options = {}) {
    const token = localStorage.getItem("token");
    const headers = {
        "Content-Type": "application/json",
        ...options.headers,
        "Authorization": `Bearer ${token}`
    };
    return fetch(`${API_URL}${endpoint}`, { ...options, headers });
}
