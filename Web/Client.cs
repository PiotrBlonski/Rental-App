using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using System.Net;

namespace ICarus_Rental.Web
{
    public class Client
    {
        readonly string Address;
        readonly HttpClient HttpClient;
        Token Token;
        public bool LoggedOut;
        public Client(string Address)
        {
            this.Address = Address;
            this.HttpClient = new();
        }

        public async Task<HttpResponseMessage> GetToken(string Login, string Password)
        {
            var LoginDetails = new Dictionary<string, string>
            {
                { "password", Password },
                { "login", Login }
            };

            HttpResponseMessage Response = await SendRequest(HttpMethod.Post, "/user/login", false, LoginDetails);
            if (Response.IsSuccessStatusCode)
            {
                Token = await GetData<Token>(Response);
                RefreshTokenTask(Token);
            }

            return Response;
        }

        public async Task<HttpResponseMessage> GetToken(string RefreshToken)
        {
            var LoginDetails = new Dictionary<string, string> { { "token", RefreshToken } };

            HttpResponseMessage Response = await SendRequest(HttpMethod.Post, "/user/refreshtoken", false, LoginDetails);
            if (Response.IsSuccessStatusCode)
            {
                Token = new Token
                {
                    RefreshToken = RefreshToken,
                    AccessToken = await Response.Content.ReadAsStringAsync()
                };
                RefreshTokenTask(Token);
            }

            return Response;
        }

        public async Task<HttpResponseMessage> SendRequest(HttpMethod Method, string Page, bool RequiresToken, IDictionary<string, string> Parameters = null, string? FilePath = null)
        {
            var Request = new HttpRequestMessage()
            {
                RequestUri = new Uri(Address + Page),
                Method = Method
            };

            if (Parameters != null)
                Request.Content = new StringContent(JsonSerializer.Serialize(Parameters), Encoding.UTF8, "application/json");

            if (FilePath != null)
                Request.Content = new MultipartFormDataContent { { new StreamContent(File.OpenRead(FilePath)), "file", FilePath[(FilePath.LastIndexOf('/') + 1)..] } };

            if (RequiresToken)
                Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token.AccessToken);

            try { return await HttpClient.SendAsync(Request); }
            catch { return new HttpResponseMessage() { StatusCode = HttpStatusCode.InternalServerError }; }
        }

        public static async Task<T> GetData<T>(HttpResponseMessage Response) => await JsonSerializer.DeserializeAsync<T>(await Response.Content.ReadAsStreamAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        public void SaveToken() => File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, "Token.txt"), Token.RefreshToken);

        void RefreshTokenTask(Token Token)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    Thread.Sleep(14000);

                    if (LoggedOut)
                        break;

                    HttpResponseMessage Response = await SendRequest(HttpMethod.Post, "/user/refreshtoken", false, new Dictionary<string, string> { { "token", Token.RefreshToken } });

                    if (Response.IsSuccessStatusCode)
                        Token.AccessToken = await Response.Content.ReadAsStringAsync();
                }
            });
        }
    }
}
