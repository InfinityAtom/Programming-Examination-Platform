using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class TextbeltService
{
  private readonly HttpClient _httpClient;

  public TextbeltService()
  {
    _httpClient = new HttpClient();
  }

  public async Task SendSmsAsync(string phoneNumber, string message, string apiKey)
  {
    try
    {
      // Construct the URL using the provided API key, phone number, and message
      string url = $"https://textbelt.com/text?key={apiKey}&phone={phoneNumber}&message={Uri.EscapeDataString(message)}";

      // Send the GET request to the constructed URL
      var response = await _httpClient.GetStringAsync(url);

      // Deserialize the JSON response into a TextbeltResponse object
      var textbeltResponse = JsonSerializer.Deserialize<TextbeltResponse>(response);

      // Output the deserialized response to the console for debugging
      Console.WriteLine($"Response from Textbelt: Success={textbeltResponse.Success}, TextId={textbeltResponse.TextId}, QuotaRemaining={textbeltResponse.QuotaRemaining}");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error sending SMS: {ex.Message}");
    }
  }
}