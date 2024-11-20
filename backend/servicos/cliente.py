import datetime
from servicos.database.conector import DatabaseManager

class ClientesDatabase:
    def __init__(self, db_provider=DatabaseManager()) -> None:
        self.db = db_provider
    
    def get_cliente_vencido(self):
        query = """
            SELECT DISTINCT 
            c.cpf AS cpf_cliente,
            c.nome AS nome_cliente,
            c.telefone AS telefone_cliente,
            p.idPet AS id_pet,
            p.nome AS nome_pet
        FROM 
            Cliente c
        JOIN 
            Pet p ON c.cpf = p.idCliente
        JOIN 
            Servico s ON p.idPet = s.idPet
        WHERE 
            s.status = FALSE -- Considerando que `FALSE` representa uma tosa vencida ou não realizada
            AND s.data < CURRENT_DATE;
        """
        
        try:
            clientes = self.db.execute_select_all(query)
            return clientes
        except Exception as e:
            print(f"Erro ao listar clientes: {e}")
            return []