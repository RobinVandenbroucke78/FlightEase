using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FlightEase.Domains;
using FlightEase.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace FlightEase.Services
{
    public class HotelService : IHotelService
    {
        private readonly IConfiguration configure;
        private string? apiBaseUrl;

        public HotelService(IConfiguration configuration)
        {
            configure = configuration;
            apiBaseUrl = configure["amadeusAPI: BaseUrl"];
        }

        public Task AddAsync(Hotel entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Hotel entity)
        {
            throw new NotImplementedException();
        }

        public Task<Hotel?> FindByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Hotel>?> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Hotel>?> GetHotelsByCityAsync(string city)
        {
            string? apiKey = configure["AmadeusApi:ApiKey"];
            string? apiSecret = configure["AmadeusApi:ApiSecret"];

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var tokenResponse = await GetOAuthTokenAsync(apiKey, apiSecret);

                    if (string.IsNullOrEmpty(tokenResponse))
                    {
                        throw new Exception("OAuth-token niet ontvangen.");
                    }

                    // Voeg de token toe aan de headers voor authenticatie
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse);

                    // Stap 2: Doe een verzoek naar de Amadeus Hotels API
                    //  var response = await _httpClient.GetAsync($"{{_baseUrl}}/v1/reference-data/locations/hotels/by-city?cityCode={city}");


                    var response = await httpClient.GetAsync($"{configure["AmadeusApi:BaseUrl"]}/v1/reference-data/locations/hotels/by-city?cityCode={city}");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var hotels = System.Text.Json.JsonSerializer.Deserialize<HotelApiResponse>(jsonString, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        // By default, deserialization looks for case -sensitive property name matches between JSON and the target object properties. To change that behavior, 
                        //set JsonSerializerOptions.PropertyNameCaseInsensitive to true:

                        return hotels?.Data ?? new List<Hotel>();
                    }
                    else
                    {
                        throw new Exception($"Fout bij het ophalen van hotels: {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    // Log fout en gooi deze opnieuw
                    Console.WriteLine($"Er is een fout opgetreden: {ex.Message}");
                    throw;
                }
            }
        }

        private async Task<string> GetOAuthTokenAsync(string apiKey, string apiSecret)
        {
            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", apiKey),
                new KeyValuePair<string, string>("client_secret", apiSecret)
            });
            var httpClient = new HttpClient();

            var response = await httpClient.PostAsync("https://test.api.amadeus.com/v1/security/oauth2/token", requestContent);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(jsonString);
                var token = jsonDocument.RootElement.GetProperty("access_token").GetString();
                return token;
            }

            return null;
        }

        public Task UpdateAsync(Hotel entity)
        {
            throw new NotImplementedException();
        }
    }
}
