using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ComplaintServiceAPI.ApiRouteSettings;
using ComplaintServiceAPI.ComplaintServiceAPIContext;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Xunit;

namespace ComplaintServiceAPI.Test
{
    public class ComplaintServiceIntegrationBaseTest
    {
        public readonly HttpClient _client;

        public ComplaintServiceIntegrationBaseTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(ComplaintDbContext));
                        services.AddDbContext<ComplaintDbContext>(options => options.UseSqlServer(@"Data Source = FCMB-IT-L16582\TUNDE;database=BanklyTestApp;User Id=rib_details;Password=adejORbl@q9000"));
                    });
                });

            _client = appFactory.CreateClient();
        }
    }
    public class ComplaintServiceApiIntegrationTest : ComplaintServiceIntegrationBaseTest
    {
        [Fact]
        public async Task GetComplaintById_ShouldReturnComplaint_ValidID()
        {
            // arrange
            var getComplaint = await _client.GetAsync(ApiRoutes.getComplaintById.Replace("{Id}", 2.ToString()));
            
            // act
            dynamic response = (dynamic) JsonConvert.DeserializeObject(await getComplaint.Content.ReadAsStringAsync());
            
            // assert
            Console.WriteLine(response.message);
            getComplaint.StatusCode.Should().Be(HttpStatusCode.OK);
            //response.message.Should().Be("success");
        }
        
        [Fact]
        public async Task GetComplaintById_ShouldNotFound_WhenInValidIdIsPassed()
        {
            // arrange
            var getComplaint = await _client.GetAsync(ApiRoutes.getComplaintById.Replace("{Id}", 200.ToString()));
            
            // act
            dynamic response = (dynamic) JsonConvert.DeserializeObject(await getComplaint.Content.ReadAsStringAsync());
            
            // assert
          //  Console.WriteLine(response.message);
            getComplaint.StatusCode.Should().Be(HttpStatusCode.NotFound);
            //response.message.Should().Be("success");
        }

        [Fact]
        public async Task CReateNewComplaint_ShouldReturnOK()
        {
            // arrange 
            var stringContent = new StringContent(JsonConvert.SerializeObject(new ComplaintRequest()
            {
                productName = "testProduct",
                description ="testDescription",
                subject = "testSubject",
                phoneNumber = "testPhone",
                emailAddress = "testEmail@gmail.cm"
                
            }), Encoding.UTF8, "application/json");
            var httpResponse = await _client.PostAsync(ApiRoutes.registerComplaint, stringContent);
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        }
    }
}