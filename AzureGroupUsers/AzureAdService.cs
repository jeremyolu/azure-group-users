using Azure.Identity;
using Microsoft.Graph;

namespace AzureGroupUsers
{
    public class AzureAdService : IAzureAdService
    {
        public List<AzureUser> GetAzureUsers()
        {
            var users = new List<AzureUser>();

            string tenantId = Util.TenantId;
            string clientId = Util.ClientId;
            string clientSecret = Util.ClientSecret;
            var scopes = new[] { ".default" };

            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

            var serviceUsers = graphClient.Users.Request().Select(x => new { x.Id, x.DisplayName, x.Mail }).GetAsync().Result;

            foreach (var user in serviceUsers)
            {
                users.Add(new AzureUser
                { Id = user.Id, Name = user.DisplayName, Mail = user.Mail });
            }

            return users;
        }

        public AzureUser GetAzureUserById(string id)
        {
            string tenantId = Util.TenantId;
            string clientId = Util.ClientId;
            string clientSecret = Util.ClientSecret;
            var scopes = new[] { ".default" };

            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

            var user = graphClient.Users.Request().Select(x => new { x.Id, x.DisplayName, x.Mail }).GetAsync().Result.FirstOrDefault(x => x.Id == id);

            return new AzureUser { Id = user.Id, Name = user.DisplayName, Mail = user.Mail };
        }

        public List<AzureGroup> GetAzureGroups()
        {
            var groups = new List<AzureGroup>();

            string tenantId = Util.TenantId;
            string clientId = Util.ClientId;
            string clientSecret = Util.ClientSecret;
            var scopes = new[] { ".default" };

            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

            var serviceGroups = graphClient.Groups.Request().Select(x => new { x.Id, x.DisplayName }).GetAsync().Result;

            foreach (var group in serviceGroups)
            {
                groups.Add(new AzureGroup { Id = group.Id, Name = group.DisplayName });
            }

            return groups;
        }

        public AzureGroup GetAzureGroupById(string id)
        {

            var groups = new List<AzureGroup>();

            string tenantId = Util.TenantId;
            string clientId = Util.ClientId;
            string clientSecret = Util.ClientSecret;
            var scopes = new[] { ".default" };

            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

            var serviceGroup = graphClient.Groups.Request().Select(x => new { x.Id, x.DisplayName }).GetAsync().Result.FirstOrDefault(x => x.Id == id);

            return new AzureGroup { Id = serviceGroup.Id, Name = serviceGroup.DisplayName };
        }

        public async Task<List<AzureUser>> GetAzureUsersInGroupId(string id)
        {
            var groupUsers = new List<AzureUser>();

            string tenantId = Util.TenantId;
            string clientId = Util.ClientId;
            string clientSecret = Util.ClientSecret;
            var scopes = new[] { ".default" };

            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

            var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

            var members = await graphClient.Groups[id].Members.Request().GetAsync();

            foreach (var member in members)
            {
                var user = GetAzureUserById(member.Id);

                groupUsers.Add(user);
            }

            return groupUsers;
        }
    }
}
