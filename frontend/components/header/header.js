fetch("/components/header/header.html")
    .then(response => {
        if (!response.ok) {
            throw new Error(`Erro ao carregar o header: ${response.statusText}`);
        }
        return response.text();
    })
    .then(data => {
        document.getElementById("header").innerHTML = data;
    })
    .catch(error => console.error("Erro ao carregar o header:", error));

    document.addEventListener("DOMContentLoaded", () => {
        const header = document.getElementById("header");
        const container = document.querySelector(".container");
    
        function ajustarLayout() {
            const larguraMenu = header.offsetWidth;
            container.style.marginLeft = `${larguraMenu}px`;
            container.style.width = `calc(100% - ${larguraMenu}px)`;
        }
    
        ajustarLayout();
    
        window.addEventListener("resize", ajustarLayout); // Recalcula no redimensionamento
    });
    