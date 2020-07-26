using ComplaintServiceAPI.Services;
using Xunit;

namespace ComplaintServiceAPI.Test
{
    [CollectionDefinition(name:"ComplaintServiceAPITest")]
    public class ComplaintTestCollectionDefinition : ICollectionFixture<IComplaintService>
    { }
}