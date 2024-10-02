using Events.Application.Configuration.Settings;
using Events.Application.Models.Participant;
using Events.Application.Validation.Common;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Events.Application.Validation.Participant;

public class ParticipantSignUpRequestModelValidator
    : AbstractValidator<ParticipantSignUpRequestModel>
{
    private readonly ValidationSettings _validationSettings;

    public ParticipantSignUpRequestModelValidator(IOptions<AppSettings> appSettings)
    {
        _validationSettings = appSettings.Value.ValidationSettings;

        RuleFor(x => x.Username)
            .NotEmptyWithMessage(nameof(ParticipantSignUpRequestModel.Username))
            .MaximumLengthWithMessage(
                _validationSettings.AccountUsernameMaxLength,
                nameof(ParticipantSignUpRequestModel.Username)
            )
            .MatchesWithMessage(
                _validationSettings.AccountUsernameFormat,
                nameof(ParticipantSignUpRequestModel.Username)
            );

        RuleFor(x => x.Password)
            .NotEmptyWithMessage(nameof(ParticipantSignUpRequestModel.Password))
            .MinimumLengthWithMessage(
                _validationSettings.AccountPasswordMinLength,
                nameof(ParticipantSignUpRequestModel.Password)
            )
            .MaximumLengthWithMessage(
                _validationSettings.StringMaxLength,
                nameof(ParticipantSignUpRequestModel.Password)
            );

        RuleFor(x => x.FirstName)
            .NotEmptyWithMessage(nameof(ParticipantSignUpRequestModel.FirstName))
            .MaximumLengthWithMessage(
                _validationSettings.ParticipantFirstNameMaxLength,
                nameof(ParticipantSignUpRequestModel.FirstName)
            );

        RuleFor(x => x.LastName)
            .NotEmptyWithMessage(nameof(ParticipantSignUpRequestModel.LastName))
            .MaximumLengthWithMessage(
                _validationSettings.ParticipantLastNameMaxLength,
                nameof(ParticipantSignUpRequestModel.LastName)
            );

        RuleFor(x => x.BirthDate)
            .NotDefaultWithMessage(nameof(ParticipantSignUpRequestModel.BirthDate))
            .MustBePastDateWithMessage(nameof(ParticipantSignUpRequestModel.BirthDate));

        RuleFor(x => x.Email)
            .NotEmptyWithMessage(nameof(ParticipantSignUpRequestModel.Email))
            .MaximumLengthWithMessage(
                _validationSettings.ParticipantEmailMaxLength,
                nameof(ParticipantSignUpRequestModel.Email)
            )
            .EmailAddressWithMessage(nameof(ParticipantSignUpRequestModel.Email));
    }
}
