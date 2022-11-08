using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public enum ResponseStatus
{
    OK,
    ERROR
}
public class ListResponse<T> : NetworkResponse
{
    List<T> values = new List<T>();

    public List<T> GetValues()
    {
        return values;
    }

    public ListResponse()
    {

    }

    public ListResponse(List<T> value)
    {
        this.values.Clear();
        this.values.AddRange(value);

        status = ResponseStatus.OK;
    }



    public static ListResponse<T> getNetworkError(string erno)
    {
        ListResponse<T> errResponse = new ListResponse<T>();
        errResponse.responseMessage = erno;
        return errResponse;
    }

}

public class Response<T> : NetworkResponse
{
    T value;

    public T GetValue()
    {
        return value;
    }

    public Response()
    {

    }

    public Response(T value)
    {
        this.value = value;
        status = ResponseStatus.OK;
    }

    public static Response<T> getNetworkError(string erno)
    {
        Response<T> errResponse = new Response<T>();
        errResponse.responseMessage = erno;
        errResponse.status = ResponseStatus.ERROR;
        return errResponse;
    }
}

public class NetworkResponse
{
    protected ResponseStatus status = ResponseStatus.ERROR;

    public ResponseStatus responseStatus()
    {
        return status;
    }

    internal string responseMessage;
    public string GetResponseMessage()
    {
        return responseMessage;
    }
}
