using ForceGetCase.Core.Entities;
using ForceGetCase.DataAccess.Persistence;

namespace ForceGetCase.DataAccess.Repositories.Impl;

public class QuoteRepository : BaseRepository<Quote>, IQuoteRepository
{
    public QuoteRepository(DatabaseContext context) : base(context)
    {
    }
}
