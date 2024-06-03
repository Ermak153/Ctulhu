document.addEventListener('DOMContentLoaded', (event) => {
    const modal = document.getElementById("modal");
    const error = document.getElementById("error");
    const createPostButton = document.getElementById("createPostButton");
    const closeButton = document.querySelector(".close");
    const cancelButton = document.getElementById("cancelButton");
    const createPostForm = document.getElementById('createPostForm');
    const createButton = document.getElementById('createButton');

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
        var title = createPostForm.elements['title'].value;
        var description = createPostForm.elements['description'].value;
        var image = createPostForm.elements['image'].files[0];
        var tag = Array.from(createPostForm.elements['tag']).some(radio => radio.checked);

        if (!title || !description || !image || !tag) {
            event.preventDefault();
            error.style.display = "block";
            modal.style.display = "block";
        }
    });
});
