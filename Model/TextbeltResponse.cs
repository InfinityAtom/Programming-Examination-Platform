using System.Text.Json.Serialization;

public class TextbeltResponse
{
  [JsonPropertyName("success")]
  public bool Success { get; set; }

  [JsonPropertyName("textId")]
  public string TextId { get; set; }

  [JsonPropertyName("quotaRemaining")]
  public int QuotaRemaining { get; set; }
}