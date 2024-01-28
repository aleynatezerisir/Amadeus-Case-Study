using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Flight
{
    public int Id { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string Date { get; set; }
}

public class ApiResponse
{
    public int StatusCode { get; set; }
    public JObject Response { get; set; }
}

class Program
{
    static void Main()
    {
        // Test case'i JSON formatına dönüştür
        string testCaseJson = @"
        {
          ""status_code"": 200,
          ""response"": {
            ""data"": [
              {
                ""id"": 1,
                ""from"": ""IST"",
                ""to"": ""LAX"",
                ""date"": ""2022-12-13""
              },
              {
                ""id"": 2,
                ""from"": ""JFK"",
                ""to"": ""LHR"",
                ""date"": ""2022-12-14""
              }
            ]
          }
        }";

        // JSON formatındaki test case'i parse et
        ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(testCaseJson);

        // Kontrolleri yap
        if (apiResponse.StatusCode != 200)
        {
            Console.WriteLine($"Test case başarısız. Hatalı HTTP status code: {apiResponse.StatusCode}");
            return;
        }

        // Header kontrolü
        if (apiResponse.Response["Content-Type"] == null || apiResponse.Response["Content-Type"].ToString() != "application/json")
        {
            Console.WriteLine("Test case başarısız. Hatalı Content-Type header değeri.");
            return;
        }

        // Response içeriği kontrolü
        try
        {
            var flights = apiResponse.Response["data"].ToObject<Flight[]>();
            foreach (var flight in flights)
            {
                // Her bir Flight objesinin kontrolü
                if (flight.Id <= 0 || string.IsNullOrEmpty(flight.From) || string.IsNullOrEmpty(flight.To) || string.IsNullOrEmpty(flight.Date))
                {
                    Console.WriteLine("Test case başarısız. Flight objesi hatalı.");
                    return;
                }
            }

            Console.WriteLine("Test case başarıyla geçti.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Test case başarısız. Hata: {ex.Message}");
        }
    }
}
