using OficinaMotos.Domain.Common;
using OficinaMotos.Domain.Enums;
using System;
using System.Collections.Generic;

namespace OficinaMotos.Domain.Entities
{
    public class OrdemServico : BaseEntity
    {
        public long Numero { get; set; }
        public long ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public long? VeiculoId { get; set; }
        public Veiculo? Veiculo { get; set; }
        public string DescricaoProblema { get; set; } = string.Empty;
        public string? Diagnostico { get; set; }
        public OrdemServicoEnum Status { get; set; } = OrdemServicoEnum.Aberta;
        public OrdemServicoPrioridade Prioridade { get; set; } = OrdemServicoPrioridade.Normal;
        public DateTime DataAbertura { get; set; } = DateTime.UtcNow;
        public DateTime? PrevisaoEntrega { get; set; }
        public DateTime? DataConclusao { get; set; }
        public decimal ValorPecas { get; set; }
        public decimal ValorServicos { get; set; }
        public decimal Desconto { get; set; }
        public decimal ValorTotal { get; set; }
        public string? Observacoes { get; set; }
        

        public ICollection<OrdemServicoAnexo> Anexos { get; set; } = new HashSet<OrdemServicoAnexo>();
        public ICollection<OrdemServicoAvaliacao> Avaliacoes { get; set; } = new HashSet<OrdemServicoAvaliacao>();
        public ICollection<OrdemServicoChecklist> Checklists { get; set; } = new HashSet<OrdemServicoChecklist>();
        public ICollection<OrdemServicoItem> Itens { get; set; } = new HashSet<OrdemServicoItem>();
        public ICollection<OrdemServicoObservacao> ListaObservacoes { get; set; } = new HashSet<OrdemServicoObservacao>();
        public ICollection<OrdemServicoHistorico> Historico { get; set; } = new HashSet<OrdemServicoHistorico>();
        public ICollection<OrdemServicoPagamento> Pagamentos { get; set; } = new HashSet<OrdemServicoPagamento>();
        public ICollection<FinanceiroContaReceber> ContasReceber { get; set; } = new HashSet<FinanceiroContaReceber>();
    }

    public class OrdemServicoItem : BaseEntity
    {
        public long OrdemServicoId { get; set; }
        public OrdemServico? OrdemServico { get; set; }
        public long? PecaId { get; set; }
        public EstoquePeca? Peca { get; set; }
        public long? AtividadeProfissionalId { get; set; }
        public AtividadeProfissional? AtividadeProfissional { get; set; }
        public long? PrestadorId { get; set; }
        public Prestador? Prestador { get; set; }
        public OrdemServicoEnum Status { get; set; } = OrdemServicoEnum.Pendente;
        public OrdemServicoTipo Tipo { get; set; } = OrdemServicoTipo.Servico;
        public string Descricao { get; set; } = string.Empty;
        public decimal Quantidade { get; set; } = 1;
        public decimal ValorUnitario { get; set; }
        public decimal Desconto { get; set; }
        public decimal Total { get; set; } //=> Quantidade * ValorUnitario;
        public string? Observacoes { get; set; }
        
    }

    public class OrdemServicoAnexo : BaseEntity
    {
        public long OrdemServicoId { get; set; }
        public OrdemServico? OrdemServico { get; set; }
        public string Nome { get; set; } = string.Empty;
        public OrdemServicoAnexoTipo Tipo { get; set; } = OrdemServicoAnexoTipo.Imagem;
        public string Url { get; set; } = string.Empty;
        public string? Observacao { get; set; }
    }

    public class OrdemServicoChecklist : BaseEntity
    {
        public long OrdemServicoId { get; set; }
        public OrdemServico? OrdemServico { get; set; }
        public long? ResponsavelPrestadorId { get; set; }
        public Prestador? ResponsavelPrestador { get; set; }
        public string Item { get; set; } = string.Empty;
        public string? Categoria { get; set; }
        public string Status { get; set; } = "PENDENTE";
        public string? Observacao { get; set; }
    }

    public class OrdemServicoObservacao : BaseEntity
    {
        public long OrdemServicoId { get; set; }
        public OrdemServico? OrdemServico { get; set; }
        public long? UsuarioId { get; set; }
        public SegUsuario? Usuario { get; set; }
        public string Texto { get; set; } = string.Empty;
        public OrdemServicoObservacaoTipo Tipo { get; set; } = OrdemServicoObservacaoTipo.Interna;
    }

    public class OrdemServicoAvaliacao : BaseEntity
    {
        public long OrdemServicoId { get; set; }
        public OrdemServico? OrdemServico { get; set; }
        public long ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public byte Nota { get; set; }
        public string? Comentario { get; set; }
        public DateTime DataAvaliacao { get; set; } = DateTime.UtcNow;
    }

    public class OrdemServicoHistorico : BaseEntity
    {
        public long OrdemServicoId { get; set; }
        public OrdemServico? OrdemServico { get; set; }
        public long? UsuarioId { get; set; }
        public SegUsuario? Usuario { get; set; }
        public DateTime DataAlteracao { get; set; } = DateTime.UtcNow;
        public string? Campo { get; set; }
        public string? ValorAntigo { get; set; }
        public string? ValorNovo { get; set; }
    }

    public class OrdemServicoPagamento : BaseEntity
    {
        public long OrdemServicoId { get; set; }
        public OrdemServico? OrdemServico { get; set; }
        public decimal Valor { get; set; }
        public OrdemServicoPagamentoStatus Status { get; set; } = OrdemServicoPagamentoStatus.Pendente;
        public DateTime? DataPagamento { get; set; }
        public string? Metodo { get; set; }
        public string? Observacao { get; set; }
    }

}
