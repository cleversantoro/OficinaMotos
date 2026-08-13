using OficinaMotos.Domain.Common;
using System.Collections.Generic;

namespace OficinaMotos.Domain.Entities
{
    public class Veiculo : BaseEntity
    {
        public long ClienteId { get; set; }
        public string Placa { get; set; } = string.Empty;
        public long? ModeloId { get; set; }
        public int? AnoFab { get; set; }
        public int? AnoMod { get; set; }
        public string? Cor { get; set; }
        public string? Chassi { get; set; }
        public string? Renavam { get; set; }
        public string? Km { get; set; }
        public string? Combustivel { get; set; }
        public string? Observacao { get; set; }
        public bool Principal { get; set; }
        public bool Ativo { get; set; } = true;

        public Cliente? Cliente { get; set; }
        public VeiculoModelo? Modelo { get; set; }
        public ICollection<OrdemServico> OrdensServico { get; set; } = new HashSet<OrdemServico>();
    }

    public class VeiculoMarca : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string? Pais { get; set; }

        public ICollection<VeiculoModelo> Modelos { get; set; } = new HashSet<VeiculoModelo>();
    }

    public class VeiculoModelo : BaseEntity
    {
        public long MarcaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int? AnoInicio { get; set; }
        public int? AnoFim { get; set; }

        public VeiculoMarca? Marca { get; set; }
        public ICollection<Veiculo> Veiculos { get; set; } = new HashSet<Veiculo>();
    }
}
