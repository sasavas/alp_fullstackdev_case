using ForceGetCase.Core.Entities;
using ForceGetCase.DataAccess.Persistence;

namespace ForceGetCase.DataAccess.Repositories.Impl;

public class TodoListRepository : BaseRepository<TodoList>, ITodoListRepository
{
    public TodoListRepository(DatabaseContext context) : base(context) { }
}
