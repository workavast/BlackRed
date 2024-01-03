using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;

public static partial class WebApi
{
    public static async partial Task<(bool, string, T)> Get<T>(string webApiPath, string jwt)
        => await SendRequest<T>(webApiPath, jwt);

    public static async partial Task<(bool, string, T)> Post<T>(string webApiPath, object data, string jwt)
        => await SendRequest<T>(webApiPath, jwt, RequestType.POST, data);

    public static async partial Task<(bool, string)> Post(string webApiPath, object data, string jwt)
        => await SendRequest(webApiPath, jwt, RequestType.POST, data);
    
    private static async Task<(bool, string, T)> SendRequest<T>(string webApiPath, string jwt = null, RequestType requestType = RequestType.GET, object data = null)
    {
        var request = CreateRequest(webApiPath, jwt, requestType, data);

        var result = await WebRequestAwait<T>(request);
        request.Dispose();
        
        return result;
    }
    
    private static async Task<(bool, string)> SendRequest(string webApiPath, string jwt = null, RequestType requestType = RequestType.GET, object data = null)
    {
        var request = CreateRequest(webApiPath, jwt, requestType, data);

        var result = await WebRequestAwait(request);
        request.Dispose();

        return result;
    }
    
    private static UnityWebRequest CreateRequest(string path, string jwt = null, RequestType requestType = RequestType.GET, object data = null)
    {
        var request = new UnityWebRequest(path, requestType.ToString());

        if (data != null)
        {
            var dataRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            request.uploadHandler = new UploadHandlerRaw(dataRaw);
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        if(jwt != null)
            request.SetRequestHeader("Authorization", jwt);

        return request;
    }
    
    private static async Task<(bool, string, T)> WebRequestAwait<T>(UnityWebRequest request)
    {
        request.SendWebRequest();

        while (!request.isDone)
            await Task.Delay(10);
        
        return request.error == null 
            ? (true, null, JsonConvert.DeserializeObject<T>(request.downloadHandler.text))
            : (false, request.downloadHandler.text, default);
    }
    
    private static async Task<(bool, string)> WebRequestAwait(UnityWebRequest request)
    {
        request.SendWebRequest();

        while (!request.isDone)
            await Task.Delay(10);
        
        return request.error == null 
            ? (true, null)
            : (false, request.downloadHandler.text);
    }
    
    private enum RequestType
    {
        GET,
        POST
    }
}

public static partial class WebApi
{
    /// <returns>
    /// return three values. <br/>
    /// bool - if request success then it true, else false. <br/>
    /// string - if request success then it null, else error string. <br/>
    /// T - if request success then it T-response data, else default. <br/>
    /// </returns>
    public static partial Task<(bool, string, T)> Get<T>(string webApiPath, string jwt = null);

    /// <returns>
    /// return three values. <br/>
    /// bool - if request success then it true, else false. <br/>
    /// string - if request success then it null, else error string. <br/>
    /// T - if request success then it T-response data, else default. <br/>
    /// </returns>
    public static partial Task<(bool, string, T)> Post<T>(string webApiPath, object data = null, string jwt = null);

    /// <returns>
    /// return two values. <br/>
    /// bool - if request success then it true, else false. <br/>
    /// string - if request success then it null, else error string. <br/>
    /// </returns>
    public static partial Task<(bool, string)> Post(string webApiPath, object data = null, string jwt = null);
}