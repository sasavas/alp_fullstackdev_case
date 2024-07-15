using ForceGetCase.Core.Entities;
using ForceGetCase.DataAccess.Persistence;

namespace ForceGetCase.DataAccess.Repositories.Impl;

public class DimensionRepository : BaseRepository<Dimension>, IDimensionRepository
{
    public DimensionRepository(DatabaseContext context) : base(context)
    {
    }
}
