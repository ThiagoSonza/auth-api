using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Infrastructure.Validation;

public class ValidatonCustomProblemDetails : ProblemDetails
{
    [JsonPropertyName("errors")]
    public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();

    public ValidatonCustomProblemDetails(
        HttpStatusCode status,
        string? detail = null,
        IEnumerable<(string PropertyName, string ErrorMessage)> errors = default!) : this()
    {
        Title = status switch
        {
            HttpStatusCode.BadRequest => "One or more validation errors occurred.",
            HttpStatusCode.InternalServerError => "Internal server error.",
            _ => "An error has occurred."
        };

        Status = (int)status;
        Detail = detail;

        if (errors is not null)
        {
            if (errors.Count() == 1)
                Detail = errors.First().PropertyName + ": " + errors.First().ErrorMessage;
            else if (errors.Count() > 1)
                Detail = "Multiple problems have occurred.";

            foreach (var (property, message) in errors)
            {
                if (!Errors.TryGetValue(property, out var messages))
                    Errors[property] = [message];
                else
                    Errors[property] = [.. messages, message];
            }
        }
    }

    public ValidatonCustomProblemDetails(
        HttpStatusCode status,
        HttpRequest request,
        string? detail = null,
        IEnumerable<(string PropertyName, string ErrorMessage)> errors = default!) : this(status, detail, errors) =>
        Instance = request.Path;

    private ValidatonCustomProblemDetails() =>
        Errors = new Dictionary<string, string[]>();
}