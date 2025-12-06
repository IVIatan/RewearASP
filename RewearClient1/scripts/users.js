const API_BASE = "http://localhost:5030/api";

export async function loadUsers() {
    const tbody = document.getElementById("usersTableBody");
    if (!tbody) {
        return;
    }

    tbody.innerHTML = "";

    try {
        const res = await fetch(`${API_BASE}/Users`);
        if (!res.ok) {
            console.error("Failed to fetch users", res.status);
            return;
        }

        const data = await res.json();
        data.forEach((u) => {
            const tr = document.createElement("tr");
            const created = u.createdAt
                ? new Date(u.createdAt).toLocaleDateString("he-IL")
                : "";
            tr.innerHTML = `
                <td>${u.userId}</td>
                <td>${u.fullName}</td>
                <td>${u.email}</td>
                <td>${u.phone ?? ""}</td>
                <td>${u.city}</td>
                <td>${u.userType}</td>
                <td>${created}</td>
                <td>${u.isActive ? "פעיל" : "לא פעיל"}</td>
            `;
            tbody.appendChild(tr);
        });
    } catch (err) {
        console.error(err);
    }
}
