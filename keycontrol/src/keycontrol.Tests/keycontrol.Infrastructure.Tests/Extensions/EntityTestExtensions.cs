using keycontrol.Domain.Entities;
using keycontrol.Domain.Shared;
using System.Reflection;

namespace keycontrol.Tests.Extensions;

public static class EntityTestExtensions{
      public static void SetPrivateId<T>(this Result<T> entity, int id) where T : Entity
    {
        typeof(Entity).GetProperty("Id", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            ?.SetValue(entity.Value, id);
    }
}