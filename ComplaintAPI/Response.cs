namespace ComplaintAPI
{
    public class Response
    {
        public Response(string message, string statusCode, string description, dynamic responseData)
        {
            this.responseData = responseData;
            this.message = message;
            this.statusCode = statusCode;
            this.description = description;
        }
        public dynamic responseData { get; set; }
        public string statusCode { get; set; }
        public string description { get; set; }
        public string message { get; set; }
    }

    public interface IApiResponse
    {
        object GetApiResponse(string message, string statusCode, string description, dynamic responseData);
    }
    public class ApiResponse : IApiResponse
    {
        public object GetApiResponse(string message, string statusCode, string description, dynamic responseData)
        {
            var response = new Response(message, statusCode, description, responseData);
            return new
            {
                responseData = response.responseData,
                statusCode = response.statusCode,
                message = response.message,
                description = response.description
            };
        }
    }
}