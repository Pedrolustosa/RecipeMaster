using MediatR;
using RecipeMaster.Application.Commands.Ingredients;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Application.Handlers
{
    public class DeleteIngredientCommandHandler : IRequestHandler<DeleteIngredientCommand, Unit>
    {
        private readonly IIngredientRepository _repository;

        public DeleteIngredientCommandHandler(IIngredientRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteIngredientCommand request, CancellationToken cancellationToken)
        {
            var ingredient = await _repository.GetByIdAsync(request.Id);

            if (ingredient == null)
            {
                throw new KeyNotFoundException("Ingredient not found");
            }

            await _repository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
