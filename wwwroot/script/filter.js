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

        var dateFilter = filterForm.elements['dateFilter'].value;

        fetchPosts(dateFilter);
        filterModal.style.display = 'none';
    });

    function fetchPosts(dateFilter) {
        fetch('/Home/GetPosts')
            .then(response => response.json())
            .then(posts => {
                updatePostAttributes(posts);
                filterPosts(dateFilter);
            })
            .catch(error => console.error('Error fetching posts:', error));
    }

    function updatePostAttributes(posts) {
        posts.forEach(post => {
            var postElement = document.getElementById(`${post.ID}`);
            console.log('------------------')
            console.log(post.ID)
            console.log(post.CreatedAt)
            console.log('------------------')
            if (postElement) {
                postElement.setAttribute('data-created-at', post.CreatedAt);
            }
        });
    }

    function filterPosts(dateFilter) {
        var now = new Date();
        var posts = Array.from(postsContainer.getElementsByClassName('post'));

        posts.forEach(post => {
            var postDate = new Date(post.getAttribute('data-created-at'));

            var isDateMatch = checkDateFilter(postDate, dateFilter, now);

            if (isDateMatch) {
                post.style.display = 'block';
            } else {
                post.style.display = 'none';
            }
        });
    }

    function checkDateFilter(postDate, dateFilter, now) {
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
