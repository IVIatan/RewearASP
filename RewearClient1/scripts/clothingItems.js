const API_BASE = "http://localhost:5030/api";

function createItemRow(item) {
    const tr = document.createElement("tr");
    const expiration = item.expirationDate
        ? new Date(item.expirationDate).toLocaleDateString("he-IL")
        : "";
    const ownerName = item.ownerFullName || "";
    const destStoreName = item.destinationStoreName || "";
    const imagePath = item.imagePath || item.imageURL || null;
    let imageCellHtml = "";

    if (imagePath) {
        const baseForFiles = API_BASE.replace("/api", "");
        const url = imagePath.startsWith("http")
            ? imagePath
            : `${baseForFiles}/${imagePath.replace(/^\/+/, "")}`;
        imageCellHtml = `<img src="${url}" alt="תמונה" class="item-image">`;
    }

    tr.innerHTML = `
        <td>${item.itemId}</td>
        <td>${item.ownerUserId}</td>
        <td>${ownerName}</td>
        <td>${item.storeId ?? ""}</td>
        <td>${item.destinationStoreId ?? ""}</td>
        <td>${destStoreName}</td>
        <td>${item.title}</td>
        <td>${item.category}</td>
        <td>${item.size}</td>
        <td>${item.condition}</td>
        <td>${item.color}</td>
        <td>${item.price ?? ""}</td>
        <td>${item.status}</td>
        <td>${expiration}</td>
        <td>${imageCellHtml}</td>
        <td><button class="btn btn-danger btn-sm" data-id="${item.itemId}">מחק</button></td>
    `;

    const deleteBtn = tr.querySelector("button[data-id]");
    deleteBtn.addEventListener("click", async () => {
        const id = deleteBtn.dataset.id;
        const confirmDelete = confirm("למחוק את הפריט הזה?");
        if (!confirmDelete) {
            return;
        }
        await deleteItem(id);
        await loadClothingItems();
    });

    return tr;
}

export async function loadClothingItems() {
    const tbody = document.getElementById("itemsTableBody");
    if (!tbody) {
        return;
    }

    tbody.innerHTML = "";

    try {
        const res = await fetch(`${API_BASE}/ClothingItems`);
        if (!res.ok) {
            console.error("Failed to fetch items", res.status);
            return;
        }

        const data = await res.json();
        data.forEach((item) => {
            const row = createItemRow(item);
            tbody.appendChild(row);
        });
    } catch (err) {
        console.error(err);
    }
}

async function deleteItem(id) {
    try {
        const res = await fetch(`${API_BASE}/ClothingItems/${id}`, {
            method: "DELETE"
        });

        if (!res.ok) {
            console.error("Failed to delete item", res.status);
        }
    } catch (err) {
        console.error(err);
    }
}

export async function handleAddItemSubmit(event) {
    event.preventDefault();

    const form = event.target;

    const ownerUserId = Number(form.ownerUserId.value);
    const storeId = form.storeId.value ? Number(form.storeId.value) : null;
    const destinationStoreId = form.destinationStoreId.value
        ? Number(form.destinationStoreId.value)
        : null;
    const title = form.title.value.trim();
    const description = form.description.value.trim() || null;
    const category = form.category.value.trim();
    const size = form.size.value.trim();
    const condition = form.condition.value.trim();
    const color = form.color.value.trim();
    const price = form.price.value ? Number(form.price.value) : null;
    const status = form.status.value;
    const expirationDate = form.expirationDate.value
        ? form.expirationDate.value
        : null;
    const fileInput = form.itemImage;

    const body = {
        ownerUserId,
        storeId,
        destinationStoreId,
        title,
        description,
        category,
        size,
        condition,
        color,
        price,
        status,
        expirationDate
    };

    try {
        const res = await fetch(`${API_BASE}/ClothingItems`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(body)
        });

        if (!res.ok) {
            console.error("Failed to add item", res.status);
            return;
        }

        const created = await res.json();
        let itemId = created.itemId || created.ItemId || created.id || created;

        if (fileInput && fileInput.files && fileInput.files.length > 0 && itemId) {
            const formData = new FormData();
            formData.append("file", fileInput.files[0]);

            const uploadRes = await fetch(
                `${API_BASE}/ClothingItems/${itemId}/image`,
                {
                    method: "POST",
                    body: formData
                }
            );

            if (!uploadRes.ok) {
                console.error("Failed to upload image", uploadRes.status);
            }
        }

        form.reset();
        if (form.status) {
            form.status.value = "Available";
        }

        await loadClothingItems();
    } catch (err) {
        console.error(err);
    }
}
