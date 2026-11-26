using OficinaMotos.Domain.Common;
using System.Collections.Generic;

namespace OficinaMotos.Domain.Entities
{
    public class Veiculo : BaseEntity
    {
        public long ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
        
        public long? MarcaId { get; set; }
        public VeiculoMarca? Marca { get; set; }

        public long? ModeloId { get; set; }
        public VeiculoModelo? Modelo { get; set; }
        
        public string? Codigo { get; set; }
        public string? Placa { get; set; }
        public string? Renavam { get; set; }
        public string? Chassi { get; set; }
        public short? AnoFabricacao { get; set; }
        public short? AnoModelo { get; set; }
        public string? Cor { get; set; }
        public string? Combustivel { get; set; }
        public int? Quilometragem { get; set; }
        public string? Observacoes { get; set; }
        public bool Ativo { get; set; } = true;
        

        public ICollection<OrdemServico> OrdensServico { get; set; } = new HashSet<OrdemServico>();
    }

    public class VeiculoMarca : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public ICollection<VeiculoModelo> Modelos { get; set; } = new HashSet<VeiculoModelo>();
    }

    public class VeiculoModelo : BaseEntity
    {
        public long MarcaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int? Cilindrada { get; set; }
        public bool Ativo { get; set; } = true;

        public VeiculoMarca? Marca { get; set; }
        public ICollection<Veiculo> Veiculos { get; set; } = new HashSet<Veiculo>();
    }
}
