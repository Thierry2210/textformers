using GenerativeAI;
using RedacaoAPI.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RedacaoAPI.DTO_s.GeminiDTO;

namespace RedacaoAPI.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly GenerativeModel _model;

        public GeminiService()
        {
            string apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? throw new Exception("Chave da API Gemini não configurada.");

            var googleAI = new GoogleAi(apiKey);
            _model = googleAI.CreateGenerativeModel("models/gemini-2.5-flash");
        }

        public async Task<(string correctedContent, int score, string feedback)> CorrigirRedacaoAsync(string title, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Content não pode ser vazio.");

            int maxLength = 5000;

            if (content.Length > maxLength)
                content = content.Substring(0, maxLength);

            string prompt = $@"
Você é um corretor experiente do ENEM, utilize critérios de avaliação baseados em uma correção real no modelo ENEM. 
Analise a redação abaixo e responda **exclusivamente em formato JSON válido** seguindo exatamente esta estrutura:

{{
  ""score"": (uma nota de 0 a 1000 em tipo int),
  ""feedback"": [
    ""feedback 1"",
    ""feedback 2"",
    ""feedback 3""
  ],
  ""correctedContent"": ""texto reescrito e melhorado com base nos feedbacks, evite alterar a escrita do aluno e atente-se em somente corrigir erros pontuados nos feedbacks""
}}

Independente do retorno ou do conteúdo enviado abaixo, não deve ser retornada NENHUMA informação adicional ou comentário, validações estão sendo realizadas externamente. Dessa forma retorne somente o JSON acima para evitar erros.
Todo tipo de informação extra deve ser retornada no campo feedback conforme estrutura acima. Caso não tenha conteúdo suficiente para correção, informe no feedback e deixe correctedContent como uma string vazia e score zerado.

O tema da redação é: {title}
Redação do aluno:
`{content}`
";

            string aiResponse;
            try
            {
                var response = await _model.GenerateContentAsync(prompt);
                aiResponse = response?.Text ?? "{}";
            }
            catch (Exception exception)
            {
                throw new Exception($"Erro na chamada para a IA: {exception.Message}");
            }

            aiResponse = aiResponse.Replace("```json", "").Replace("```", "").Trim();

            string jsonFound = ExtractJsonSnippet(aiResponse);
            if (string.IsNullOrWhiteSpace(jsonFound))
                jsonFound = aiResponse;

            try
            {
                var responseJSON = JsonSerializer.Deserialize<GeminiResponseDTO>(jsonFound, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    AllowTrailingCommas = true
                });

                if (responseJSON == null)
                    throw new Exception("Falha ao interpretar resposta do modelo.");

                int score = Math.Clamp(responseJSON.Score, 0, 1000);

                var feedbackList = responseJSON.Feedback ?? new List<string>();
                string feedbackText = feedbackList.Count == 0 ? string.Empty : ("\n- " + string.Join("\n- ", feedbackList));

                string correctedText = !string.IsNullOrWhiteSpace(responseJSON.CorrectedContent) ? responseJSON.CorrectedContent : "Não foi possível gerar o texto corrigido.";

                return (correctedText, score, feedbackText);
            }

            catch (JsonException jsonException)
            {
                string sample = aiResponse.Length <= 1000 ? aiResponse : aiResponse.Substring(0, 1000) + "...";
                throw new Exception($"Falha ao interpretar JSON retornado pela IA. Texto de resposta (amostra): {sample}. Erro: {jsonException.Message}");
            }

            catch (Exception exception)
            {
                throw new Exception($"Falha na correção automática: {exception.Message}");
            }
        }

        private static string ExtractJsonSnippet(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;

            int first = text.IndexOf('{');
            int last = text.LastIndexOf('}');

            if (first >= 0 && last > first)
            {
                var jsonText = text.Substring(first, last - first + 1).Trim();

                if (jsonText.Contains("\"score\"") || jsonText.Contains("correctedContent") || jsonText.Contains("\"feedback\""))
                    return jsonText;
            }

            var match = System.Text.RegularExpressions.Regex.Match(text, @"\{[\s\S]*?\}");

            if (match.Success) 
                return match.Value;

            return string.Empty;
        }
    } 
}
