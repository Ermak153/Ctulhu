document.addEventListener('DOMContentLoaded', (event) => {
    const modal = document.getElementById("modal");
    const createPostButton = document.getElementById("createPostButton");
    const closeButton = document.querySelector(".close");
    const cancelButton = document.getElementById("cancelButton");

    createPostButton.onclick = function () {
        modal.style.display = "block";
    }

    closeButton.onclick = function () {
        modal.style.display = "none";
    }

    cancelButton.onclick = function () {
        modal.style.display = "none";
    }

    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    }
});
