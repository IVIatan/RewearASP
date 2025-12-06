const API_BASE = "http://localhost:5030/api";

export async function loadStores() {
    const tbody = document.getElementById("storesTableBody");
    if (!tbody) {
        return;
    }

    tbody.innerHTML = "";

    try {
        const res = await fetch(`${API_BASE}/Stores`);
        if (!res.ok) {
            console.error("Failed to fetch stores", res.status);
            return;
        }

        const data = await res.json();
        data.forEach((s) => {
            const created = s.createdAt
                ? new Date(s.createdAt).toLocaleDateString("he-IL")
                : "";
            const row = `
                <tr>
                    <td>${s.storeId}</td>
                    <td>${s.ownerUserId}</td>
                    <td>${s.storeName}</td>
                    <td>${s.purpose}</td>
                    <td>${s.city}</td>
                    <td>${created}</td>
                    <td>${s.isActive ? "פעיל" : "לא פעיל"}</td>
                </tr>
            `;
            tbody.insertAdjacentHTML("beforeend", row);
        });
    } catch (err) {
        console.error(err);
    }
}
