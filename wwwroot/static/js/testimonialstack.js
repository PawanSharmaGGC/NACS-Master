
document.addEventListener("DOMContentLoaded", function () {
    // Select all containers with the class `.container_div`
    const containers = document.querySelectorAll(".container_div");

    // Loop through each container and apply the logic separately
    containers.forEach((container) => {
        const cards = container.querySelectorAll(".ref-card");

        function updateCardClasses() {
            let closestCard = null;
            let closestDistance = Number.MAX_VALUE;

            cards.forEach((card) => {
                const rect = card.getBoundingClientRect();
                let distance = 0;
                if (window.innerWidth < 768) {
                    distance = Math.abs(rect.top - container.getBoundingClientRect().top);
                }
                else {
                    distance = Math.abs(rect.left - container.getBoundingClientRect().left);
                }
                if (distance < closestDistance) {
                    closestDistance = distance;
                    closestCard = card;
                }
            });

            cards.forEach((card) => {
                const cardBody = card.querySelector(".card-body");
                const icon = card.querySelector(".fa-solid");
                const text = card.querySelector(".text");
                const author = card.querySelector(".author");

                if (card === closestCard) {
                    cardBody.classList.add("TestimonialStackStyle-module__active_slider_card");
                    cardBody.classList.remove("slider_card");
                    icon.classList.add("color-0053A5");
                    icon.classList.remove("color-FFFFFF");
                    text.classList.remove("color-FFFFFF");
                    author.classList.remove("color-FFFFFF");
                    text.classList.add("color-002569");
                    author.classList.add("color-000000");
                } else {
                    cardBody.classList.add("slider_card");
                    cardBody.classList.remove("TestimonialStackStyle-module__active_slider_card");
                    icon.classList.remove("color-0053A5");
                    icon.classList.add("color-FFFFFF");
                    text.classList.add("color-FFFFFF");
                    author.classList.add("color-FFFFFF");
                    text.classList.remove("color-002569");
                    author.classList.remove("color-000000");
                }
            });
        }

        container.addEventListener("scroll", updateCardClasses);
        window.addEventListener("resize", updateCardClasses);

        // Run once on load to set the initial classes
        updateCardClasses();
    });
});

