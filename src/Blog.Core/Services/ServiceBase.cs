namespace Blog.Core.Services;

public abstract class ServiceBase
{
    private readonly ISerializerService _serializadorService;

    protected ServiceBase(ISerializerService serializerService)
    {
        _serializadorService = serializerService;
    }

    protected virtual bool ManipularResponseErrors(HttpResponseMessage response)
    {
        return _serializadorService.HandleResponseErrors(response);
    }

    protected virtual async Task<T> Deserializar<T>(HttpResponseMessage responseMessage)
    {
        return await _serializadorService.Deserialize<T>(responseMessage);
    }

    protected virtual StringContent FormatarConteudo(object data)
    {
        return _serializadorService.FormatContent(data);
    }
}
