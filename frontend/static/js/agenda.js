 // Redirecionar para a página de criação de agendamento
 document.getElementById("criar-agendamento").addEventListener("click", function () {
    window.location.href = "/pages/agenda/criar_agendamento.html";
});

// Redirecionar para a página de criação de agendamento
document.getElementById("buscar-ficha-banho-tosa").addEventListener("click", function () {
    window.location.href = "/pages/agenda/buscar_ficha_banho_tosa.html";
});

// Busca todos os agendamentos
document.getElementById("buscar-agendamentos").addEventListener("click", async function () {
    const dataInicio = document.getElementById("data-inicio").value || null;
    const dataFim = document.getElementById("data-fim").value || null;

    const url = new URL("http://localhost:8000/agendamentos");
    if (dataInicio) url.searchParams.append("start_date", dataInicio);
    if (dataFim) url.searchParams.append("end_date", dataFim);

    try {
        const response = await fetch(url);
        if (!response.ok) {
            if (response.status === 404) {
                alert("Nenhum agendamento encontrado para o período.");
            }
            throw new Error(`Erro na consulta: ${response.statusText}`);
        }

        const agendamentos = await response.json();
        const tabelaBody = document.querySelector("#agendamentos-tabela tbody");
        tabelaBody.innerHTML = "";

        if (agendamentos.length > 0) {
            agendamentos.forEach(ag => {
                const row = tabelaBody.insertRow();
                row.insertCell(0).textContent = ag.data;
                row.insertCell(1).textContent = ag.horario;
                row.insertCell(2).textContent = ag.funcionario_nome;
                row.insertCell(3).textContent = ag.preco;
                row.insertCell(4).textContent = ag.tipo;
            });
        } else {
            const row = tabelaBody.insertRow();
            row.insertCell(0).textContent = "Nenhum agendamento encontrado.";
        }
    } catch (error) {
        console.error("Erro ao buscar agendamentos:", error);
        alert("Ocorreu um erro ao buscar os agendamentos.");
    }
});

 // Busca os agendamentos de um determinado cliente
document.getElementById("buscar-historico-cliente").addEventListener("click", async function () {
const clienteId = document.getElementById("cliente-id").value.trim();  // Usando o id correto do cliente

if (!clienteId) {
    alert("Digite o ID do cliente.");
    return;
}

const url = new URL("http://localhost:8000/historico_cliente");
url.searchParams.append("cliente_id", clienteId);  

try {
    const response = await fetch(url);
    if (!response.ok) throw new Error(`Erro: ${response.statusText}`);

    const historico = await response.json();
    const tabelaBody = document.querySelector("#historico-cliente-tabela tbody");
    tabelaBody.innerHTML = "";  

    if (historico.length > 0) {
        historico.forEach(ag => {
            const row = tabelaBody.insertRow();
            row.insertCell(0).textContent = ag.data_servico;
            row.insertCell(1).textContent = ag.horario_servico;
            row.insertCell(2).textContent = ag.nome_pet;
            row.insertCell(3).textContent = ag.preco_servico;
            row.insertCell(4).textContent = ag.status_servico;
            row.insertCell(5).textContent = ag.tipo_servico;
        });
    } else {
        const row = tabelaBody.insertRow();
        row.insertCell(0).textContent = "Nenhum agendamento encontrado.";
    }
} catch (error) {
    console.error("Erro:", error);
    alert("Erro ao buscar histórico do cliente.");
}
});