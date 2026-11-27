using OficinaMotos.Application.DTOs.Responses.Seguranca;
using AutoMapper;
using OficinaMotos.Application.DTOs.Requests;
using OficinaMotos.Application.DTOs.Requests.Cliente;
using OficinaMotos.Application.DTOs.Requests.Estoque;
using OficinaMotos.Application.DTOs.Requests.Financeiro;
using OficinaMotos.Application.DTOs.Requests.Fornecedor;
using OficinaMotos.Application.DTOs.Requests.Mecanico;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.DTOs.Requests.Pessoas;
using OficinaMotos.Application.DTOs.Requests.Prestador;
using OficinaMotos.Application.DTOs.Requests.Veiculo;
using OficinaMotos.Application.DTOs.Requests.Seguranca;
using OficinaMotos.Application.DTOs.Responses;
using OficinaMotos.Application.DTOs.Responses.Cliente;
using OficinaMotos.Application.DTOs.Responses.Estoque;
using OficinaMotos.Application.DTOs.Responses.Financeiro;
using OficinaMotos.Application.DTOs.Responses.Fornecedor;
using OficinaMotos.Application.DTOs.Responses.Mecanico;
using OficinaMotos.Application.DTOs.Responses.OrdemServico;
using OficinaMotos.Application.DTOs.Responses.Pessoas;
using OficinaMotos.Application.DTOs.Responses.Prestador;
using OficinaMotos.Application.DTOs.Responses.Veiculo;
using OficinaMotos.Domain.Entities;

namespace OficinaMotos.Application.Mappings
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            // Pessoas
            CreateMap<Pessoa, PessoaResponseDTO>();
            CreateMap<CreatePessoaDTO, Pessoa>();
            CreateMap<PessoaPf, PessoaPfResponseDTO>();
            CreateMap<PessoaPfDTO, PessoaPf>();
            CreateMap<CreatePessoaPfDTO, PessoaPf>();
            CreateMap<PessoaPj, PessoaPjResponseDTO>();
            CreateMap<PessoaPjDTO, PessoaPj>();
            CreateMap<CreatePessoaPjDTO, PessoaPj>();
            CreateMap<PessoaContato, PessoaContatoResponseDTO>();
            CreateMap<PessoaContatoDTO, PessoaContato>();
            CreateMap<CreatePessoaContatoDTO, PessoaContato>();
            CreateMap<PessoaEndereco, PessoaEnderecoResponseDTO>();
            CreateMap<PessoaEnderecoDTO, PessoaEndereco>();
            CreateMap<CreatePessoaEnderecoDTO, PessoaEndereco>();
            CreateMap<PessoaDocumento, PessoaDocumentoResponseDTO>();
            CreateMap<PessoaDocumentoDTO, PessoaDocumento>();
            CreateMap<CreatePessoaDocumentoDTO, PessoaDocumento>();
            CreateMap<PessoaAnexo, PessoaAnexoResponseDTO>();
            CreateMap<PessoaAnexoDTO, PessoaAnexo>();
            CreateMap<CreatePessoaAnexoDTO, PessoaAnexo>();

            CreateMap<Cliente, ClienteResponseDTO>();
                //.ForMember(dest => dest.Pessoa.TipoPessoa, opt => opt.MapFrom(src => src.Pessoa.TipoPessoa))
                //.ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
                //.ForMember(dest => dest.Origem.Descricao, opt => opt.MapFrom(src => src.Origem != null ? src.Origem.Nome : null));

            CreateMap<CreateClienteDTO, Cliente>();
            //.ForMember(dest => dest.Pessoa.NomeExibicao, opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.Pessoa.NomeExibicao) ? src.Pessoa.NomeExibicao : src.Pessoa.Nome))
            //.ForMember(dest => dest.Pessoa.Documento, opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.Documento) ? src.Documento : src.Pessoa.Cpf))
            //.ForMember(dest => dest.Pessoa.TipoPessoa, opt => opt.MapFrom(src => src.Pessoa.TipoPessoa == 2 ? ClienteTipo.Pessoa.PessoaJuridica : ClienteTipo.Pessoa.PessoaFisica))
            //.ForMember(dest => dest.Status, opt => opt.MapFrom(src => (ClienteStatus)src.Status))
            //.ForMember(dest => dest.Vip, opt => opt.MapFrom(src => src.Vip));

            CreateMap<Cliente, ClienteResponseTableDTO>();
                //.ForMember(dest => dest.TipoDescricao, opt => opt.MapFrom(src => (int)src.Pessoa.TipoPessoa == 1 ? "Pessoa Fsica" : "Pessoa Jurdica"))
                //.ForMember(dest => dest.StatusDescricao, opt => opt.MapFrom(src => (int)src.Status == 0 ? "Cliente Inativo" : GetStatus((int)src.Status))); 

            CreateMap<ClienteOrigem, ClienteOrigemResponseDTO>();
            CreateMap<CreateClienteOrigemDTO, ClienteOrigem>();


            CreateMap<ClienteFinanceiro, ClienteFinanceiroResponseDTO>();
            CreateMap<CreateClienteFinanceiroDTO, ClienteFinanceiro>();

            CreateMap<ClienteIndicacao, ClienteIndicacaoResponseDTO>();
            CreateMap<CreateClienteIndicacaoDTO, ClienteIndicacao>();

            CreateMap<ClienteLgpdConsentimento, ClienteLgpdConsentimentoResponseDTO>();
            CreateMap<CreateClienteLgpdConsentimentoDTO, ClienteLgpdConsentimento>();


            CreateMap<EstoqueCategoria, EstoqueCategoriaResponseDTO>();
            CreateMap<CreateEstoqueCategoriaDTO, EstoqueCategoria>();

            CreateMap<EstoqueFabricante, EstoqueFabricanteResponseDTO>();
            CreateMap<CreateEstoqueFabricanteDTO, EstoqueFabricante>();

            CreateMap<EstoqueLocalizacao, EstoqueLocalizacaoResponseDTO>();
            CreateMap<CreateEstoqueLocalizacaoDTO, EstoqueLocalizacao>();

            CreateMap<EstoquePeca, EstoquePecaResponseDTO>();
            CreateMap<CreateEstoquePecaDTO, EstoquePeca>();

            CreateMap<EstoqueMovimentacao, EstoqueMovimentacaoResponseDTO>();
            CreateMap<CreateEstoqueMovimentacaoDTO, EstoqueMovimentacao>();

            CreateMap<EstoquePecaAnexo, EstoquePecaAnexoResponseDTO>();
            CreateMap<CreateEstoquePecaAnexoDTO, EstoquePecaAnexo>();

            CreateMap<EstoquePecaFornecedor, EstoquePecaFornecedorResponseDTO>();
            CreateMap<CreateEstoquePecaFornecedorDTO, EstoquePecaFornecedor>();

            CreateMap<EstoquePecaHistorico, EstoquePecaHistoricoResponseDTO>();
            CreateMap<CreateEstoquePecaHistoricoDTO, EstoquePecaHistorico>();

            CreateMap<FinanceiroMetodoPagamento, FinanceiroMetodoPagamentoResponseDTO>();
            CreateMap<CreateFinanceiroMetodoPagamentoDTO, FinanceiroMetodoPagamento>();

            CreateMap<FinanceiroPagamento, FinanceiroPagamentoResponseDTO>();
            CreateMap<CreateFinanceiroPagamentoDTO, FinanceiroPagamento>();

            CreateMap<FinanceiroAnexo, FinanceiroAnexoResponseDTO>();
            CreateMap<CreateFinanceiroAnexoDTO, FinanceiroAnexo>();

            CreateMap<FinanceiroContaPagar, FinanceiroContaPagarResponseDTO>();
            CreateMap<CreateFinanceiroContaPagarDTO, FinanceiroContaPagar>();

            CreateMap<FinanceiroContaReceber, FinanceiroContaReceberResponseDTO>();
            CreateMap<CreateFinanceiroContaReceberDTO, FinanceiroContaReceber>();

            CreateMap<FinanceiroHistorico, FinanceiroHistoricoResponseDTO>();
            CreateMap<CreateFinanceiroHistoricoDTO, FinanceiroHistorico>();

            CreateMap<FinanceiroLancamento, FinanceiroLancamentoResponseDTO>();
            CreateMap<CreateFinanceiroLancamentoDTO, FinanceiroLancamento>();

            CreateMap<OrdemServico, OrdemServicoResponseDTO>();
                //.ForMember(dest => dest.ClienteNome, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nome : null))
                //.ForMember(dest => dest.ClienteNomeExibicao, opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.NomeExibicao : null))
                //.ForMember(dest => dest.VeiculoPlaca, opt => opt.MapFrom(src => src.Veiculo != null ? src.Veiculo.Placa : null))
                //.ForMember(dest => dest.VeiculoModeloNome, opt => opt.MapFrom(src => src.Veiculo != null && src.Veiculo.Modelo != null ? src.Veiculo.Modelo.Nome : null));
            CreateMap<CreateOrdemServicoDTO, OrdemServico>();

            CreateMap<OrdemServicoAnexo, OrdemServicoAnexoResponseDTO>();
            CreateMap<CreateOrdemServicoAnexoDTO, OrdemServicoAnexo>();

            CreateMap<OrdemServicoAvaliacao, OrdemServicoAvaliacaoResponseDTO>();
            CreateMap<CreateOrdemServicoAvaliacaoDTO, OrdemServicoAvaliacao>();

            CreateMap<OrdemServicoChecklist, OrdemServicoChecklistResponseDTO>();
            CreateMap<CreateOrdemServicoChecklistDTO, OrdemServicoChecklist>();

            CreateMap<OrdemServicoItem, OrdemServicoItemResponseDTO>();
            CreateMap<CreateOrdemServicoItemDTO, OrdemServicoItem>();

            CreateMap<OrdemServicoObservacao, OrdemServicoObservacaoResponseDTO>();
            CreateMap<CreateOrdemServicoObservacaoDTO, OrdemServicoObservacao>();

            CreateMap<OrdemServicoHistorico, OrdemServicoHistoricoResponseDTO>();
            CreateMap<CreateOrdemServicoHistoricoDTO, OrdemServicoHistorico>();

            CreateMap<OrdemServicoPagamento, OrdemServicoPagamentoResponseDTO>();
            CreateMap<CreateOrdemServicoPagamentoDTO, OrdemServicoPagamento>();

            CreateMap<Mecanico, MecanicoResponseDTO>();
            CreateMap<CreateMecanicoDTO, Mecanico>();

            // Prestador
            CreateMap<Prestador, PrestadorResponseDTO>();
            CreateMap<CreatePrestadorDTO, Prestador>();
            CreateMap<AtividadeProfissional, AtividadeProfissionalResponseDTO>();
            CreateMap<CreateAtividadeProfissionalDTO, AtividadeProfissional>();
            CreateMap<PrestadorAtividade, PrestadorAtividadeResponseDTO>()
                .ForMember(dest => dest.AtividadeNome, opt => opt.MapFrom(src => src.AtividadeProfissional != null ? src.AtividadeProfissional.Nome : null));
            CreateMap<CreatePrestadorAtividadeDTO, PrestadorAtividade>();
            CreateMap<PrestadorDisponibilidade, PrestadorDisponibilidadeResponseDTO>();
            CreateMap<CreatePrestadorDisponibilidadeDTO, PrestadorDisponibilidade>();
            CreateMap<PrestadorExperiencia, PrestadorExperienciaResponseDTO>();
            CreateMap<CreatePrestadorExperienciaDTO, PrestadorExperiencia>();
            CreateMap<PrestadorCertificacao, PrestadorCertificacaoResponseDTO>();
            CreateMap<CreatePrestadorCertificacaoDTO, PrestadorCertificacao>();


            CreateMap<Fornecedor, FornecedorResponseDTO>();
            CreateMap<CreateFornecedorDTO, Fornecedor>();

            CreateMap<FornecedorSegmento, FornecedorSegmentoResponseDTO>();
            CreateMap<CreateFornecedorSegmentoDTO, FornecedorSegmento>();

            CreateMap<FornecedorSegmentoRel, FornecedorSegmentoRelResponseDTO>();
            CreateMap<CreateFornecedorSegmentoRelDTO, FornecedorSegmentoRel>();


            CreateMap<FornecedorRepresentante, FornecedorRepresentanteResponseDTO>();
            CreateMap<CreateFornecedorRepresentanteDTO, FornecedorRepresentante>();

            CreateMap<FornecedorBanco, FornecedorBancoResponseDTO>();
            CreateMap<CreateFornecedorBancoDTO, FornecedorBanco>();

            CreateMap<FornecedorCertificacao, FornecedorCertificacaoResponseDTO>();
            CreateMap<CreateFornecedorCertificacaoDTO, FornecedorCertificacao>();

            CreateMap<FornecedorAvaliacao, FornecedorAvaliacaoResponseDTO>();
            CreateMap<CreateFornecedorAvaliacaoDTO, FornecedorAvaliacao>();

            CreateMap<Veiculo, VeiculoResponseDTO>();
            CreateMap<CreateVeiculoDTO, Veiculo>();

            CreateMap<VeiculoMarca, VeiculoMarcaResponseDTO>();
            CreateMap<CreateVeiculoMarcaDTO, VeiculoMarca>();

            CreateMap<VeiculoModelo, VeiculoModeloResponseDTO>();
            CreateMap<CreateVeiculoModeloDTO, VeiculoModelo>();

            // Segurança
            CreateMap<SegModulo, SegModuloResponseDTO>();
            CreateMap<CreateSegModuloDTO, SegModulo>();

            CreateMap<SegPerfil, SegPerfilResponseDTO>()
                .ForMember(dest => dest.Permissoes, opt => opt.MapFrom(src =>
                    src.PerfisPermissoes.Select(pp => pp.Permissao)));

            CreateMap<CreateSegPerfilDTO, SegPerfil>();

            CreateMap<SegPermissao, SegPermissaoResponseDTO>()
                .ForMember(dest => dest.NomeModulo, opt => opt.MapFrom(src => src.Modulo != null ? src.Modulo.Nome : string.Empty));
            CreateMap<CreateSegPermissaoDTO, SegPermissao>();

            CreateMap<SegUsuario, SegUsuarioResponseDTO>()
                .ForMember(dest => dest.Perfis, opt => opt.MapFrom(src =>
                    src.UsuariosPerfis.Select(up => up.Perfil)));
            CreateMap<SegUsuario, SegUsuarioTableDTO>();

            CreateMap<SegPerfil, SegPerfilResumoDTO>();

            CreateMap<SegAuditLog, SegAuditLogResponseDTO>();
        }

        private string GetStatus(int status)
        {
            string returnValue = "Desconhecido";

            switch (status)
            {
                case 1:
                    returnValue = "Cliente Ativo";
                    break;
                case 2:
                    returnValue = "Cliente Suspenso";
                    break;
                case 3:
                    returnValue = "Cliente Bloqueado";
                    break;
            }

            return returnValue;
        }
    }
}
