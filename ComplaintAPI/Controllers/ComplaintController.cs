using System.Linq;
using ComplaintAPI.ApiRouteSettings;
using ComplaintAPI.DbContext;
using ComplaintAPI.Services;
using ComplaintAPI.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using NLog;

namespace ComplaintAPI.Controllers
{
    
    [ApiController]
    public class ComplaintController : ControllerBase
    {
       // private ILoggerManager _logger;
        private IComplaintService _service;
        private IApiResponse _apiResponse;

        public ComplaintController(
            IComplaintService service,
            IApiResponse _response)
        {
           // this._logger = _logger;
            _service = service;
            _apiResponse = _response;
        }
        
        [Authorize]
        [Route(ApiRoutes.registerComplaint)]
        [HttpPost]
        [ServiceFilter(typeof(HandleException))]
        [ServiceFilter(typeof(ModelStateValidators))]
        public IActionResult RegisterComplaint([FromBody]ComplaintRequest complaintRequest)
        {
          //  _logger.LogInformation($"Input {JsonConvert.SerializeObject(complaintRequest)}");
            var complaint = new Complaint();
            complaint.description = complaintRequest.description;
            complaint.subject = complaintRequest.subject;
            complaint.emailAddress = complaintRequest.emailAddress;
            complaint.phoneNumber = complaintRequest.phoneNumber;
            complaint.productName = complaintRequest.productName;
            var resultFromService = _service.CreateNewComplaint(complaint);
            if (resultFromService)
            {
                //_logger.LogInformation($"response From Service:{resultFromService}");

                var response = _apiResponse.GetApiResponse("success", "00", "complaint registerd", null);

                return Ok(response);
            }
            else
            {
              //  _logger.LogInformation($"response From Service:{resultFromService}");
                var response = _apiResponse.GetApiResponse("failed", "99", "complaint failed registerd", null);
                return BadRequest();
            }
            
            
        }

        [Authorize]
        [Route(ApiRoutes.registerComplaint)]
        [HttpGet]
        public IActionResult GetUserClaims()
        {
            var claims = User.Claims;
            var simpleClaims = claims.Select(x => new
            {
                Issuuer = x.Issuer,
                
                
            });
            return Ok(claims);
        }
    }
    
    
}