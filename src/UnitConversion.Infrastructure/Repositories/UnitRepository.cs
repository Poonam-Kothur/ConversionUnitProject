using UnitConversion.Application.Interfaces;
using UnitConversion.Domain.Enums;
using UnitConversion.Domain.Models;
using UnitConversion.Infrastructure.Catalog;

namespace UnitConversion.Infrastructure.Repositories;

public sealed class UnitRepository : IUnitRepository
{
    private readonly IReadOnlyDictionary<Unit, UnitDefinition> _definitions = UnitCatalog.Definitions;
    private readonly IReadOnlyDictionary<ConversionType, IReadOnlyList<Unit>> _unitsByCategory = UnitCatalog.GetUnitsByCategory();

    public IReadOnlyCollection<UnitDefinition> GetAll()
    {
        return _definitions.Values.ToArray();
    }

    public UnitDefinition GetDefinition(Unit unit)
    {
        if (!_definitions.TryGetValue(unit, out var definition))
        {
            throw new KeyNotFoundException($"Unit '{unit}' is not defined in the unit catalog.");
        }

        return definition;
    }

    public bool TryGetDefinition(Unit unit, out UnitDefinition? definition)
    {
        return _definitions.TryGetValue(unit, out definition);
    }

    public IReadOnlyDictionary<ConversionType, IReadOnlyList<Unit>> GetUnitsByCategory()
    {
        return _unitsByCategory;
    }
}
