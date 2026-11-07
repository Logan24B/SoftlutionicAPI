using System.Net.Http.Json;

namespace SoftlutionicAPI.Services;

// Server-to-server bridge: no device tokens, credentials or recipient IDs in the browser.
public class ContactPushService(
    IHttpClientFactory clients,
    IConfiguration configuration,
    ILogger<ContactPushService> logger)
{
    public async Task NotifyAsync(string? notificationId)
    {
        var endpoint = configuration["ContactPush:Endpoint"];
        var secret = configuration["ContactPush:Secret"];
        if (string.IsNullOrWhiteSpace(notificationId) ||
            !Uri.TryCreate(endpoint, UriKind.Absolute, out var uri) ||
            uri.Scheme != Uri.UriSchemeHttps || string.IsNullOrEmpty(secret) || secret.Length < 32)
        {
            logger.LogWarning("Contact push bridge is not configured; notification remains stored.");
            return;
        }
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Add("X-Contact-Push-Key", secret);
            request.Content = JsonContent.Create(new { notification_id = notificationId });
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(8));
            using var response = await clients.CreateClient("ContactPush").SendAsync(request, timeout.Token);
            if (!response.IsSuccessStatusCode)
                logger.LogWarning("Contact push bridge returned HTTP {Status}.", (int)response.StatusCode);
        }
        catch (Exception ex)
        {
            // Do not log request headers, secrets or response bodies.
            logger.LogWarning("Contact push bridge failed: {ErrorType}.", ex.GetType().Name);
        }
    }
}
