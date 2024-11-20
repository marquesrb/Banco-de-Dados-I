import datetime
from servicos.database.conector import DatabaseManager

class FornecedorDatabase:
    def __init__(self, db_provider=DatabaseManager()) -> None:
        self.db = db_provider
        
    def get_historico_produtos_fornecedor(self, fornecedor_id):
        query = """
             SELECT 
                f.cnpj AS fornecedor_id,
                f.razao_social AS fornecedor_nome,
                p.id AS produto_id,
                p.nome AS produto_nome,
                pf.quantidade AS quantidade_fornecida,
                date(pf.data) AS data_fornecimento
            FROM 
                ProdutosFornecidos pf
            JOIN 
                Fornecedor f  ON pf.idFornecedor  = f.cnpj
            JOIN
                Produto p ON pf.idProduto = p.id
            WHERE 
                f.cnpj = %s
            ORDER BY 
                pf.data DESC;
        """
        try:
            historico = self.db.execute_select_all(query, (fornecedor_id,))
            
            for item in historico:
                
                item["data_fornecimento"] = item["data_fornecimento"].strftime("%Y-%m-%d")
                
            return historico
        except Exception as e:
            print(f"Erro ao buscar histórico de produtos: {e}")
            return []
