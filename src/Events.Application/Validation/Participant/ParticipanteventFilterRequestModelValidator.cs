using Events.Application.Configuration.Settings;
using Events.Application.Models.EventParticipant.ParticipantEvent;
using Events.Application.Validation.Common;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Events.Application.Validation.Participant;

public class ParticipantEventFilterRequestModelValidator
    : AbstractValidator<ParticipantEventFilterRequestModel>
{
    public ParticipantEventFilterRequestModelValidator(IOptions<AppSettings> appSettings)
    {
        Include(
            new BaseFilterRequestModelValidator<ParticipantEventFilterRequestModel>(appSettings)
        );
    }
}
