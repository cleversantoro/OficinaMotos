using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OficinaMotos.Application.IoC;
using OficinaMotos.Domain.Common;
using OficinaMotos.Infrastructure.Context;
using System;
using System.Reflection;

namespace OficinaMotos.Tests.Helpers
{
    public static class TestDbContextFactory
    {
        public static OficinaContext Create(string? databaseName = null)
        {
            var dbName = string.IsNullOrWhiteSpace(databaseName)
                ? $"oficina-tests-{Guid.NewGuid()}"
                : databaseName;

            var options = new DbContextOptionsBuilder<OficinaContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .EnableSensitiveDataLogging()
                .Options;

            var context = new OficinaContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        public static IMapper CreateMapper()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddApplication();
            return services.BuildServiceProvider().GetRequiredService<IMapper>();
        }

        public static T WithId<T>(this T entity, long id) where T : BaseEntity
        {
            var prop = typeof(BaseEntity).GetProperty(
                nameof(BaseEntity.Id),
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            prop?.SetValue(entity, id);
            return entity;
        }
    }
}
