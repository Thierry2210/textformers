namespace RedacaoAPI
{
    public class Key
    {

        public static string Secret = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? throw new Exception("Secret Key não configurada.");
    }
}