// Busca a ficha de horários de um determinado funcionário
document.getElementById("buscar-ficha-horarios").addEventListener("click", async function () {
    const funcionarioId = document.getElementById("funcionario-id").value.trim();

    if (!funcionarioId) {
        alert("Digite o ID do funcionário.");
        return;
    }

    const url = new URL("http://localhost:8000/agendamentos/ficha_horarios");
    url.searchParams.append("funcionario_id", funcionarioId);

    try {
        const response = await fetch(url);
        if (!response.ok) throw new Error(`Erro: ${response.statusText}`);

        const fichaHorarios = await response.json();
        const tabelaBody = document.querySelector("#ficha-horarios-tabela tbody");
        tabelaBody.innerHTML = "";  // Limpa a tabela

        if (fichaHorarios.length > 0) {
            fichaHorarios.forEach(ag => {
                const row = tabelaBody.insertRow();
                row.insertCell(0).textContent = ag.data;  
                row.insertCell(1).textContent = ag.horario; 
                row.insertCell(2).textContent = ag.nome_cliente;  
                row.insertCell(3).textContent = ag.nome_pet;  
                row.insertCell(4).textContent = ag.tipo;  
            });
        } else {
            const row = tabelaBody.insertRow();
            row.insertCell(0).textContent = "Nenhuma ficha de horários encontrada.";
        }
    } catch (error) {
        console.error("Erro ao buscar ficha de horários:", error);
        alert("Erro ao buscar ficha de horários.");
    }
});