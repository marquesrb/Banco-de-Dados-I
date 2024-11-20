from typing import List, Dict
from datetime import datetime
from servicos.database.conector import DatabaseManager

class AgendamentosDatabase:
    def __init__(self, db_provider=DatabaseManager()) -> None:
        self.db = db_provider

    def get_agendamentos_funcionario(self, funcionario_id: str, start_date: datetime, end_date: datetime) -> List[Dict]:
        """
        Retorna todos os agendamentos atribuídos a um funcionário dentro de um período de tempo.
        :param funcionario_id: ID do funcionário.
        :param start_date: Data inicial do período.
        :param end_date: Data final do período.
        :return: Lista de dicionários com os detalhes dos agendamentos.
        """
        query = """
        SELECT
            s.idServico,
            f.nome AS funcionario_nome,
            s.tipo,
            s.preco,
            s.data,
            s.horario
        FROM
            Servico s
        INNER JOIN
            Funcionario f ON s.idBanhista = f.cpf
        WHERE
            s.idBanhista = %s
            AND s.data BETWEEN %s AND %s
        ORDER BY
            s.data;
        """
        try:
            agendamentos = self.db.execute_select_special(query, (funcionario_id, start_date, end_date))
            for agendamento in agendamentos:
                if "horario" in agendamento and agendamento["horario"] is not None:
                    agendamento["horario"] = agendamento["horario"].strftime("%H:%M:%S")
            return agendamentos
        except Exception as e:
            print(f"Erro ao obter agendamentos do funcionário: {e}")
            return []
    
    def get_agendamento_historico_cliente(self, cliente_id: str) -> List[Dict]:
        """
        Retorna o histórico de agendamentos feitos por um cliente.
        :param cliente_id: ID do cliente.
        :return: Lista de dicionários com os detalhes dos agendamentos.
        """
        query = """
            SELECT 
            c.nome AS nome_cliente,
            p.nome AS nome_pet,
            s.tipo AS tipo_servico,
            s.data AS data_servico,
            s.horario AS horario_servico,
            CASE 
                WHEN s.status THEN 'Concluído' 
                ELSE 'Pendente' 
            END AS status_servico,
            s.preco AS preco_servico
        FROM 
            Cliente c
        JOIN 
            Pet p ON c.cpf = p.idCliente
        JOIN 
            Servico s ON p.idPet = s.idPet
        WHERE 
            c.cpf = %s
        ORDER BY 
            s.data DESC, s.horario DESC;
        """
        
        try:
            agendamentos = self.db.execute_select_special(query, (cliente_id,))
            for agendamento in agendamentos:
                if "data_servico" in agendamento and agendamento["data_servico"] is not None:
                    agendamento["data_servico"] = agendamento["data_servico"].strftime("%Y-%m-%d")
                if "horario_servico" in agendamento and agendamento["horario_servico"] is not None:
                    agendamento["horario_servico"] = agendamento["horario_servico"].strftime("%H:%M:%S")
            return agendamentos
        except Exception as e:
            print(f"Erro ao obter histórico de agendamentos do cliente: {e}")
            return []
