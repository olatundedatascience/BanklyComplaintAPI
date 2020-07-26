using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ComplaintServiceAPI.ComplaintServiceAPIContext;
using ComplaintServiceAPI.Services;
using Moq;
using Xunit;

namespace ComplaintServiceAPI.Test
{
    //[Collection(name:"ComplaintServiceAPITest")]
    public class ComplaintsTest
    {
        private ComplaintService _service;
        private Mock<IComplaintService> ComplaintServiceMock = new Mock<IComplaintService>();
        private Mock<IBaseRespository> BaseRepoMock = new Mock<IBaseRespository>();
        public ComplaintsTest()
        {
            this._service = new ComplaintService(BaseRepoMock.Object);
        }

        [Fact]
        public void GetComplaintById_ShouldReturnNull_WhenCustomerDoestNotExist()
        {
            //arrange
            ComplaintServiceMock.Setup(m => m.GetComplaintById(It.IsAny<long>()))
                .Returns(()=>null);
            
            // act
            var customerById = _service.GetAll(x => x.Id == 200);
            
            //assert
            Assert.Null(customerById);
        }
        [Trait("Get All Complaint", "Complaints")]
        //[Theory(DisplayName = "Get all Complaints raised by customer")]
        [Fact]
        public void GetAllComplaint_ShouldReturnNull_When_NoData()
        {
            // arrange
            IEnumerable<Complaint> allComplaints = new List<Complaint>().AsEnumerable();
            ComplaintServiceMock.Setup(m => m.GetAll())
                .Returns(allComplaints);
            
            // act
            var allComplaintsFromService = _service.GetAll();
            
            //assert
           // Assert.NotNull(allComplaints);
           Assert.Equal(allComplaints.Count(), allComplaintsFromService.Count());
        }
        
    }
}