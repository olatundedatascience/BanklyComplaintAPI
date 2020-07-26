namespace BankyAuthServer.Routes
{
    public class ApiRoutes
    {
        private const string base_url = "api";
        private const string version = ApiVersion.Version_One;

        private const string url_format = base_url + "/" + version;

        private class ClientRegistration
        {
            public const string registerClient = url_format + "/registerClient";
        }

        public const string registerClient = ClientRegistration.registerClient;
    }

    public class ApiVersion
    {
        public const string Version_One = "v1";
        public const string Version_One_Beta = "v1_1";
        public const string Version_Two = "v2";
    }
}