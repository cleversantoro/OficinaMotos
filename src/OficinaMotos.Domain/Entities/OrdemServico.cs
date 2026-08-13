using OficinaMotos.Domain.Common;
using OficinaMotos.Domain.Enums;
using System;
using System.Collections.Generic;

namespace OficinaMotos.Domain.Entities
{
    public class OrdemServico : BaseEntity
    {
        public long ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        public long MecanicoId { get; set; }
        public Mecanico? Mecanico { get; set; }
        public long VeiculoId { get; set; }
        public Veiculo? Veiculo { get; set; }
        public string DescricaoProblema { get; set; } = string.Empty;
        public OrdemServicoStatus Status { get; set; } = OrdemServicoStatus.Aberta;
        public DateTime DataAbertura { get; set; } = DateTime.UtcNow;
        public DateTime? DataConclusao { get; set; }

        public ICollection<OrdemServicoAnexo> Anexos { get; set; } = new HashSet<OrdemServicoAnexo>();
        public ICollection<OrdemServicoAvaliacao> Avaliacoes { get; set; } = new HashSet<OrdemServicoAvaliacao>();
        public ICollection<OrdemServicoChecklist> Checklists { get; set; } = new HashSet<OrdemServicoChecklist>();
        public ICollection<OrdemServicoItem> Itens { get; set; } = new HashSet<OrdemServicoItem>();
        public ICollection<OrdemServicoObservacao> Observacoes { get; set; } = new HashSet<OrdemServicoObservacao>();
        public ICollection<OrdemServicoHistorico> Historico { get; set; } = new HashSet<OrdemServicoHistorico>();
        public ICollection<OrdemServicoPagamento> Pagamentos { get; set; } = new HashSet<OrdemServicoPagamento>();
        public ICollection<FinanceiroPagamento> PagamentosFinanceiros { get; set; } = new HashSet<FinanceiroPagamento>();
    }

    public class OrdemServicoAnexo : BaseEntity
    {
        public long OrdemServicoId { get; set; }
        public OrdemServico? OrdemServico { get; set; }
        public string? Nome { get; set; }
        public string? Tipo { get; set; }
        public string? Url { get; set; }
        public string? Observacao { get; set; }
        public DateTime? DataUpload { get; set; }
    }

    public class OrdemServicoAvaliacao : BaseEntity
    {
        public long OrdemServicoId { get; set; }
        public OrdemServico? OrdemServico { get; set; }
        public int Nota { get; set; }
        public string? Comentario { get; set; }
        public string? Usuario { get; set; }
    }

    public class OrdemServicoChecklist : BaseEntity
    {
        public long OrdemServicoId { get; set; }
        public OrdemServico? OrdemServico { get; set; }
        public string Item { get; set; } = string.Empty;
        public bool Realizado { get; set; }
        public string? Observacao { get; set; }
    }

    public class OrdemServicoItem : BaseEntity
    {
        public long OrdemServicoId { get; set; }
        public OrdemServico? OrdemServico { get; set; }
        public long? PecaId { get; set; }
        public EstoquePeca? Peca { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Total { get; set; } //=> Quantidade * ValorUnitario;
    }

    public class OrdemServicoObservacao : BaseEntity
    {
        public long OrdemServicoId { get; set; }
        public OrdemServico? OrdemServico { get; set; }
        public string? Usuario { get; set; }
        public string Texto { get; set; } = string.Empty;
    }

    public class OrdemServicoHistorico : BaseEntity
    {
        public long OrdemServicoId { get; set; }
        public OrdemServico? OrdemServico { get; set; }
        public DateTime DataAlteracao { get; set; } = DateTime.UtcNow;
        public string? Usuario { get; set; }
        public string? Campo { get; set; }
        public string? ValorAntigo { get; set; }
        public string? ValorNovo { get; set; }
    }

    public class OrdemServicoPagamento : BaseEntity
    {
        public long OrdemServicoId { get; set; }
        public OrdemServico? OrdemServico { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? DataPagamento { get; set; }
        public string? Metodo { get; set; }
        public string? Observacao { get; set; }
    }
}
