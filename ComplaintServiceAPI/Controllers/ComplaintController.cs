using System.Linq;
using System.Threading.Tasks;
using ComplaintServiceAPI.ApiRouteSettings;
using ComplaintServiceAPI.ComplaintServiceAPIContext;
using ComplaintServiceAPI.Services;
using ComplaintServiceAPI.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using NLog;

namespace ComplaintServiceAPI.Controllers
{
    
    [ApiController]
    public class ComplaintController : ControllerBase
    {
       private ILoggerManager _logger;
        private IComplaintService _service;
        private IApiResponse _apiResponse;

        public ComplaintController(
            ILoggerManager _logger,
            IComplaintService service,
            IApiResponse _response)
        {
           this._logger = _logger;
            _service = service;
            _apiResponse = _response;
        }


        [Route(ApiRoutes.updateComplaint)]
        [HttpPut]
        [ServiceFilter(typeof(HandleException))]
        public async Task<IActionResult> UpdateComplaint([FromRoute] long Id,
            [FromBody] ComplaintRequest complaintRequest)
        {
            _logger.LogInformation($"update complaints input : {Id}-{JsonConvert.SerializeObject(complaintRequest)}");
            var complaint = new Complaint();
            complaint.description = complaintRequest.description;
            complaint.subject = complaintRequest.subject;
            complaint.emailAddress = complaintRequest.emailAddress;
            complaint.phoneNumber = complaintRequest.phoneNumber;
            complaint.productName = complaintRequest.productName;

            var responseFromService = _service.updateComplaint(Id, complaint);

            if (responseFromService)
            {
                _logger.LogInformation($"response From Service:{responseFromService}");
                var response = _apiResponse.GetApiResponse("success", "00", "complaint updated", null);
                return Ok(response);
            }
            else
            {
                _logger.LogInformation($"response From Service:{responseFromService}");
                var response = _apiResponse.GetApiResponse("failed", "99", "complaint failed to update", null);
                return BadRequest(response);
            }


        }

        [Route(ApiRoutes.deleteComplaint)]
        [HttpDelete]
        [ServiceFilter(typeof(HandleException))]
        public async Task<IActionResult> DeleteComplaint([FromRoute]long Id)
        {
            _logger.LogInformation($"delete complaints input : {Id}");
            var resultFromService = _service.DeleteComplaint(Id);

            if (resultFromService)
            {
                _logger.LogInformation($"response From Service:{resultFromService}");
                var response = _apiResponse.GetApiResponse("success", "00", "complaint deleted", null);
                return Ok(response);
            }
            else
            {
                _logger.LogInformation($"response From Service:{resultFromService}");
                var response = _apiResponse.GetApiResponse("failed", "99", "complaint failed to delete", null);
                return BadRequest(response);
            }
        }

        [Route(ApiRoutes.getComplaintById)]
        [HttpGet]
        [ServiceFilter(typeof(HandleException))]
        public async Task<IActionResult> GetComplaintById([FromRoute] long Id)
        {
            _logger.LogInformation($"get complaints by Id input : {Id}");
            var resultFromService = _service.GetAll(x => x.Id == Id).FirstOrDefault();

            if (resultFromService != null)
            {
                _logger.LogInformation($"response From Service:{resultFromService}");
                var response = _apiResponse.GetApiResponse("success", "00", $"complaint with Id {Id} retrieved", resultFromService);
                return Ok(response);
            }
            else
            {
                _logger.LogInformation($"response From Service:{resultFromService}");
                var response = _apiResponse.GetApiResponse("failed", "99", $"complaint with Id {Id} could not be found", null);
                return NotFound(response);
            }
        }


        [Route(ApiRoutes.updateComplaintStatus)]
        [HttpPut]
        [ServiceFilter(typeof(HandleException))]
        public async Task<IActionResult> updateTaskStatus([FromRoute] long Id, bool status)
        {
            _logger.LogInformation($"update complaints status by Id input : {Id}, status: {status}");
            var responseFromService = _service.updateStatus(Id, status);
            if (responseFromService)
            {
                _logger.LogInformation($"response From Service:{responseFromService}");
                var response = _apiResponse.GetApiResponse("success", "00", $"complaint with Id {Id} updated", null);
                return Ok(response);
            }
            else
            {
                _logger.LogInformation($"response From Service:{responseFromService}");
                var response = _apiResponse.GetApiResponse("failed", "99", $"complaint with Id {Id} could not be found or updatedd", null);
                return NotFound(response);
            }
        }
        
        [Authorize]
        [Route(ApiRoutes.getAllComplaint)]
        [HttpGet]
        [ServiceFilter(typeof(HandleException))]
        public async Task<IActionResult> GetAllComplaint()
        {
            _logger.LogInformation($"get all complaints");
            var allComplaints = _service.GetAll();
            
            var response = _apiResponse.GetApiResponse("success", "00", "complaint retrieved success", allComplaints);

            return Ok(response);
        }
        
        //[Authorize]
        [Route(ApiRoutes.registerComplaint)]
        [HttpPost]
        [ServiceFilter(typeof(HandleException))]
        [ServiceFilter(typeof(ModelStateValidators))]
        public IActionResult RegisterComplaint([FromBody]ComplaintRequest complaintRequest)
        {
           _logger.LogInformation($"Input {JsonConvert.SerializeObject(complaintRequest)}");
            var complaint = new Complaint();
            complaint.description = complaintRequest.description;
            complaint.subject = complaintRequest.subject;
            complaint.emailAddress = complaintRequest.emailAddress;
            complaint.phoneNumber = complaintRequest.phoneNumber;
            complaint.productName = complaintRequest.productName;
            var resultFromService = _service.CreateNewComplaint(complaint);
            if (resultFromService)
            {
                _logger.LogInformation($"response From Service:{resultFromService}");

                var response = _apiResponse.GetApiResponse("success", "00", "complaint registerd", null);

                return Ok(response);
            }
            else
            {
                _logger.LogInformation($"response From Service:{resultFromService}");
                var response = _apiResponse.GetApiResponse("failed", "99", "complaint failed registerd", null);
                return BadRequest(response);
            }
            
            
        }

        // [Authorize]
        // [Route(ApiRoutes.registerComplaint)]
        // [HttpGet]
        // public IActionResult GetUserClaims()
        // {
        //     var claims = User.Claims;
        //     var simpleClaims = claims.Select(x => new
        //     {
        //         Issuuer = x.Issuer,
        //         
        //         
        //     });
        //     return Ok(claims);
        // }
    }
    
    
}