namespace ComplaintServiceAPI.ApiRouteSettings
{
    public class ApiRoutes
    {
        private const string base_url = "api";
        private const string version = ApiVersion.Version_One;

        private const string url_format = base_url + "/" + version;

        private class ComplaintRegistration
        {
            public const string registerComplaint = url_format + "/registerComplaint";
            public const string getComplaint = url_format + "/getComplaint/{Id}";
            public const string getAllComplaint = url_format + "/allComplaints";
            public const string updateComplaint = url_format + "/updateComplaint/{Id}";
            public const string deleteComplaint = url_format + "/deleteComplaint/{Id}";
        }

        public const string registerComplaint = ComplaintRegistration.registerComplaint;
        public const string getAllComplaint = ComplaintRegistration.getAllComplaint;
        public const string getComplaintById = ComplaintRegistration.getComplaint;
        public const string updateComplaint = ComplaintRegistration.updateComplaint;
        public const string deleteComplaint = ComplaintRegistration.deleteComplaint;
        
    }

    public class ApiVersion
    {
        public const string Version_One = "v1";
        public const string Version_One_Beta = "v1_1";
        public const string Version_Two = "v2";
    }
}