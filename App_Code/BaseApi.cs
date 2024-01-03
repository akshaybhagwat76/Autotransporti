using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

public class BaseApi : ApiController
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public bool isRequestAuthenticated()
    {
        var header = Request.Headers;
        if (!header.Contains("Authorization"))
            return false;
        else
        {
            var token = header.GetValues("Authorization").FirstOrDefault().ToString().Replace("token ", "").Trim();
            return true; //.User.checkAuthToken(token);
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public bool isApiKeyValid()
    {
        var header = Request.Headers;
        if (!header.Contains("Authorization"))
            return false;
        else
        {
            var key = header.GetValues("Authorization").FirstOrDefault().ToString().Replace("key ", "").Trim();
            return key == Utility.Settings.ApiKey;
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public HttpResponseMessage setResponseMessage(System.Net.HttpStatusCode code, string message)
    {
        return new HttpResponseMessage()
        {
            StatusCode = code,
            Content = new StringContent(message)
        };
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public HttpResponseMessage setResponseMessage(System.Net.HttpStatusCode code, StringResponse message)
    {
        return new HttpResponseMessage()
        {
            StatusCode = code,
            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(message), System.Text.Encoding.UTF8, "application/json")
        };
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAuth : Attribute
{
    private string description;
    public ApiKeyAuth() { }
    public ApiKeyAuth(string description)
    {
        this.description = description;
    }
}
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class TokenAuth : Attribute
{
    private string description;
    public TokenAuth() { }
    public TokenAuth(string description)
    {
        this.description = description;
    }
}