using Events.Application.Configuration.Settings;
using Events.Application.Models.Participant;
using Events.Application.Validation.Account;
using Events.Application.Validation.Common;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Events.Application.Validation.Participant;

public class ParticipantUpdateRequestModelValidator
    : AbstractValidator<ParticipantUpdateRequestModel>
{
    private readonly ValidationSettings _validationSettings;

    public ParticipantUpdateRequestModelValidator(IOptions<AppSettings> appSettings)
    {
        _validationSettings = appSettings.Value.ValidationSettings;

        Include(new AccountUpdateRequestModelValidator(appSettings));

        RuleFor(x => x.FirstName)
            .NotEmptyWithMessage(nameof(ParticipantUpdateRequestModel.FirstName))
            .MaximumLengthWithMessage(
                _validationSettings.ParticipantFirstNameMaxLength,
                nameof(ParticipantUpdateRequestModel.FirstName)
            )
            .When(x => !string.IsNullOrWhiteSpace(x.FirstName));

        RuleFor(x => x.LastName)
            .NotEmptyWithMessage(nameof(ParticipantUpdateRequestModel.LastName))
            .MaximumLengthWithMessage(
                _validationSettings.ParticipantLastNameMaxLength,
                nameof(ParticipantUpdateRequestModel.LastName)
            )
            .When(x => !string.IsNullOrWhiteSpace(x.LastName));

        RuleFor(x => x.BirthDate)
            .NotDefaultWithMessage(nameof(ParticipantUpdateRequestModel.BirthDate))
            .MustBePastDateWithMessage(nameof(ParticipantUpdateRequestModel.BirthDate))
            .When(x => x.BirthDate is not null && x.BirthDate != default);

        RuleFor(x => x.Email)
            .NotEmptyWithMessage(nameof(ParticipantUpdateRequestModel.Email))
            .MaximumLengthWithMessage(
                _validationSettings.ParticipantEmailMaxLength,
                nameof(ParticipantUpdateRequestModel.Email)
            )
            .EmailAddressWithMessage(nameof(ParticipantUpdateRequestModel.Email))
            .When(x => !string.IsNullOrWhiteSpace(x.Email));
    }
}
