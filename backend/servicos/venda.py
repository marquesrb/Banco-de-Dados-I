import datetime
from servicos.database.conector import DatabaseManager


class VendasDatabase:
    def __init__(self, db_provider=DatabaseManager()) -> None:
        self.db = db_provider

    def get_vendas(self, start_date: datetime.datetime, end_date: datetime.datetime, produto: str = None):
        query = """
        SELECT 
        v.id,
        v.data AS data_venda,
        c.nome AS nome_cliente,
        vp.quantidadevendida AS quantidade,
        COALESCE(vp.valorTotalProduto, 0) + COALESCE(s.preco, 0) AS valor_total_venda
    FROM 
        vendadeproduto AS vp
    JOIN 
        venda AS v ON vp.idvenda = v.id
    JOIN 
        produto AS p ON vp.idproduto = p.id
    JOIN 
        cliente AS c ON v.idcliente = c.cpf
    LEFT JOIN 
        servico AS s ON v.id = s.idVenda
    WHERE 
        v.data BETWEEN %s AND %s
        """
        
        params = [start_date, end_date]

        # Adiciona o filtro por produto, se fornecido
        if produto:
            query += " AND p.nome = %s"
            params.append(produto)
        
        query += " ORDER BY v.data;"
        
        return self.db.execute_select_special(query, tuple(params))
    

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
       