using FiapCloudGames.Games.Application.InputModels;
using FiapCloudGames.Games.Application.Validators;
using FluentValidation.Results;

namespace FiapCloudGames.Games.Tests.Validators;

public class CreateGameInputModelValidatorTests
{
    [Fact]
    public void Should_PassValidation_When_InputModelIsValid()
    {
        // Arrange
        CreateGameInputModel input = new("Jogo Legal", "Descrição do jogo", 50, [1, 2]);

        // Act
        ValidationResult result = new CreateGameInputModelValidator().Validate(input);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_FailValidation_When_TitleIsEmpty()
    {
        CreateGameInputModel input = new("", "Descrição do jogo", 50, [1]);

        ValidationResult result = new CreateGameInputModelValidator().Validate(input);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Title" && e.ErrorMessage == "O título precisa ser informado!");
    }

    [Fact]
    public void Should_FailValidation_When_DescriptionExceedsMaxLength()
    {
        CreateGameInputModel input = new("Jogo Legal", new string('a', 1001), 50, [1]);

        ValidationResult result = new CreateGameInputModelValidator().Validate(input);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Description" && e.ErrorMessage == "O e-mail não pode exceder 1000 caracteres!");
    }
}
