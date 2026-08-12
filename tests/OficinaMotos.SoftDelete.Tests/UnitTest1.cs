using Microsoft.EntityFrameworkCore;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.DTOs.Responses.OrdemServicoDTO;
using OficinaMotos.Domain.Common;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Enums;
using OficinaMotos.Infrastructure.Context;
using OficinaMotos.Infrastructure.Repositories;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OficinaMotos.SoftDelete.Tests;

public class SoftDeleteTests
{
    [Fact]
    public void BaseEntity_Delete_ShouldMarkEntityAsSoftDeleted()
    {
        var entity = new FakeEntity();

        entity.Delete();

        Assert.True(entity.IsDeleted);
        Assert.NotNull(entity.DeletedAt);
        Assert.NotNull(entity.UpdatedAt);
    }

    [Fact]
    public async Task Repository_DeleteAsync_ShouldSoftDeleteWithoutHardDelete()
    {
        await using var context = CreateContext();
        var repository = new Repository<ClienteOrigem>(context);

        var entity = new ClienteOrigem { Nome = "Origem Teste" };
        await repository.AddAsync(entity);

        var id = entity.Id;
        await repository.DeleteAsync(id);

        var activeLookup = await repository.GetByIdAsync(id);
        Assert.Null(activeLookup);

        var deletedEntity = await context.Set<ClienteOrigem>()
            .IgnoreQueryFilters()
            .SingleAsync(x => x.Id == id);

        Assert.True(deletedEntity.IsDeleted);
        Assert.NotNull(deletedEntity.DeletedAt);
    }

    [Fact]
    public async Task Repository_GetAllAndFind_ShouldIgnoreSoftDeletedRows()
    {
        await using var context = CreateContext();
        var repository = new Repository<ClienteOrigem>(context);

        var active = await repository.AddAsync(new ClienteOrigem { Nome = "Ativa" });
        var toDelete = await repository.AddAsync(new ClienteOrigem { Nome = "Excluir" });

        await repository.SoftDeleteAsync(toDelete.Id);

        var all = await repository.GetAllAsync();
        var found = await repository.FindAsync(_ => true);

        Assert.Single(all);
        Assert.Single(found);
        Assert.Equal(active.Id, all[0].Id);
        Assert.Equal(active.Id, found[0].Id);
    }

    [Fact]
    public async Task Repository_DeleteAsync_ShouldWorkForThreeEntityFlows()
    {
        await using var context = CreateContext();

        var clienteOrigemRepository = new Repository<ClienteOrigem>(context);
        var veiculoMarcaRepository = new Repository<VeiculoMarca>(context);
        var fornecedorSegmentoRepository = new Repository<FornecedorSegmento>(context);

        var origem = await clienteOrigemRepository.AddAsync(new ClienteOrigem { Nome = "Web" });
        var marca = await veiculoMarcaRepository.AddAsync(new VeiculoMarca { Nome = "Marca X" });
        var segmento = await fornecedorSegmentoRepository.AddAsync(new FornecedorSegmento { Codigo = "S001", Nome = "Moto Peças" });

        await clienteOrigemRepository.DeleteAsync(origem.Id);
        await veiculoMarcaRepository.DeleteAsync(marca.Id);
        await fornecedorSegmentoRepository.DeleteAsync(segmento.Id);

        Assert.Null(await clienteOrigemRepository.GetByIdAsync(origem.Id));
        Assert.Null(await veiculoMarcaRepository.GetByIdAsync(marca.Id));
        Assert.Null(await fornecedorSegmentoRepository.GetByIdAsync(segmento.Id));
    }

    [Fact]
    public void NewEntity_ShouldStartAsNotDeleted()
    {
        var entity = new ClienteOrigem { Nome = "Nova" };

        Assert.False(entity.IsDeleted);
        Assert.Null(entity.DeletedAt);
    }

    [Fact]
    public async Task AddedEntity_ShouldPersistWithIsDeletedFalse()
    {
        await using var context = CreateContext();
        var repository = new Repository<ClienteOrigem>(context);

        var entity = await repository.AddAsync(new ClienteOrigem { Nome = "Persistida" });

        var persisted = await context.Set<ClienteOrigem>()
            .IgnoreQueryFilters()
            .SingleAsync(x => x.Id == entity.Id);

        Assert.False(persisted.IsDeleted);
        Assert.Null(persisted.DeletedAt);
    }

    [Fact]
    public void DbContext_ShouldApplyGlobalFilterToAllBaseEntities()
    {
        using var context = CreateContext();

        var derivedTypes = context.Model.GetEntityTypes()
            .Where(t => t.ClrType != null && typeof(BaseEntity).IsAssignableFrom(t.ClrType));

        Assert.NotEmpty(derivedTypes);
        Assert.All(derivedTypes, type => Assert.NotNull(type.GetQueryFilter()));
    }

    [Fact]
    public void OrdemServicoStatus_ShouldHaveExpectedValues()
    {
        Assert.Equal(1, (int)OrdemServicoStatus.Aberta);
        Assert.Equal(2, (int)OrdemServicoStatus.EmAndamento);
        Assert.Equal(3, (int)OrdemServicoStatus.AguardandoPeca);
        Assert.Equal(4, (int)OrdemServicoStatus.Concluida);
        Assert.Equal(5, (int)OrdemServicoStatus.Cancelada);
    }

    [Fact]
    public void OrdemServico_ShouldStartWithDefaultStatusEnum()
    {
        var ordem = new OrdemServico
        {
            ClienteId = 1,
            MecanicoId = 1,
            DescricaoProblema = "Teste"
        };

        Assert.Equal(OrdemServicoStatus.Aberta, ordem.Status);
    }

    [Fact]
    public void CreateOrdemServicoDto_ShouldUseEnumStatusContract()
    {
        var dto = new CreateOrdemServicoDTO
        {
            ClienteId = 1,
            MecanicoId = 1,
            DescricaoProblema = "Teste",
            Status = OrdemServicoStatus.EmAndamento
        };

        Assert.Equal(OrdemServicoStatus.EmAndamento, dto.Status);
    }

    [Fact]
    public void OrdemServicoJson_ShouldSerializeAsTextStatus()
    {
        var dto = new OrdemServicoResponseDTO
        {
            Id = 1,
            ClienteId = 10,
            MecanicoId = 20,
            DescricaoProblema = "Teste de status",
            Status = OrdemServicoStatus.AguardandoPeca,
            DataAbertura = DateTime.UtcNow
        };

        var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        });

        Assert.Contains("\"Status\":\"AguardandoPeca\"", json);
        Assert.DoesNotContain("\"Status\":3", json);
    }

    [Fact]
    public void OrdemServicoJson_ShouldDeserializeFromTextStatus()
    {
        const string json = "{\"ClienteId\":1,\"MecanicoId\":2,\"DescricaoProblema\":\"Teste\",\"Status\":\"Concluida\"}";

        var dto = JsonSerializer.Deserialize<CreateOrdemServicoDTO>(json, new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        });

        Assert.NotNull(dto);
        Assert.Equal(OrdemServicoStatus.Concluida, dto!.Status);
    }

    [Theory]
    [InlineData("ABERTA", OrdemServicoStatus.Aberta)]
    [InlineData("EM_ANDAMENTO", OrdemServicoStatus.EmAndamento)]
    [InlineData("AGUARDANDO_PECA", OrdemServicoStatus.AguardandoPeca)]
    [InlineData("AGUARDANDO_APROVACAO", OrdemServicoStatus.AguardandoPeca)]
    [InlineData("CONCLUIDA", OrdemServicoStatus.Concluida)]
    [InlineData("CANCELADA", OrdemServicoStatus.Cancelada)]
    public void OrdemServicoStatus_ShouldParseLegacyDatabaseValues(string value, OrdemServicoStatus expected)
    {
        Assert.Equal(expected, OrdemServicoStatusExtensions.ParseLegacy(value));
    }

    private static OficinaContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<OficinaContext>()
            .UseInMemoryDatabase($"soft-delete-tests-{Guid.NewGuid()}")
            .Options;

        return new OficinaContext(options);
    }

    private sealed class FakeEntity : BaseEntity
    {
    }
}
