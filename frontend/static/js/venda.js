document.getElementById("buscar-vendas").addEventListener("click", function () {
    // Recuperar valores dos filtros
    const produto = document.getElementById("produto").value;
    const dataInicio = document.getElementById("data-inicio").value;
    const dataFim = document.getElementById("data-fim").value;

    // Montar a URL com os parâmetros de consulta
    const url = new URL("http://localhost:8000/vendas");
    if (dataInicio) url.searchParams.append("start_date", dataInicio);
    if (dataFim) url.searchParams.append("end_date", dataFim);
    if (produto) url.searchParams.append("produto", produto);

    // Requisição Vendas
    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error(`Erro na consulta: ${response.statusText}`);
            }
            return response.json();
        })
        .then(data => {
            // Preencher a tabela com os resultados
            const tabela = document.querySelector("#tabela-vendas tbody");
            tabela.innerHTML = ""; // Limpar tabela

            if (data.vendas && data.vendas.length > 0) {
                data.vendas.forEach(venda => {
                    const row = tabela.insertRow();
                    row.insertCell(0).textContent = venda.data_venda;
                    row.insertCell(1).textContent = venda.nome_cliente;
                    row.insertCell(2).textContent = venda.quantidade;
                    row.insertCell(3).textContent = venda.valor_total_venda;

                     // Criar um link para a página de detalhes
                     const cellDetalhes = row.insertCell(4);
                     const detalhesLink = document.createElement("a");
                     detalhesLink.href = `http://127.0.0.1:5500/pages/venda/venda_detalhes.html?id=${venda.id}`;  
                     detalhesLink.textContent = "Detalhes";
                     cellDetalhes.appendChild(detalhesLink);
                });
            } else {
                // Caso nenhum resultado seja encontrado
                const row = tabela.insertRow();
                const cell = row.insertCell(0);
                cell.colSpan = 4;
                cell.textContent = "Nenhuma venda encontrada.";
                cell.style.textAlign = "center";
            }
        })
        .catch(error => {
            console.error("Erro ao buscar vendas:", error);
            alert("Ocorreu um erro ao buscar as vendas. Tente novamente.");
        });
});

// Requisição listagem de produtos
document.addEventListener("DOMContentLoaded", function () {
    const selectProduto = document.getElementById("produto");
    selectProduto.innerHTML = '<option value="">Carregando produtos...</option>';

    fetch("http://localhost:8000/produtos")
        .then(response => {
            if (!response.ok) {
                throw new Error(`Erro ao buscar produtos: ${response.statusText}`);
            }
            return response.json();
        })
        .then(data => {
            selectProduto.innerHTML = '<option value="">Selecione um Produto</option>';

            if (data && data.length > 0) {
                data.forEach(produto => {
                    const option = document.createElement("option");
                    option.value = produto.nome; 
                    option.textContent = produto.nome;
                    selectProduto.appendChild(option);
                });
            } else {
                const option = document.createElement("option");
                option.value = "";
                option.textContent = "Nenhum produto disponível";
                selectProduto.appendChild(option);
            }
        })
        .catch(error => {
            console.error("Erro ao carregar produtos:", error);
            alert("Ocorreu um erro ao carregar a lista de produtos.");
        });
});