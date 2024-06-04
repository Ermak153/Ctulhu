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
            } else if (action === 'cancel') {
                cancelEdit(postId);
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
        const originalDescription = postElement.getAttribute('data-full-description');

        titleElement.innerHTML = `<input type='text' value='${originalTitle}' />`;
        descriptionElement.innerHTML = `<textarea>${originalDescription}</textarea>`;

        editButton.textContent = 'Сохранить';
        editButton.setAttribute('data-action', 'save');
        editButton.setAttribute('onclick', `savePost(${postId}, this)`);

        const cancelButton = postElement.querySelector('.cancel-button1');
        cancelButton.style.display = 'inline-block';
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
                const updatedDescription = postElement.querySelector('p textarea').value;
                postElement.querySelector('p').textContent = (updatedDescription.length > 150
                    ? updatedDescription.substring(0, 150) + "..."
                    : updatedDescription);
                isEditing = false;
                saveButton.textContent = 'Редактировать';
                saveButton.setAttribute('data-action', 'edit');
                saveButton.setAttribute('onclick', '');
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
            body: JSON.stringify(postId)
        }).then(response => {
            if (response.ok) {
                const postElement = document.querySelector(`.post[data-post-id='${postId}']`);
                postElement.remove();
            } else {
                response.json().then(data => {
                    alert(`Ошибка при удалении поста: ${data.message}`);
                });
            }
        }).catch(error => {
            alert(`Ошибка при удалении поста: ${error.message}`);
        });
    }
    function cancelEdit(postId) {
        const postElement = document.querySelector(`.post[data-post-id='${postId}']`);
        const titleElement = postElement.querySelector('h2');
        const descriptionElement = postElement.querySelector('p');

        titleElement.textContent = titleElement.querySelector('input').defaultValue;
        descriptionElement.textContent = (postElement.getAttribute('data-full-description').length > 150
            ? postElement.getAttribute('data-full-description').substring(0, 150) + "..."
            : postElement.getAttribute('data-full-description'));

        const editButton = postElement.querySelector('[data-action="save"]');
        editButton.textContent = 'Редактировать';
        editButton.setAttribute('data-action', 'edit');
        editButton.removeAttribute('onclick');
        isEditing = false;

        const cancelButton = postElement.querySelector('.cancel-button1');
        cancelButton.style.display = 'none';
    }
});
