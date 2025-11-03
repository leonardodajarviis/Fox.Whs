using System.Text;
using System.Text.Json;
using Fox.Whs.Options;
using Microsoft.Extensions.Options;

namespace Fox.Whs.Services;

public class SapServiceLayerAuthService
{
    private readonly HttpClient _httpClient;
    private readonly SapServiceLayerOptions _options;
    private string? _sessionId;
    private DateTime? _sessionExpiry;

    public SapServiceLayerAuthService(
        HttpClient httpClient, 
        IOptions<SapServiceLayerOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    /// <summary>
    /// Login vào SAP Service Layer và lấy Session ID
    /// </summary>
    /// <param name="username">Username để login (bắt buộc)</param>
    /// <param name="password">Password để login (bắt buộc)</param>
    /// <param name="companyDb">Company DB (optional, sẽ lấy từ config nếu không truyền)</param>
    public async Task<string?> LoginAsync(string username, string password, string? companyDb = null)
    {
        try
        {
            // Validate username và password
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username không được để trống", nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password không được để trống", nameof(password));
            }

            // Lấy CompanyDB từ options nếu không truyền vào
            companyDb ??= _options.CompanyDB;
            var baseUrl = _options.BaseUrl;

            if (string.IsNullOrEmpty(companyDb))
            {
                throw new ArgumentException("CompanyDB không được để trống");
            }

            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentException("BaseUrl chưa được cấu hình");
            }

            var loginData = new
            {
                CompanyDB = companyDb,
                UserName = username,
                Password = password
            };

            var jsonContent = JsonSerializer.Serialize(loginData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{baseUrl}/Login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);

                // Lấy SessionId từ response
                if (loginResponse.TryGetProperty("SessionId", out var sessionIdElement))
                {
                    _sessionId = sessionIdElement.GetString();
                    _sessionExpiry = DateTime.Now.AddMinutes(_options.SessionTimeoutMinutes);
                    
                    return _sessionId;
                }

                // Lấy SessionId từ Cookie nếu không có trong response body
                if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
                {
                    foreach (var cookie in cookies)
                    {
                        if (cookie.StartsWith("B1SESSION="))
                        {
                            _sessionId = cookie.Split(';')[0].Replace("B1SESSION=", "");
                            _sessionExpiry = DateTime.Now.AddMinutes(_options.SessionTimeoutMinutes);
                            
                            return _sessionId;
                        }
                    }
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Login SAP thất bại. Status: {response.StatusCode}, Error: {errorContent}");
            }

            return null;
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Lấy Session ID hiện tại
    /// </summary>
    /// <returns>SessionId nếu còn hợp lệ, null nếu đã hết hạn</returns>
    public string? GetSessionId()
    {
        if (string.IsNullOrEmpty(_sessionId) || _sessionExpiry == null || DateTime.Now >= _sessionExpiry)
        {
            return null;
        }

        return _sessionId;
    }

    /// <summary>
    /// Kiểm tra Session có còn hợp lệ không
    /// </summary>
    public bool IsSessionValid()
    {
        return !string.IsNullOrEmpty(_sessionId) && 
               _sessionExpiry != null && 
               DateTime.Now < _sessionExpiry;
    }

    /// <summary>
    /// Logout khỏi SAP Service Layer
    /// </summary>
    public async Task<bool> LogoutAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(_sessionId))
            {
                return false;
            }

            var baseUrl = _options.BaseUrl;
            
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={_sessionId}");

            var response = await _httpClient.PostAsync($"{baseUrl}/Logout", null);

            if (response.IsSuccessStatusCode)
            {
                _sessionId = null;
                _sessionExpiry = null;
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Thực hiện GET request với Session ID
    /// </summary>
    public async Task<string?> GetAsync(string endpoint)
    {
        try
        {
            var sessionId = GetSessionId();
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new InvalidOperationException("Session không hợp lệ hoặc đã hết hạn. Vui lòng login lại.");
            }

            var baseUrl = _options.BaseUrl;
            
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={sessionId}");

            var response = await _httpClient.GetAsync($"{baseUrl}/{endpoint}");
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"GET request thất bại. Endpoint: {endpoint}, Status: {response.StatusCode}, Error: {error}");
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}
