namespace Application.DataTransfertObjects.Utilities;

public static class IdBuilder
{
    public static string AppIdFromHostAndApp(string hostName, string appName) => $"{hostName};{appName}";
    public static string ConnectionIdFromAppIdAndClientGaia(string appId, string clientGaia) => $"{appId};{clientGaia}";
}
