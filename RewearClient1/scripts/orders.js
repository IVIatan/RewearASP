const API_BASE = "http://localhost:5030/api";

export async function loadOrders() {
    const tbody = document.getElementById("ordersTableBody");
    if (!tbody) {
        return;
    }

    tbody.innerHTML = "";

    try {
        const res = await fetch(`${API_BASE}/Orders`);
        if (!res.ok) {
            console.error("Failed to fetch orders", res.status);
            return;
        }

        const data = await res.json();
        data.forEach((o) => {
            const date = o.orderDate
                ? new Date(o.orderDate).toLocaleDateString("he-IL")
                : "";
            const row = `
                <tr>
                    <td>${o.orderId}</td>
                    <td>${o.itemId}</td>
                    <td>${o.itemTitle ?? ""}</td>
                    <td>${o.buyerUserId}</td>
                    <td>${o.buyerFullName ?? ""}</td>
                    <td>${o.sellerUserId}</td>
                    <td>${o.sellerFullName ?? ""}</td>
                    <td>${date}</td>
                    <td>${o.totalAmount}</td>
                    <td>${o.status}</td>
                </tr>
            `;
            tbody.insertAdjacentHTML("beforeend", row);
        });
    } catch (err) {
        console.error(err);
    }
}
