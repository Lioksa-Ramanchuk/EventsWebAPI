namespace Events.Application.Models.Common;

public abstract record BaseFilterRequestModel(int? Offset, int? Limit);
