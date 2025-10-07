using FiapCloudGames.Games.Application.InputModels;
using FluentValidation;
using Serilog;

namespace FiapCloudGames.Games.Application.Validators;

public class UpdateGameInputModelValidator : AbstractValidator<UpdateGameInputModel>
{
    public UpdateGameInputModelValidator()
    {
        Log.Information("Iniciando {ValidatorName}", nameof(UpdateGameInputModelValidator));

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(inputModel => inputModel.Title)
            .NotEmpty().WithMessage("O título precisa ser informado!")
            .MaximumLength(100).WithMessage("O nome não pode exceder 100 caracteres!");

        RuleFor(inputModel => inputModel.Description)
            .NotEmpty().WithMessage("O e-mail precisa ser informado!")
            .MaximumLength(1000).WithMessage("O e-mail não pode exceder 1000 caracteres!")
            .When(inputModel => !string.IsNullOrWhiteSpace(inputModel.Description));

        RuleFor(inputModel => inputModel.Price)
            .NotNull().WithMessage("O preço precisa ser informado!")
            .GreaterThan(0).WithMessage("O preço precisa ser maior que zero!");

        RuleFor(inputModel => inputModel.GameGenreIds)
            .NotEmpty().WithMessage("Os gêneros do jogo deve conter no mínimo um.")
            .Must(list => list.All(n => n != 0))
            .WithMessage("Um ou mais identificadores do gêneros do jogo é inválido.");
    }
}
