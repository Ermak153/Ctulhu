document.addEventListener('DOMContentLoaded', (event) => {
    const modal = document.getElementById("modal");
    const error = document.getElementById("error");
    const createPostButton = document.getElementById("createPostButton");
    const closeButton = document.querySelector(".close");
    const cancelButton = document.getElementById("cancelButton");
    const createPostForm = document.getElementById('createPostForm');
    const createButton = document.getElementById('createButton');
    const titleInput = createPostForm.elements['title'];

    createPostButton.onclick = function () {
        modal.style.display = "block";
    }

    closeButton.onclick = function () {
        modal.style.display = "none";
        error.style.display = "none";
    }

    cancelButton.onclick = function () {
        modal.style.display = "none";
        error.style.display = "none";
    }

    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
            error.style.display = "none";
        }
    }

    createPostForm.addEventListener('submit', function (event) {
        var title = titleInput.value;
        var description = createPostForm.elements['description'].value;
        var image = createPostForm.elements['image'].files[0];
        var tag = Array.from(createPostForm.elements['tag']).some(radio => radio.checked);

        if (title.length > 20) {
            event.preventDefault();
            error.textContent = "Заголовок не может быть длиннее 20 символов";
            error.style.display = "block";
            modal.style.display = "block";
            return;
        }

        if (!title || !description || !image || !tag) {
            event.preventDefault();
            error.textContent = "Все поля должны быть заполнены";
            error.style.display = "block";
            modal.style.display = "block";
        }
    });
});