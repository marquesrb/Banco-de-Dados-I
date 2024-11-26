document.getElementById("form-criar-agendamento").addEventListener("submit", async function (e) {
    e.preventDefault();

    const tipo = document.getElementById("tipo").value;
    const preco = document.getElementById("preco").value;
    const status = document.getElementById("status").value; 
    const data = document.getElementById("data").value;
    const horario = document.getElementById("horario").value;
    const idCaixa = document.getElementById("idCaixa").value;
    const idPet = document.getElementById("idPet").value;
    const idBanhista = document.getElementById("idBanhista").value;
    const idVenda = document.getElementById("idVenda") ? document.getElementById("idVenda").value.trim() : null;

    const statusMap = {
        "Agendado": true,
        "Concluído": true,
        "Cancelado": false
    };
    
    const statusValue = statusMap[status];

    const agendamentoData = {
        tipo,
        preco,
        status: statusValue,
        data: data,
        horario: horario,
        idCaixa: idCaixa,
        idPet: idPet,
        idBanhista: idBanhista,
        idVenda: idVenda,
    };

    try {
        const response = await fetch("http://localhost:8000/agendamentos/criar", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(agendamentoData),
        });

        if (response.ok) {
            document.getElementById("msg-sucesso").style.display = "block";
            document.getElementById("form-criar-agendamento").reset();  // Limpa o formulário
        } else {
            const errorData = await response.json();
            console.error("Erro:", errorData);
            document.getElementById("msg-erro").style.display = "block";
        }
    } catch (error) {
        console.error("Erro ao criar agendamento:", error);
        document.getElementById("msg-erro").style.display = "block";
    }
});