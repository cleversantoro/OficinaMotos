using OficinaMotos.Application.DTOs.Requests.Pessoas;
using OficinaMotos.Application.DTOs.Responses.Pessoas;
using OficinaMotos.Application.Interfaces.Pessoas;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Exceptions;
using OficinaMotos.Domain.Interfaces.Repositories;

namespace OficinaMotos.Application.Services.Pessoas
{
    public class PessoaService : IPessoaService
    {
        private readonly IPessoaRepository _repository;

        public PessoaService(IPessoaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PessoaResponseDTO>> GetAllAsync()
        {
            var pessoas = await _repository.GetAllAsync();
            return pessoas.Select(Map).ToList();
        }

        public async Task<PessoaResponseDTO?> GetByIdAsync(long id)
        {
            var pessoa = await _repository.GetByIdAsync(id);
            return pessoa == null ? null : Map(pessoa);
        }

        public async Task<PessoaResponseDTO> CreateAsync(CreatePessoaDTO request)
        {
            var documento = NormalizeDocument(request.Documento);
            var tipo = request.TipoPessoa.Trim().ToUpperInvariant();

            if (tipo is not ("PF" or "PJ"))
                throw new DomainException("Tipo de pessoa deve ser PF ou PJ.");

            if (!ValidarDocumento(tipo, documento))
                throw new DomainException(tipo == "PF" ? "CPF inválido." : "CNPJ inválido.");

            var existente = await _repository.GetByDocumentoAsync(documento);
            if (existente != null)
                throw new DomainException($"Já existe uma pessoa cadastrada com este { (tipo == "PF" ? "CPF" : "CNPJ") }.");

            var pessoa = new Pessoa
            {
                Codigo = await _repository.GetNextCodigoAsync(),
                TipoPessoa = tipo,
                Documento = documento,
                Nome = request.Nome.Trim(),
                NomeExibicao = string.IsNullOrWhiteSpace(request.NomeExibicao) ? request.Nome.Trim() : request.NomeExibicao.Trim(),
                Ativo = request.Ativo,
                Observacoes = request.Observacoes
            };

            if (tipo == "PF" && request.PessoaFisica != null)
            {
                pessoa.PessoaFisica = new PessoaPf
                {
                    Rg = request.PessoaFisica.Rg,
                    DataNascimento = request.PessoaFisica.DataNascimento,
                    Genero = request.PessoaFisica.Genero,
                    EstadoCivil = request.PessoaFisica.EstadoCivil,
                    Profissao = request.PessoaFisica.Profissao
                };
            }

            if (tipo == "PJ")
            {
                if (request.PessoaJuridica == null || string.IsNullOrWhiteSpace(request.PessoaJuridica.RazaoSocial))
                    throw new DomainException("Razão social é obrigatória para pessoa jurídica.");

                pessoa.PessoaJuridica = new PessoaPj
                {
                    RazaoSocial = request.PessoaJuridica.RazaoSocial.Trim(),
                    NomeFantasia = request.PessoaJuridica.NomeFantasia,
                    InscricaoEstadual = request.PessoaJuridica.InscricaoEstadual,
                    InscricaoMunicipal = request.PessoaJuridica.InscricaoMunicipal,
                    Responsavel = request.PessoaJuridica.Responsavel,
                    Website = request.PessoaJuridica.Website
                };
            }

            MapCollections(request, pessoa);
            var criada = await _repository.AddAsync(pessoa);
            return Map(criada);
        }

        public async Task<PessoaResponseDTO?> UpdateAsync(long id, UpdatePessoaDTO request)
        {
            var pessoa = await _repository.GetByIdAsync(id);
            if (pessoa == null) return null;

            var documento = NormalizeDocument(request.Documento);
            var tipo = request.TipoPessoa.Trim().ToUpperInvariant();
            if (tipo is not ("PF" or "PJ"))
                throw new DomainException("Tipo de pessoa deve ser PF ou PJ.");
            if (!ValidarDocumento(tipo, documento))
                throw new DomainException(tipo == "PF" ? "CPF inválido." : "CNPJ inválido.");

            var existente = await _repository.GetByDocumentoAsync(documento);
            if (existente != null && existente.Id != id)
                throw new DomainException($"Já existe outra pessoa cadastrada com este { (tipo == "PF" ? "CPF" : "CNPJ") }.");

            pessoa.TipoPessoa = tipo;
            pessoa.Documento = documento;
            pessoa.Nome = request.Nome.Trim();
            pessoa.NomeExibicao = string.IsNullOrWhiteSpace(request.NomeExibicao) ? pessoa.Nome : request.NomeExibicao.Trim();
            pessoa.Ativo = request.Ativo;
            pessoa.Observacoes = request.Observacoes;
            pessoa.SetUpdated();

            pessoa.PessoaFisica = tipo == "PF" && request.PessoaFisica != null
                ? new PessoaPf
                {
                    PessoaId = pessoa.Id,
                    Rg = request.PessoaFisica.Rg,
                    DataNascimento = request.PessoaFisica.DataNascimento,
                    Genero = request.PessoaFisica.Genero,
                    EstadoCivil = request.PessoaFisica.EstadoCivil,
                    Profissao = request.PessoaFisica.Profissao
                }
                : null;

            pessoa.PessoaJuridica = tipo == "PJ" && request.PessoaJuridica != null
                ? new PessoaPj
                {
                    PessoaId = pessoa.Id,
                    RazaoSocial = request.PessoaJuridica.RazaoSocial.Trim(),
                    NomeFantasia = request.PessoaJuridica.NomeFantasia,
                    InscricaoEstadual = request.PessoaJuridica.InscricaoEstadual,
                    InscricaoMunicipal = request.PessoaJuridica.InscricaoMunicipal,
                    Responsavel = request.PessoaJuridica.Responsavel,
                    Website = request.PessoaJuridica.Website
                }
                : null;

            await _repository.UpdateAsync(pessoa);
            return Map(pessoa);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }

        private static void MapCollections(CreatePessoaDTO request, Pessoa pessoa)
        {
            pessoa.Contatos = request.Contatos.Select(x => new PessoaContato
            {
                Tipo = x.Tipo,
                Valor = x.Valor,
                Principal = x.Principal,
                Observacao = x.Observacao
            }).ToHashSet();

            pessoa.Enderecos = request.Enderecos.Select(x => new PessoaEndereco
            {
                Tipo = x.Tipo,
                Cep = x.Cep,
                Logradouro = x.Logradouro,
                Numero = x.Numero,
                Bairro = x.Bairro,
                Cidade = x.Cidade,
                Estado = x.Estado,
                Pais = x.Pais,
                Complemento = x.Complemento,
                Principal = x.Principal,
                Observacao = x.Observacao
            }).ToHashSet();

            pessoa.Documentos = request.Documentos.Select(x => new PessoaDocumento
            {
                Tipo = x.Tipo,
                Numero = x.Numero,
                DataEmissao = x.DataEmissao,
                DataValidade = x.DataValidade,
                OrgaoExpedidor = x.OrgaoExpedidor,
                ArquivoUrl = x.ArquivoUrl,
                Observacoes = x.Observacoes
            }).ToHashSet();

            pessoa.Anexos = request.Anexos.Select(x => new PessoaAnexo
            {
                Nome = x.Nome,
                Tipo = x.Tipo,
                Url = x.Url,
                Observacao = x.Observacao,
                TamanhoBytes = x.TamanhoBytes,
                MimeType = x.MimeType
            }).ToHashSet();
        }

        private static PessoaResponseDTO Map(Pessoa pessoa)
        {
            return new PessoaResponseDTO
            {
                Id = pessoa.Id,
                Codigo = pessoa.Codigo,
                TipoPessoa = pessoa.TipoPessoa,
                Documento = pessoa.Documento,
                Nome = pessoa.Nome,
                NomeExibicao = pessoa.NomeExibicao,
                Ativo = pessoa.Ativo,
                Observacoes = pessoa.Observacoes,
                PessoaFisica = pessoa.PessoaFisica == null ? null : new PessoaPfResponseDTO
                {
                    Rg = pessoa.PessoaFisica.Rg,
                    DataNascimento = pessoa.PessoaFisica.DataNascimento,
                    Genero = pessoa.PessoaFisica.Genero,
                    EstadoCivil = pessoa.PessoaFisica.EstadoCivil,
                    Profissao = pessoa.PessoaFisica.Profissao
                },
                PessoaJuridica = pessoa.PessoaJuridica == null ? null : new PessoaPjResponseDTO
                {
                    RazaoSocial = pessoa.PessoaJuridica.RazaoSocial,
                    NomeFantasia = pessoa.PessoaJuridica.NomeFantasia,
                    InscricaoEstadual = pessoa.PessoaJuridica.InscricaoEstadual,
                    InscricaoMunicipal = pessoa.PessoaJuridica.InscricaoMunicipal,
                    Responsavel = pessoa.PessoaJuridica.Responsavel,
                    Website = pessoa.PessoaJuridica.Website
                },
                Contatos = pessoa.Contatos.Select(x => new PessoaContatoResponseDTO { Id = x.Id, Tipo = x.Tipo, Valor = x.Valor, Principal = x.Principal, Observacao = x.Observacao }).ToList(),
                Enderecos = pessoa.Enderecos.Select(x => new PessoaEnderecoResponseDTO { Id = x.Id, Tipo = x.Tipo, Cep = x.Cep, Logradouro = x.Logradouro, Numero = x.Numero, Bairro = x.Bairro, Cidade = x.Cidade, Estado = x.Estado, Pais = x.Pais, Complemento = x.Complemento, Principal = x.Principal, Observacao = x.Observacao }).ToList(),
                Documentos = pessoa.Documentos.Select(x => new PessoaDocumentoResponseDTO { Id = x.Id, Tipo = x.Tipo, Numero = x.Numero, DataEmissao = x.DataEmissao, DataValidade = x.DataValidade, OrgaoExpedidor = x.OrgaoExpedidor, ArquivoUrl = x.ArquivoUrl, Observacoes = x.Observacoes }).ToList(),
                Anexos = pessoa.Anexos.Select(x => new PessoaAnexoResponseDTO { Id = x.Id, Nome = x.Nome, Tipo = x.Tipo, Url = x.Url, Observacao = x.Observacao, TamanhoBytes = x.TamanhoBytes, MimeType = x.MimeType }).ToList(),
                CreatedAt = pessoa.CreatedAt,
                UpdatedAt = pessoa.UpdatedAt
            };
        }

        private static string NormalizeDocument(string document)
        {
            return new string(document.Where(char.IsDigit).ToArray());
        }

        private static bool ValidarDocumento(string tipo, string documento)
        {
            if (tipo == "PF") return ValidarCpf(documento);
            if (tipo == "PJ") return ValidarCnpj(documento);
            return false;
        }

        private static bool ValidarCpf(string cpf)
        {
            if (cpf.Length != 11 || cpf.Distinct().Count() == 1) return false;
            var digits = cpf.Select(x => x - '0').ToArray();
            var first = digits.Take(9).Select((value, index) => value * (10 - index)).Sum() % 11;
            first = first < 2 ? 0 : 11 - first;
            if (digits[9] != first) return false;
            var second = digits.Take(10).Select((value, index) => value * (11 - index)).Sum() % 11;
            second = second < 2 ? 0 : 11 - second;
            return digits[10] == second;
        }

        private static bool ValidarCnpj(string cnpj)
        {
            if (cnpj.Length != 14 || cnpj.Distinct().Count() == 1) return false;
            var digits = cnpj.Select(x => x - '0').ToArray();
            int[] weightsFirst = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] weightsSecond = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var first = digits.Take(12).Select((value, index) => value * weightsFirst[index]).Sum() % 11;
            first = first < 2 ? 0 : 11 - first;
            if (digits[12] != first) return false;
            var second = digits.Take(13).Select((value, index) => value * weightsSecond[index]).Sum() % 11;
            second = second < 2 ? 0 : 11 - second;
            return digits[13] == second;
        }
    }
}
