using MediatR;
using AutoMapper;
using RecipeMaster.Application.Commands.Recipes;
using RecipeMaster.Core.Interfaces.Repositories;

namespace RecipeMaster.Application.Handlers.Recipes
{
    public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, Unit>
    {
        private readonly IRecipeRepository _repository;
        private readonly IMapper _mapper;

        public UpdateRecipeCommandHandler(IRecipeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {
            var recipe = await _repository.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException("Recipe not found");
            _mapper.Map(request, recipe);
            await _repository.UpdateAsync(recipe);
            return Unit.Value;
        }
    }
}
