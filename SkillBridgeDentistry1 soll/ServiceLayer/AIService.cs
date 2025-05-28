using CoreLayer.Services;
using Newtonsoft.Json;
using SkillBridgeDentistry1.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _aiApiUrl = "https://dentalaiapp.onrender.com/predict";
        public AIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<AIResponseDto> PredictDiseaseAsync(string imagePath)
        {
            using var form = new MultipartFormDataContent();
            var fileStream = File.OpenRead(imagePath);
            var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            form.Add(streamContent, "file", Path.GetFileName(imagePath));

            var response = await _httpClient.PostAsync(_aiApiUrl, form);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("AI model prediction failed.");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AIResponseDto>(jsonString);

            return result;
        }



    }

}
