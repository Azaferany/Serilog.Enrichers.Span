namespace Serilog.Enrichers.Span;

using System;
using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

/// <summary>
/// A log event enricher which adds baggage from the current <see cref="Activity"/>.
/// </summary>
public class ActivityOperationNameEnricher : ILogEventEnricher
{
    private readonly SpanLogEventPropertiesNames propertiesNames;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActivityOperationNameEnricher"/> class.
    /// </summary>
    /// <param name="logEventPropertiesNames">Names for log event properties.</param>
    public ActivityOperationNameEnricher(SpanLogEventPropertiesNames logEventPropertiesNames)
    {
        CheckPropertiesNamesArgument(logEventPropertiesNames);
        this.propertiesNames = logEventPropertiesNames;
    }

    /// <summary>
    /// Enrich the log event.
    /// </summary>
    /// <param name="logEvent">The log event to enrich.</param>
    /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(logEvent);
#else
        if (logEvent is null)
        {
            throw new ArgumentNullException(nameof(logEvent));
        }
#endif

        var activity = Activity.Current;
        if (activity is not null)
        {
            var operationName = activity.OperationName;
            logEvent.AddPropertyIfAbsent(new LogEventProperty(this.propertiesNames.OperationName, new ScalarValue(operationName)));
        }
    }

    private static void CheckPropertiesNamesArgument(SpanLogEventPropertiesNames logEventPropertyNames)
    {
#if NET6_0_OR_GREATER
#pragma warning disable IDE0022
        ArgumentNullException.ThrowIfNull(logEventPropertyNames);
#pragma warning restore IDE0022
#else
        if (logEventPropertyNames is null)
        {
            throw new ArgumentNullException(nameof(logEventPropertyNames));
        }
#endif
    }
}
