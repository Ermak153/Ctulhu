document.addEventListener('DOMContentLoaded', function () {
    let currentOpenMenu = null;
    let isEditing = false;

    document.querySelectorAll('.menu-button').forEach(button => {
        button.addEventListener('click', function (e) {
            if (currentOpenMenu && currentOpenMenu !== button) {
                currentOpenMenu.nextElementSibling.classList.remove('open');
            }
            currentOpenMenu = button;
            button.nextElementSibling.classList.toggle('open');
        });
    });

    document.addEventListener('click', function (e) {
        if (currentOpenMenu && !currentOpenMenu.contains(e.target) && !currentOpenMenu.nextElementSibling.contains(e.target)) {
            currentOpenMenu.nextElementSibling.classList.remove('open');
            currentOpenMenu = null;
        }
    });

    document.querySelectorAll('.menu-list li').forEach(item => {
        item.addEventListener('click', function (e) {
            const action = e.target.getAttribute('data-action');
            const postId = e.target.closest('.post').getAttribute('data-post-id');
            if (action === 'edit') {
                editPost(postId, e.target);
            } else if (action === 'delete') {
                deletePost(postId);
            }
        });
    });

    function editPost(postId, editButton) {
        if (isEditing) {
            alert("Вы уже редактируете другой пост.");
            return;
        }
        isEditing = true;

        const postElement = document.querySelector(`.post[data-post-id='${postId}']`);
        const titleElement = postElement.querySelector('h2');
        const descriptionElement = postElement.querySelector('p');

        const originalTitle = titleElement.textContent;
        const originalDescription = descriptionElement.textContent;

        titleElement.innerHTML = `<input type='text' value='${originalTitle}' />`;
        descriptionElement.innerHTML = `<textarea>${originalDescription}</textarea>`;

        editButton.textContent = 'Сохранить';
        editButton.setAttribute('data-action', 'save');
        editButton.setAttribute('onclick', `savePost(${postId}, this)`); // Добавлена ссылка на функцию savePost
    }

    window.savePost = function (postId, saveButton) {
        const postElement = document.querySelector(`.post[data-post-id='${postId}']`);
        const titleElement = postElement.querySelector('h2 input');
        const descriptionElement = postElement.querySelector('p textarea');

        const updatedTitle = titleElement.value;
        const updatedDescription = descriptionElement.value;

        fetch('/Home/EditPost', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ id: postId, title: updatedTitle, description: updatedDescription })
        }).then(response => {
            if (response.ok) {
                postElement.querySelector('h2').textContent = updatedTitle;
                postElement.querySelector('p').textContent = updatedDescription;
                isEditing = false;
                saveButton.textContent = 'Редактировать';
                saveButton.setAttribute('data-action', 'edit');
                saveButton.setAttribute('onclick', ''); // Удаляем атрибут onclick
            } else {
                alert("Ошибка при сохранении изменений.");
            }
        });
    }

    function deletePost(postId) {
        if (!confirm("Вы уверены, что хотите удалить этот пост?")) {
            return;
        }
        fetch(`/Home/DeletePost`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ id: postId })
        }).then(response => {
            if (response.ok) {
                const postElement = document.querySelector(`.post[data-post-id='${postId}']`);
                postElement.remove();
            } else {
                alert("Ошибка при удалении поста.");
            }
        });
    }
});
