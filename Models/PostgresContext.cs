using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace prjGura.Models;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Fornecedor> Fornecedors { get; set; }

    public virtual DbSet<Funcionario> Funcionarios { get; set; }

    public virtual DbSet<Pet> Pets { get; set; }

    public virtual DbSet<Produto> Produtos { get; set; }

    public virtual DbSet<Produtosfornecido> Produtosfornecidos { get; set; }

    public virtual DbSet<Servico> Servicos { get; set; }

    public virtual DbSet<Vendadeproduto> Vendadeprodutos { get; set; }

    public virtual DbSet<Vendum> Venda { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pg_catalog", "adminpack");

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Cpf).HasName("cliente_pkey");

            entity.ToTable("cliente");

            entity.Property(e => e.Cpf)
                .HasMaxLength(20)
                .HasColumnName("cpf");
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .HasColumnName("nome");
            entity.Property(e => e.QuantidadePets).HasColumnName("quantidade_pets");
            entity.Property(e => e.Telefone)
                .HasMaxLength(255)
                .HasColumnName("telefone");
        });

        modelBuilder.Entity<Fornecedor>(entity =>
        {
            entity.HasKey(e => e.Cnpj).HasName("fornecedor_pkey");

            entity.ToTable("fornecedor");

            entity.Property(e => e.Cnpj)
                .HasMaxLength(40)
                .HasColumnName("cnpj");
            entity.Property(e => e.Endereco)
                .HasMaxLength(255)
                .HasColumnName("endereco");
            entity.Property(e => e.RazaoSocial)
                .HasMaxLength(255)
                .HasColumnName("razao_social");
            entity.Property(e => e.Telefone)
                .HasMaxLength(255)
                .HasColumnName("telefone");
        });

        modelBuilder.Entity<Funcionario>(entity =>
        {
            entity.HasKey(e => e.Cpf).HasName("funcionario_pkey");

            entity.ToTable("funcionario");

            entity.Property(e => e.Cpf)
                .HasMaxLength(20)
                .HasColumnName("cpf");
            entity.Property(e => e.Cargo)
                .HasMaxLength(255)
                .HasColumnName("cargo");
            entity.Property(e => e.Cpfsupervisiona)
                .HasMaxLength(20)
                .HasColumnName("cpfsupervisiona");
            entity.Property(e => e.Endereco)
                .HasMaxLength(255)
                .HasColumnName("endereco");
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .HasColumnName("nome");
            entity.Property(e => e.Salario)
                .HasPrecision(10, 2)
                .HasColumnName("salario");
            entity.Property(e => e.Telefone)
                .HasMaxLength(255)
                .HasColumnName("telefone");
            entity.Property(e => e.Turno)
                .HasMaxLength(10)
                .HasColumnName("turno");
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasKey(e => e.Idpet).HasName("pet_pkey");

            entity.ToTable("pet");

            entity.Property(e => e.Idpet)
                .ValueGeneratedNever()
                .HasColumnName("idpet");
            entity.Property(e => e.Idade).HasColumnName("idade");
            entity.Property(e => e.Idcliente)
                .HasMaxLength(20)
                .HasColumnName("idcliente");
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .HasColumnName("nome");
            entity.Property(e => e.Porte)
                .HasMaxLength(255)
                .HasColumnName("porte");
            entity.Property(e => e.Raca)
                .HasMaxLength(255)
                .HasColumnName("raca");

            entity.HasOne(d => d.IdclienteNavigation).WithMany(p => p.Pets)
                .HasForeignKey(d => d.Idcliente)
                .HasConstraintName("pet_idcliente_fkey");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("produto_pkey");

            entity.ToTable("produto");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(255)
                .HasColumnName("nome");
            entity.Property(e => e.PrecoVenda)
                .HasPrecision(10, 2)
                .HasColumnName("preco_venda");
        });

        modelBuilder.Entity<Produtosfornecido>(entity =>
        {
            entity.HasKey(e => new { e.Idproduto, e.Idfornecedor }).HasName("produtosfornecidos_pkey");

            entity.ToTable("produtosfornecidos");

            entity.Property(e => e.Idproduto).HasColumnName("idproduto");
            entity.Property(e => e.Idfornecedor)
                .HasMaxLength(40)
                .HasColumnName("idfornecedor");
            entity.Property(e => e.Data).HasColumnName("data");
            entity.Property(e => e.PrecoCusto)
                .HasPrecision(10, 2)
                .HasColumnName("preco_custo");
            entity.Property(e => e.Quantidade).HasColumnName("quantidade");

            entity.HasOne(d => d.IdfornecedorNavigation).WithMany(p => p.Produtosfornecidos)
                .HasForeignKey(d => d.Idfornecedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produtosfornecidos_idfornecedor_fkey");

            entity.HasOne(d => d.IdprodutoNavigation).WithMany(p => p.Produtosfornecidos)
                .HasForeignKey(d => d.Idproduto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produtosfornecidos_idproduto_fkey");
        });

        modelBuilder.Entity<Servico>(entity =>
        {
            entity.HasKey(e => e.Idservico).HasName("servico_pkey");

            entity.ToTable("servico");

            entity.Property(e => e.Idservico)
                .ValueGeneratedNever()
                .HasColumnName("idservico");
            entity.Property(e => e.Data).HasColumnName("data");
            entity.Property(e => e.Horario).HasColumnName("horario");
            entity.Property(e => e.Idbanhista)
                .HasMaxLength(20)
                .HasColumnName("idbanhista");
            entity.Property(e => e.Idcaixa)
                .HasMaxLength(20)
                .HasColumnName("idcaixa");
            entity.Property(e => e.Idpet).HasColumnName("idpet");
            entity.Property(e => e.Idvenda).HasColumnName("idvenda");
            entity.Property(e => e.Preco)
                .HasPrecision(10, 2)
                .HasColumnName("preco");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Tipo)
                .HasMaxLength(255)
                .HasColumnName("tipo");

            entity.HasOne(d => d.IdbanhistaNavigation).WithMany(p => p.ServicoIdbanhistaNavigations)
                .HasForeignKey(d => d.Idbanhista)
                .HasConstraintName("servico_idbanhista_fkey");

            entity.HasOne(d => d.IdcaixaNavigation).WithMany(p => p.ServicoIdcaixaNavigations)
                .HasForeignKey(d => d.Idcaixa)
                .HasConstraintName("servico_idcaixa_fkey");

            entity.HasOne(d => d.IdpetNavigation).WithMany(p => p.Servicos)
                .HasForeignKey(d => d.Idpet)
                .HasConstraintName("servico_idpet_fkey");

            entity.HasOne(d => d.IdvendaNavigation).WithMany(p => p.Servicos)
                .HasForeignKey(d => d.Idvenda)
                .HasConstraintName("servico_idvenda_fkey");
        });

        modelBuilder.Entity<Vendadeproduto>(entity =>
        {
            entity.HasKey(e => new { e.Idvenda, e.Idproduto }).HasName("vendadeproduto_pkey");

            entity.ToTable("vendadeproduto");

            entity.Property(e => e.Idvenda).HasColumnName("idvenda");
            entity.Property(e => e.Idproduto).HasColumnName("idproduto");
            entity.Property(e => e.Quantidadevendida).HasColumnName("quantidadevendida");
            entity.Property(e => e.Valortotalproduto)
                .HasPrecision(10, 2)
                .HasColumnName("valortotalproduto");

            entity.HasOne(d => d.IdprodutoNavigation).WithMany(p => p.Vendadeprodutos)
                .HasForeignKey(d => d.Idproduto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vendadeproduto_idproduto_fkey");

            entity.HasOne(d => d.IdvendaNavigation).WithMany(p => p.Vendadeprodutos)
                .HasForeignKey(d => d.Idvenda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vendadeproduto_idvenda_fkey");
        });

        modelBuilder.Entity<Vendum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("venda_pkey");

            entity.ToTable("venda");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Data).HasColumnName("data");
            entity.Property(e => e.Idcliente)
                .HasMaxLength(20)
                .HasColumnName("idcliente");
            entity.Property(e => e.Valortotal)
                .HasPrecision(10, 2)
                .HasColumnName("valortotal");

            entity.HasOne(d => d.IdclienteNavigation).WithMany(p => p.Venda)
                .HasForeignKey(d => d.Idcliente)
                .HasConstraintName("venda_idcliente_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
