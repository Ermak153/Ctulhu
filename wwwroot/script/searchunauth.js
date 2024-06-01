document.addEventListener("DOMContentLoaded", function () {
    const searchInput = document.getElementById("searchunauth");
    const postsContainer = document.querySelector(".posts-container");
    const posts = postsContainer.querySelectorAll(".post");

    searchInput.addEventListener("input", function () {
        const searchTerm = searchInput.value.toLowerCase();

        posts.forEach(function (post) {
            const title = post.querySelector("h2").textContent.toLowerCase();
            const description = post.querySelector("p").textContent.toLowerCase();

            if (title.includes(searchTerm) || description.includes(searchTerm)) {
                post.style.visibility = "visible";
                post.style.position = "static";
            } else {
                post.style.visibility = "hidden";
                post.style.position = "absolute";
            }
        });
    });
});
