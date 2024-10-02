using Events.Application.Configuration.Settings;
using Events.Application.Models.EventParticipant.EventParticipant;
using Events.Application.Validation.Common;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Events.Application.Validation.EventParticipant;

public class EventParticipantFilterRequestModelValidator
    : AbstractValidator<EventParticipantFilterRequestModel>
{
    public EventParticipantFilterRequestModelValidator(IOptions<AppSettings> appSettings)
    {
        Include(
            new BaseFilterRequestModelValidator<EventParticipantFilterRequestModel>(appSettings)
        );
    }
}
