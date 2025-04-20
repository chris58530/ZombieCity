public class JsonDataProxy : IProxy
{
    public JsonData jsonData;
    public void SetData(JsonData data)
    {
        jsonData = data;
    }
    public JsonData GetData()
    {
        return jsonData;
    }
}
