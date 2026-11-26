using OficinaMotos.Domain.Common;
using System;
using System.Collections.Generic;

namespace OficinaMotos.Domain.Entities
{
    public class Mecanico : BaseEntity
    {
        public long PrestadorId { get; set; }
        public Prestador? Prestador { get; set; }
        public string? Matricula { get; set; }
        public string? Nivel { get; set; }
        public DateTime? DataAdmissao { get; set; }
        public string? Observacoes { get; set; }
    }
}
