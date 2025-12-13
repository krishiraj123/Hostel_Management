
class ApiKeyAuthenticationMiddleware
{
    private readonly IConfiguration _configuration;
    private readonly RequestDelegate _next;

    private const string APIKEYNAME = "ApiKey";

    public ApiKeyAuthenticationMiddleware(IConfiguration configuration, RequestDelegate next)
    {
        _configuration = configuration;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if(!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API key is missing.");
            return;            
        }

        string apiKey = _configuration.GetValue<string>("ApiKey");

        if(string.IsNullOrWhiteSpace(apiKey) || !apiKey.Equals(extractedKey))
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Unauthorized client.");
            return;
        }

        await _next(context);
    }
}