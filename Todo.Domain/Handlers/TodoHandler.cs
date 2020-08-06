using Flunt.Notifications;
using Todo.Domain.Commands;
using Todo.Domain.Commands.Contracts;
using Todo.Domain.Entities;
using Todo.Domain.Handlers.Contracts;
using Todo.Domain.Repositories;

namespace Todo.Domain.Handlers
{
    public class TodoHandler : 
    Notifiable,
    IHandler<CreateTodoCommand>,
    IHandler<UpdateTodoCommand>,
    IHandler<MarkTodoCommand>,
    IHandler<MarkTodoAsUndoneCommand>
    {

        private readonly ITodoRepository _repository;
        public TodoHandler(ITodoRepository repository)
        {
            _repository = repository;
        }

        public ICommandResult Handle(CreateTodoCommand command)
        {
            //Fail Fast Validation
            command.Validate();
            if(command.Invalid){
                return new GenericCommandResult(false,
                "Ops, parece que sua tarefa está errada",
                command.Notifications);
            }

            //Gera TodoItem
            var todo = new TodoItem(command.Title, command.User,command.Date);

            //Salva no banco
            _repository.Create(todo);

            //Retorna o resultado

            return new GenericCommandResult(true,"Tarefa salva",todo);

        }

        public ICommandResult Handle(UpdateTodoCommand command)
        {
            //Fail Fast Validation
            command.Validate();
            if(command.Invalid){
                return new GenericCommandResult(false,
                "Ops, parece que sua tarefa está errada",
                command.Notifications);
            }

            //recupera um TodoItem
            var todo = _repository.GetById(command.Id, command.User);

            //altera titulo
            todo.UpdateTitle(command.Title);

            //salva no banco
            _repository.Update(todo);

            //retorna resultado

            return new GenericCommandResult(true," Tarefa Salva",todo);

        }

        public ICommandResult Handle(MarkTodoCommand command)
        {
            command.Validate();
            if(command.Invalid)
            {
                 return new GenericCommandResult(false,
                "Ops, parece que sua tarefa está errada",
                command.Notifications);
            }

            var todo = _repository.GetById(command.Id,command.User);

            //altera estado
            todo.MarkAsDone();

            //Salva
            _repository.Update(todo);

            //retorna
            return new GenericCommandResult(true,"Tarefa salva",todo);
        }

        public ICommandResult Handle(MarkTodoAsUndoneCommand command)
        {
             command.Validate();
            if(command.Invalid)
            {
                 return new GenericCommandResult(false,
                "Ops, parece que sua tarefa está errada",
                command.Notifications);
            }

            var todo = _repository.GetById(command.Id,command.User);

            //altera estado
            todo.MarkAsUnDone();

            //Salva
            _repository.Update(todo);

            //retorna
            return new GenericCommandResult(true,"Tarefa salva",todo);
        }
    }
}