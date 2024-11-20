import datetime
from servicos.database.conector import DatabaseManager

class FuncionariosDatabase:
    def __init__(self, db_provider=DatabaseManager()) -> None:
        self.db = db_provider
    
    def get_funcionario_ficha_horario(self, id_banhista):
        query = """
            SELECT 
                s.tipo,
                s.data,
                s.horario,
                p.nome AS nome_pet,
                c.nome AS nome_cliente
            FROM 
                Servico s
            INNER JOIN 
                Pet p ON s.idPet = p.idPet
            INNER JOIN 
                Cliente c ON p.idCliente = c.cpf
            WHERE 
                s.idBanhista = %s
            ORDER BY 
                s.data, s.horario;
                """
            
        try:
            results = self.db.execute_select_all(query, (id_banhista,))
            for row in results:
                row['data'] = row['data'].strftime("%Y-%m-%d")
                row['horario'] = row['horario'].strftime("%H:%M:%S")
            return results
        except Exception as e:
            print(f"Erro ao buscar ficha de horários: {e}")
            return []
                