using Microsoft.EntityFrameworkCore;
using OficinaMotos.Application.DTOs.Requests.OrdemServico;
using OficinaMotos.Application.DTOs.Responses.OrdemServicoDTO;
using OficinaMotos.Domain.Common;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Domain.Enums;
using OficinaMotos.Infrastructure.Repositories;
using OficinaMotos.Tests.Helpers;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace OficinaMotos.Tests.Domain
{
    public class SoftDeleteAndOrdemServicoContractTests
    {
        [Fact]
        public void BaseEntity_Delete_ShouldMarkEntityAsSoftDeleted()
        {
            var entity = new FakeEntity();

            entity.Delete();

            Assert.NotNull(entity.DeletedAt);
            Assert.NotNull(entity.UpdatedAt);
        }

        [Fact]
        public async Task Repository_DeleteAsync_ShouldSoftDeleteWithoutHardDelete()
        {
            await using var context = TestDbContextFactory.Create();
            var repository = new Repository<SegPerfil>(context);

            var entity = new SegPerfil { Nome = "Perfil Teste" };
            await repository.AddAsync(entity);

            var id = entity.Id;
            await repository.DeleteAsync(id);

            var activeLookup = await repository.GetByIdAsync(id);
            Assert.Null(activeLookup);

            var retainedEntity = await context.Set<SegPerfil>()
                .IgnoreQueryFilters()
                .SingleAsync(x => x.Id == id);
            Assert.NotNull(retainedEntity.DeletedAt);
        }

        [Fact]
        public async Task Repository_GetAllAndFind_ShouldIgnoreSoftDeletedRows()
        {
            await using var context = TestDbContextFactory.Create();
            var repository = new Repository<SegPerfil>(context);

            var active = await repository.AddAsync(new SegPerfil { Nome = "Ativa" });
            var toDelete = await repository.AddAsync(new SegPerfil { Nome = "Excluir" });

            await repository.SoftDeleteAsync(toDelete.Id);

            var all = await repository.GetAllAsync();
            var found = await repository.FindAsync(_ => true);

            Assert.Single(all);
            Assert.Single(found);
            Assert.Equal(active.Id, all[0].Id);
            Assert.Equal(active.Id, found[0].Id);
        }

        [Fact]
        public async Task Repository_DeleteAsync_ShouldRejectEntitiesWithoutDeletedAtColumn()
        {
            await using var context = TestDbContextFactory.Create();
            var repository = new Repository<ClienteOrigem>(context);
            var entity = await repository.AddAsync(new ClienteOrigem { Nome = "Web" });

            await Assert.ThrowsAsync<NotSupportedException>(() => repository.DeleteAsync(entity.Id));
            Assert.NotNull(await repository.GetByIdAsync(entity.Id));
        }

        [Fact]
        public void NewEntity_ShouldHaveNoDeletionTimestamp()
        {
            var entity = new ClienteOrigem { Nome = "Nova" };

            Assert.Null(entity.DeletedAt);
        }

        [Fact]
        public async Task AddedEntity_ShouldPersistWithNoDeletionTimestamp()
        {
            await using var context = TestDbContextFactory.Create();
            var repository = new Repository<SegPerfil>(context);

            var entity = await repository.AddAsync(new SegPerfil { Nome = "Persistida" });

            var persisted = await context.Set<SegPerfil>()
                .IgnoreQueryFilters()
                .SingleAsync(x => x.Id == entity.Id);

            Assert.Null(persisted.DeletedAt);
        }

        [Fact]
        public void DbContext_ShouldMapBaseTimestampsAndFilterOnlySoftDeleteTables()
        {
            using var context = TestDbContextFactory.Create();

            foreach (var entityType in context.Model.GetEntityTypes()
                         .Where(type => type.ClrType != null && typeof(BaseEntity).IsAssignableFrom(type.ClrType)))
            {
                Assert.Null(entityType.FindProperty("IsDeleted"));

                var createdAt = entityType.FindProperty(nameof(BaseEntity.CreatedAt));
                if (createdAt != null)
                {
                    Assert.Equal("created_at", createdAt.GetColumnName());
                }

                var updatedAt = entityType.FindProperty(nameof(BaseEntity.UpdatedAt));
                if (updatedAt != null)
                {
                    Assert.Equal("updated_at", updatedAt.GetColumnName());
                }

                var deletedAt = entityType.FindProperty(nameof(BaseEntity.DeletedAt));
                Assert.Equal(deletedAt != null, entityType.GetQueryFilter() != null);
                if (deletedAt != null)
                {
                    Assert.Equal("deleted_at", deletedAt.GetColumnName());
                }
            }

            var softDeleteTypes = new[]
            {
                typeof(Cliente),
                typeof(Fornecedor),
                typeof(FinanceiroContaPagar),
                typeof(Mecanico),
                typeof(OrdemServicoItem),
                typeof(Pessoa),
                typeof(Prestador),
                typeof(SegPerfil),
                typeof(SegUsuario),
                typeof(EstoquePeca),
                typeof(Veiculo)
            };
            var nonSoftDeleteEntity = context.Model.FindEntityType(typeof(ClienteOrigem))!;
            var marcaEntity = context.Model.FindEntityType(typeof(VeiculoMarca))!;

            foreach (var type in softDeleteTypes)
            {
                var entityType = context.Model.FindEntityType(type)!;
                Assert.NotNull(entityType.GetQueryFilter());
                Assert.Equal("deleted_at", entityType.FindProperty(nameof(BaseEntity.DeletedAt))!.GetColumnName());
            }

            Assert.Null(nonSoftDeleteEntity.GetQueryFilter());
            Assert.Null(marcaEntity.FindProperty(nameof(BaseEntity.DeletedAt)));
            Assert.Null(nonSoftDeleteEntity.FindProperty(nameof(BaseEntity.DeletedAt)));
            Assert.Equal("created_at", nonSoftDeleteEntity.FindProperty(nameof(BaseEntity.CreatedAt))!.GetColumnName());
            Assert.Equal("updated_at", nonSoftDeleteEntity.FindProperty(nameof(BaseEntity.UpdatedAt))!.GetColumnName());
            Assert.Null(marcaEntity.FindProperty(nameof(BaseEntity.UpdatedAt)));

            var lancamentoEntity = context.Model.FindEntityType(typeof(FinanceiroLancamento))!;
            Assert.Null(lancamentoEntity.FindProperty(nameof(BaseEntity.CreatedAt)));
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
                DescricaoProblema = "Teste"
            };

            Assert.Equal(OrdemServicoStatus.Aberta, ordem.Status);
        }

        [Fact]
        public void OrdemServico_ShouldHaveVehicleReferenceContract()
        {
            var ordem = new OrdemServico
            {
                ClienteId = 1,
                VeiculoId = 42,
                DescricaoProblema = "Teste"
            };

            Assert.Equal(42L, ordem.VeiculoId);
        }

        [Fact]
        public void CreateOrdemServicoDto_ShouldUseVehicleReferenceContract()
        {
            var dto = new CreateOrdemServicoDTO
            {
                ClienteId = 1,
                VeiculoId = 42,
                DescricaoProblema = "Teste",
                Status = OrdemServicoStatus.EmAndamento
            };

            Assert.Equal(42L, dto.VeiculoId);
            Assert.Equal(OrdemServicoStatus.EmAndamento, dto.Status);
        }

        [Fact]
        public void OrdemServicoJson_ShouldSerializeAsTextStatus()
        {
            var dto = new OrdemServicoResponseDTO
            {
                Id = 1,
                ClienteId = 10,
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
            const string json = "{\"ClienteId\":1,\"DescricaoProblema\":\"Teste\",\"Status\":\"Concluida\"}";

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

        private sealed class FakeEntity : BaseEntity
        {
        }
    }
}
