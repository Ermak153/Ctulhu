document.addEventListener('DOMContentLoaded', function () {
    var filterButton = document.getElementById('filterButton');
    var filterModal = document.getElementById('filterModal');
    var closeFilter = document.querySelector('.close-filter');
    var cancelFilterButton = document.getElementById('cancelFilterButton');
    var filterForm = document.getElementById('filterForm');
    var postsContainer = document.querySelector('.posts-container');

    filterButton.addEventListener('click', function () {
        filterModal.style.display = 'block';
    });

    closeFilter.addEventListener('click', function () {
        filterModal.style.display = 'none';
    });

    cancelFilterButton.addEventListener('click', function () {
        filterModal.style.display = 'none';
    });

    window.addEventListener('click', function (event) {
        if (event.target == filterModal) {
            filterModal.style.display = 'none';
        }
    });

    filterForm.addEventListener('submit', function (event) {
        event.preventDefault();

        var selectedTags = Array.from(filterForm.elements['tags'])
            .filter(checkbox => checkbox.checked)
            .map(checkbox => checkbox.value);

        var dateFilter = filterForm.elements['dateFilter'].value;

        filterPosts(selectedTags, dateFilter);
        filterModal.style.display = 'none';
    });

    function filterPosts(tags, dateFilter) {
        var posts = Array.from(postsContainer.getElementsByClassName('post'));

        posts.forEach(post => {
            var postTag = post.querySelector('.post-content small:first-child').innerText.replace('Тег: ', '');
            var postDate = new Date(post.querySelector('.post-content small:last-child').innerText.replace('Автор: ', ''));

            var isTagMatch = tags.length === 0 || tags.includes(postTag);
            var isDateMatch = checkDateFilter(postDate, dateFilter);

            if (isTagMatch && isDateMatch) {
                post.style.display = 'block';
            } else {
                post.style.display = 'none';
            }
        });
    }

    function checkDateFilter(postDate, dateFilter) {
        var now = new Date();

        switch (dateFilter) {
            case 'today':
                return postDate.toDateString() === now.toDateString();
            case 'week':
                var oneWeekAgo = new Date();
                oneWeekAgo.setDate(now.getDate() - 7);
                return postDate >= oneWeekAgo;
            case 'month':
                var oneMonthAgo = new Date();
                oneMonthAgo.setMonth(now.getMonth() - 1);
                return postDate >= oneMonthAgo;
            case 'year':
                var oneYearAgo = new Date();
                oneYearAgo.setFullYear(now.getFullYear() - 1);
                return postDate >= oneYearAgo;
            case 'all':
            default:
                return true;
        }
    }
});
