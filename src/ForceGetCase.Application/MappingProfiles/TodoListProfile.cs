using AutoMapper;
using ForceGetCase.Application.Models.TodoList;
using ForceGetCase.Core.Entities;

namespace ForceGetCase.Application.MappingProfiles;

public class TodoListProfile : Profile
{
    public TodoListProfile()
    {
        CreateMap<CreateTodoListModel, TodoList>();

        CreateMap<TodoList, TodoListResponseModel>();
    }
}
