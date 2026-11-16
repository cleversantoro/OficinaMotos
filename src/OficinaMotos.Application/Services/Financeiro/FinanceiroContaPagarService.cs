using AutoMapper;
using OficinaMotos.Application.DTOs.Requests.Financeiro;
using OficinaMotos.Application.DTOs.Responses.Financeiro;
using OficinaMotos.Application.Interfaces.Financeiro;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Interfaces.Repositories.FinanceiroRepo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace OficinaMotos.Application.Services.Financeiro
{
    public class FinanceiroContaPagarService : IFinanceiroContaPagarService
    {
        private readonly IFinanceiroContaPagarRepository _repository;
        private readonly IMapper _mapper;
        private readonly IFinanceiroPagamentoRepository? _pagamentoRepository;

        public FinanceiroContaPagarService(
            IFinanceiroContaPagarRepository repository,
            IMapper mapper,
            IFinanceiroPagamentoRepository? pagamentoRepository = null)
        {
            _repository = repository;
            _mapper = mapper;
            _pagamentoRepository = pagamentoRepository;
        }

        public async Task<List<FinanceiroContaPagarResponseDTO>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            return _mapper.Map<List<FinanceiroContaPagarResponseDTO>>(items);
        }

        public async Task<FinanceiroContaPagarResponseDTO?> GetByIdAsync(long id)
        {
            var item = await _repository.GetByIdAsync(id);
            return _mapper.Map<FinanceiroContaPagarResponseDTO?>(item);
        }

        public async Task<FinanceiroContaPagarResponseDTO> CreateAsync(CreateFinanceiroContaPagarDTO request)
        {
            var entity = _mapper.Map<FinanceiroContaPagar>(request);
            var created = await _repository.AddAsync(entity);
            return _mapper.Map<FinanceiroContaPagarResponseDTO>(created);
        }

        public async Task<FinanceiroContaPagarResponseDTO?> UpdateAsync(long id, UpdateFinanceiroContaPagarDTO request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.FornecedorId = request.FornecedorId;
            entity.Descricao = request.Descricao;
            entity.Valor = request.Valor;
            entity.Vencimento = request.Vencimento;
            entity.Status = request.Status;
            entity.DataPagamento = request.DataPagamento;
            entity.MetodoId = request.MetodoId;
            entity.Observacao = request.Observacao;
            entity.SetUpdated();

            await _repository.UpdateAsync(entity);
            return _mapper.Map<FinanceiroContaPagarResponseDTO>(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            if (!await _repository.ExistsAsync(id)) return false;
            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task<ContaPagarPagamentoResponseDTO> RegistrarBaixaAsync(CreateContaPagarPagamentoDTO request)
        {
            var conta = await _repository.GetByIdAsync(request.ContaPagarId);
            if (conta == null)
            {
                throw new KeyNotFoundException("Conta a pagar nao encontrada.");
            }

            var statusAtual = (conta.Status ?? string.Empty).Trim().ToUpperInvariant();
            if (statusAtual == "PAGO" || statusAtual == "LIQUIDADO" || statusAtual == "CANCELADO")
            {
                throw new InvalidOperationException("Não é possível registrar baixa para uma conta a pagar já liquidada ou cancelada.");
            }

            var valorPago = decimal.Round(request.ValorPago, 2, MidpointRounding.AwayFromZero);
            var saldoAtual = decimal.Round(conta.Valor, 2, MidpointRounding.AwayFromZero);

            if (valorPago <= 0m)
            {
                throw new InvalidOperationException("O valor pago deve ser maior que zero.");
            }

            if (valorPago > saldoAtual)
            {
                throw new InvalidOperationException("O valor pago não pode exceder o saldo atual da conta a pagar.");
            }

            if (request.MetodoId <= 0)
            {
                throw new InvalidOperationException("A forma de pagamento é obrigatória.");
            }

            var dataPagamento = request.DataPagamento == default ? DateTime.UtcNow : request.DataPagamento;
            var saldoResidual = decimal.Round(saldoAtual - valorPago, 2, MidpointRounding.AwayFromZero);
            var baixaIntegral = saldoResidual == 0m;

            long pagamentoId = 0;
            if (_pagamentoRepository != null)
            {
                var pagamento = new FinanceiroPagamento
                {
                    FornecedorId = conta.FornecedorId,
                    Valor = valorPago,
                    Status = "PAGO",
                    DataPagamento = dataPagamento,
                    MetodoId = request.MetodoId,
                    Observacao = TruncateObservacao(
                        string.IsNullOrWhiteSpace(request.Observacao)
                            ? $"Baixa Conta a Pagar #{conta.Id} - {conta.Descricao}"
                            : $"{request.Observacao.Trim()} (Conta a Pagar #{conta.Id})")
                };
                var pagamentoCriado = await _pagamentoRepository.AddAsync(pagamento);
                pagamentoId = pagamentoCriado.Id;
            }

            conta.MetodoId = request.MetodoId;
            if (baixaIntegral)
            {
                conta.Status = "PAGO";
                conta.DataPagamento = dataPagamento;
                if (!string.IsNullOrWhiteSpace(request.Observacao))
                {
                    conta.Observacao = CombineObservacao(conta.Observacao, request.Observacao.Trim());
                }
            }
            else
            {
                conta.Valor = saldoResidual;
                conta.DataPagamento = null;
                var notaParcial = string.IsNullOrWhiteSpace(request.Observacao)
                    ? $"Baixa parcial R$ {valorPago.ToString("F2", CultureInfo.InvariantCulture)} ({dataPagamento:yyyy-MM-dd})"
                    : $"{request.Observacao.Trim()} | Baixa parcial R$ {valorPago.ToString("F2", CultureInfo.InvariantCulture)} ({dataPagamento:yyyy-MM-dd})";
                conta.Observacao = CombineObservacao(conta.Observacao, notaParcial);
            }

            conta.Fornecedor = null;
            conta.Metodo = null;
            conta.Anexos?.Clear();
            conta.SetUpdated();

            await _repository.UpdateAsync(conta);

            return new ContaPagarPagamentoResponseDTO
            {
                ContaPagarId = conta.Id,
                PagamentoId = pagamentoId,
                ValorPago = valorPago,
                SaldoResidual = saldoResidual,
                BaixaIntegral = baixaIntegral,
                Conta = _mapper.Map<FinanceiroContaPagarResponseDTO>(conta)
            };
        }

        private static string? CombineObservacao(string? atual, string nova)
        {
            if (string.IsNullOrWhiteSpace(atual))
            {
                return TruncateObservacao(nova);
            }
            return TruncateObservacao($"{atual.Trim()} | {nova}");
        }

        private static string? TruncateObservacao(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            var trimmed = value.Trim();
            return trimmed.Length <= 240 ? trimmed : trimmed.Substring(0, 240);
        }
    }
}
