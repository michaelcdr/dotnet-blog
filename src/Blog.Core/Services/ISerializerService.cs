using System.Net;
using System.Text;
using System.Text.Json;

namespace Blog.Core.Services;

public interface ISerializerService
{
    bool HandleResponseErrors(HttpResponseMessage response);
    Task<T> Deserialize<T>(HttpResponseMessage responseMessage);
    StringContent FormatContent(object data);
}

public class SerializerService : ISerializerService
{
    public async Task<T> Deserialize<T>(HttpResponseMessage responseMessage)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
    }

    public StringContent FormatContent(object data)
    {
        return new StringContent(
             JsonSerializer.Serialize(data),
             Encoding.UTF8,
             "application/json");
    }

    public bool HandleResponseErrors(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.BadRequest) return false;

        response.EnsureSuccessStatusCode();
        return true;
    }
    //protected ResponseResult RetornoOk()
    //{
    //    return new ResponseResult();
    //}
}