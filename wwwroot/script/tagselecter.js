document.addEventListener('DOMContentLoaded', function () {
    const tagSuggestionsContainer = document.getElementById('tagSuggestions');
    const selectedTagsContainer = document.getElementById('selectedTags');
    const inputTags = document.getElementById('tags');

    // Получаем все доступные теги и показываем их
    getAllTags().then(tags => {
        renderTagSuggestions(tags);
    });

    // Функция для отображения списка тегов
    function renderTagSuggestions(tags) {
        tagSuggestionsContainer.innerHTML = '';
        tags.forEach(tag => {
            const tagElement = document.createElement('div');
            tagElement.classList.add('tag-suggestion');
            tagElement.textContent = tag;
            tagElement.addEventListener('click', function () {
                addTag(tag);
            });
            tagSuggestionsContainer.appendChild(tagElement);
        });
    }

    // Функция для добавления выбранного тега в поле ввода
    function addTag(tagName) {
        // Проверяем, не был ли тег уже добавлен
        if (!Array.from(selectedTagsContainer.querySelectorAll('.tag')).some(tag => tag.dataset.tag === tagName)) {
            // Создаем элемент для выбранного тега
            const tagElement = document.createElement('span');
            tagElement.classList.add('tag');
            tagElement.textContent = tagName;
            tagElement.dataset.tag = tagName;

            // Создаем кнопку для удаления тега
            const closeButton = document.createElement('button');
            closeButton.innerHTML = '&times;';
            closeButton.classList.add('close');
            closeButton.addEventListener('click', function () {
                tagElement.remove();
                updateTagsInput();
                renderTagSuggestions([tagName]);
            });

            // Добавляем кнопку удаления к элементу тега
            tagElement.appendChild(closeButton);

            // Добавляем выбранный тег к контейнеру выбранных тегов
            selectedTagsContainer.appendChild(tagElement);

            // Обновляем скрытое поле с тегами
            updateTagsInput();

            // Убираем выбранный тег из списка предложений
            const suggestion = tagSuggestionsContainer.querySelector('.tag-suggestion:contains("' + tagName + '")');
            if (suggestion) {
                suggestion.remove();
            }
        }
    }

    // Функция для обновления скрытого поля с тегами
    function updateTagsInput() {
        const selectedTags = Array.from(selectedTagsContainer.querySelectorAll('.tag')).map(tag => tag.dataset.tag);
        inputTags.value = selectedTags.join(',');
    }

    // Функция для получения всех тегов
    function getAllTags() {
        return fetch(`/tags/search`)
            .then(response => response.json())
            .then(data => data);
    }
});
