import { loadClothingItems, handleAddItemSubmit } from "./clothingItems.js";
import { loadUsers } from "./users.js";
import { loadStores } from "./stores.js";
import { loadOrders } from "./orders.js";

document.addEventListener("DOMContentLoaded", () => {
    loadClothingItems();

    const form = document.querySelector("#addItemForm");
    if (form) {
        form.addEventListener("submit", handleAddItemSubmit);
    }

    const reloadItemsBtn = document.querySelector("#reloadItemsBtn");
    if (reloadItemsBtn) {
        reloadItemsBtn.addEventListener("click", loadClothingItems);
    }

    const reloadUsersBtn = document.querySelector("#reloadUsersBtn");
    if (reloadUsersBtn) {
        reloadUsersBtn.addEventListener("click", loadUsers);
    }

    const reloadStoresBtn = document.querySelector("#reloadStoresBtn");
    if (reloadStoresBtn) {
        reloadStoresBtn.addEventListener("click", loadStores);
    }

    const reloadOrdersBtn = document.querySelector("#reloadOrdersBtn");
    if (reloadOrdersBtn) {
        reloadOrdersBtn.addEventListener("click", loadOrders);
    }

    const tabs = document.querySelectorAll(".tab-button");
    const sections = document.querySelectorAll(".section");

    tabs.forEach((btn) => {
        btn.addEventListener("click", () => {
            const target = btn.dataset.target;
            tabs.forEach((b) => b.classList.remove("active"));
            btn.classList.add("active");
            sections.forEach((sec) => {
                sec.classList.toggle("active", sec.id === target);
            });
        });
    });
});
