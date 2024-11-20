import datetime
from servicos.database.conector import DatabaseManager


class VendasDatabase:
    def __init__(self, db_provider=DatabaseManager()) -> None:
        self.db = db_provider

    def get_vendas(self, start_date: datetime.datetime, end_date: datetime.datetime):
        query = """
        
        SELECT 
            v.data AS data_venda,
            c.nome AS nome_cliente,
            v.valorTotal AS valor_total_venda
        FROM 
            Venda v
        JOIN 
            Cliente c ON v.idCliente = c.cpf
        WHERE 
            v.data BETWEEN %s AND %s
        ORDER BY 
            v.data;

        """
        return self.db.execute_select_special(query, (start_date, end_date))
    

    def get_detalhes_venda(self, id_venda: int):
        query = """
        SELECT 
            v.id AS id_venda,
            v.data AS data_venda,
            c.nome AS nome_cliente,
            p.nome AS nome_produto,
            vp.quantidadeVendida AS quantidade_produto,
            vp.valorTotalProduto AS valor_produto,
            s.tipo AS tipo_servico,
            s.preco AS preco_servico,
            f.nome AS nome_caixa
        FROM 
            Venda v
        JOIN 
            Cliente c ON v.idCliente = c.cpf
        LEFT JOIN 
            VendaDeProduto vp ON v.id = vp.idVenda
        LEFT JOIN 
            Produto p ON vp.idProduto = p.id
        LEFT JOIN 
            Servico s ON v.id = s.idVenda
        LEFT JOIN 
            Funcionario f ON s.idCaixa = f.cpf
        WHERE 
            v.id = %s;
        """
        return self.db.execute_select_one(query, (id_venda,))
       