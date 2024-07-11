using ForceGetCase.Core.Entities;
using ForceGetCase.DataAccess.Persistence;

namespace ForceGetCase.DataAccess.Repositories.Impl;

public class TodoItemRepository : BaseRepository<TodoItem>, ITodoItemRepository
{
    public TodoItemRepository(DatabaseContext context) : base(context) { }
}
