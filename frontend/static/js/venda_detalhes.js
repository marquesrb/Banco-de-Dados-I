// Função para extrair o ID da venda da URL
function getVendaIdFromURL() {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get("id");  // Pega o ID da URL: ?id=123
}

// Formatar valores monetários
function formatCurrency(value) {
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(value);
}

document.addEventListener("DOMContentLoaded", function () {
    const vendaId = getVendaIdFromURL();  // Pega o ID da venda da URL
    const detalhesContainer = document.getElementById("venda_detalhes");

    if (!vendaId) {
        detalhesContainer.textContent = "ID da venda não encontrado na URL.";
        return;
    }

    // Fazer a requisição para obter os detalhes da venda
    fetch(`http://127.0.0.1:8000/vendas/${vendaId}`)  // Requisição ao backend
        .then(response => {
            if (!response.ok) {
                throw new Error("Erro ao carregar os detalhes da venda.");
            }
            return response.json();
        })
        .then(detalhes => {
            if (detalhes.error) {
                detalhesContainer.textContent = detalhes.error;
                return;
            }

            // Garantir dados básicos 
            const cliente = detalhes.nome_cliente || "Cliente não informado";
            const valorTotal = (parseFloat(detalhes.valor_produto) + parseFloat(detalhes.preco_servico)).toFixed(2);  // Soma do valor do produto e do serviço

            // Preencher o HTML com os dados da venda
            detalhesContainer.innerHTML = `
                <p><strong>ID da Venda:</strong> ${detalhes.id_venda}</p>
                <p><strong>Data da Venda:</strong> ${new Date(detalhes.data_venda).toLocaleDateString('pt-BR')}</p>
                <p><strong>Nome do Caixa:</strong> ${detalhes.nome_caixa}</p>
                <p><strong>Cliente:</strong> ${cliente}</p>
                <p><strong>Valor Total:</strong> ${formatCurrency(valorTotal)}</p>

                <h2>Produtos e Serviços</h2>
                <table>
                    <thead>
                        <tr>
                            <th>Produto/Serviço</th>
                            <th>Quantidade</th>
                            <th>Preço Unitário</th>
                            <th>Total</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>${detalhes.nome_produto}</td>
                            <td>${detalhes.quantidade_produto}</td>
                            <td>${formatCurrency(detalhes.valor_produto)}</td>
                            <td>${formatCurrency(detalhes.valor_produto * detalhes.quantidade_produto)}</td>
                        </tr>
                        <tr>
                            <td>${detalhes.tipo_servico}</td>
                            <td>1</td>
                            <td>${formatCurrency(detalhes.preco_servico)}</td>
                            <td>${formatCurrency(detalhes.preco_servico)}</td>
                        </tr>
                    </tbody>
                </table>
            `;
        })
        .catch(error => {
            console.error("Erro ao carregar os detalhes da venda:", error);
            detalhesContainer.textContent = "Erro ao carregar os detalhes da venda. Verifique e tente novamente.";
        });
});