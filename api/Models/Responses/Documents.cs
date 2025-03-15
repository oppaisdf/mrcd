namespace api.Models.Responses;

public record DocumentResponse(
    string Name,
    IEnumerable<BasicPersonResponse> People
);