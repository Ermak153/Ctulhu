document.addEventListener('DOMContentLoaded', () => {
    const filterModal = document.getElementById("filterModal");
    const closeFilterButton = document.querySelector(".close-filter");
    const cancelFilterButton = document.getElementById("cancelFilterButton");
    const filterForm = document.getElementById("filterForm");
    const postsContainer = document.querySelector(".posts-container");

    const filterButton = document.getElementById("filterButton");
    filterButton.onclick = () => {
        filterModal.style.display = "block";
    }

    closeFilterButton.onclick = () => {
        filterModal.style.display = "none";
    }

    cancelFilterButton.onclick = () => {
        filterModal.style.display = "none";
        filterForm.reset();
        postsContainer.querySelectorAll(".post").forEach(post => {
            post.style.display = "block";
        });
    }

    filterForm.addEventListener("submit", (event) => {
        event.preventDefault();

        const selectedTags = Array.from(filterForm.querySelectorAll('input[name="tags"]:checked')).map(checkbox => checkbox.value);
        const dateFilter = filterForm.elements['dateFilter'].value;

        filterPosts(selectedTags, dateFilter);

        filterModal.style.display = "none";
    });

    function filterPosts(selectedTags, dateFilter) {
        postsContainer.querySelectorAll(".post").forEach(post => {
            const postId = post.getAttribute('data-post-id');
            var postTag = post.querySelector('.post-content small:first-child').innerText.replace('Тег: ', '');

            fetch(`/Home/GetPost/${post.getAttribute('data-post-id')}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error(`Ошибка при получении даты создания поста: ${response.status}`);
                    }

                    return response.json();
                })
                .then(data => {
                    const postCreatedAt = new Date(data.createdAt);

                    if (selectedTags.length > 0 && !selectedTags.includes(postTag)) {
                        post.style.display = "none";
                        return;
                    }

                    let isDateMatch = true;
                    switch (dateFilter) {
                        case "today":
                            isDateMatch = isToday(postCreatedAt);
                            break;
                        case "week":
                            isDateMatch = isWithinWeek(postCreatedAt);
                            break;
                        case "month":
                            isDateMatch = isWithinMonth(postCreatedAt);
                            break;
                        case "year":
                            isDateMatch = isWithinYear(postCreatedAt);
                            break;
                    }
                    if (!isDateMatch) {
                        post.style.display = "none";
                        return;
                    }

                    post.style.display = "block";
                })
                .catch(error => {
                    console.error("Ошибка при получении даты создания поста:", error);
                });
        });
    }
    function isToday(date) {
        const today = new Date();
        return date.getDate() === today.getDate() &&
            date.getMonth() === today.getMonth() &&
            date.getFullYear() === today.getFullYear();
    }

    function isWithinWeek(date) {
        const today = new Date();
        const lastWeek = new Date();
        lastWeek.setDate(today.getDate() - 7);
        return date >= lastWeek && date <= today;
    }

    function isWithinMonth(date) {
        const today = new Date();
        const lastMonth = new Date();
        lastMonth.setMonth(today.getMonth() - 1);
        return date >= lastMonth && date <= today;
    }

    function isWithinYear(date) {
        const today = new Date();
        const lastYear = new Date();
        lastYear.setFullYear(today.getFullYear() - 1);
        return date >= lastYear && date <= today;
    }
});