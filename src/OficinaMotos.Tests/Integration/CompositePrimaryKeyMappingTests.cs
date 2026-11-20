using Microsoft.EntityFrameworkCore;
using OficinaMotos.Domain.Entities;
using OficinaMotos.Infrastructure.Context;

namespace OficinaMotos.Tests.Integration;

public class CompositePrimaryKeyMappingTests
{
    [Fact]
    public void Join_entities_use_the_composite_primary_keys_defined_by_the_schema()
    {
        var options = new DbContextOptionsBuilder<OficinaContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new OficinaContext(options);

        AssertPrimaryKey<FornecedorSegmentoRel>(context, nameof(FornecedorSegmentoRel.FornecedorId), nameof(FornecedorSegmentoRel.SegmentoId));
        AssertPrimaryKey<PrestadorAtividade>(context, nameof(PrestadorAtividade.PrestadorId), nameof(PrestadorAtividade.AtividadeProfissionalId));
        AssertPrimaryKey<SegPerfilPermissao>(context, nameof(SegPerfilPermissao.PerfilId), nameof(SegPerfilPermissao.PermissaoId));
        AssertPrimaryKey<SegUsuarioPerfil>(context, nameof(SegUsuarioPerfil.UsuarioId), nameof(SegUsuarioPerfil.PerfilId));
    }

    private static void AssertPrimaryKey<TEntity>(OficinaContext context, params string[] expectedProperties)
        where TEntity : class
    {
        var entityType = context.Model.FindEntityType(typeof(TEntity));
        Assert.NotNull(entityType);
        Assert.Equal(expectedProperties, entityType!.FindPrimaryKey()!.Properties.Select(property => property.Name));
        Assert.DoesNotContain(entityType.GetProperties(), property => property.Name == "Id");
    }
}
