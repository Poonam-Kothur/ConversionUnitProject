using UnitConversion.Domain.Enums;
using UnitConversion.Domain.Models;

namespace UnitConversion.Application.Interfaces;

public interface IUnitRepository
{
    IReadOnlyCollection<UnitDefinition> GetAll();
    UnitDefinition GetDefinition(Unit unit);
    bool TryGetDefinition(Unit unit, out UnitDefinition? definition);
    IReadOnlyDictionary<ConversionType, IReadOnlyList<Unit>> GetUnitsByCategory();
}
