
namespace ASTRA.EMSG.Common.Master.GeoJSON
{
    public class GeoJSONStrings
    {
        public static string GeoJSONSuccess(string message)
        {
            return "{ \"success\": \"true\", \"message\":\"" + message + "\"}";
        }

        public static string GeoJSONFailure(string message)
        {
            return "{ \"success\": \"false\", \"message\":\"" + message + "\"}";
        }
    }
}
