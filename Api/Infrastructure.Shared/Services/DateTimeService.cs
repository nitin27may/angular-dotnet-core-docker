using Application.Interfaces;
using System;

namespace Infrastructure.Shared.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime NowUtc => DateTime.UtcNow;
}
