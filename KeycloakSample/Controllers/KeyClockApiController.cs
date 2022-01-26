using Keycloak.Net;
using Keycloak.Net.Models.Roles;
using Keycloak.Net.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KeycloakSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeyClockApiController : ControllerBase
    {
        private readonly KeycloakClient _client = new("http://keycloak:8080", "admin", "admin");

        private const string Realm = "master";
        private const string ClientId = "06b11d09-6583-4634-8f25-5ceac008389e";

        [HttpGet, Route("GetUsers")]
        public async Task<IEnumerable<User>> GetUsers()
        {
            var allUsers = await _client.GetUsersAsync(Realm);

            return allUsers;
        }

        [HttpGet, Route("GetClientRoleMappingsForUser")]
        public async Task<IEnumerable<Role>> GetClientRoleMappingsForUserAsync(string userId)
        {
            var userRoles = await _client.GetClientRoleMappingsForUserAsync(Realm, userId, ClientId);

            return userRoles;
        }

        [HttpGet, Route("GetAvailableClientRoleMappingsForUser")]
        public async Task<IEnumerable<Role>> GetAvailableClientRoleMappingsForUser(string userId)
        {
            var availableClientRoles = await _client.GetAvailableClientRoleMappingsForUserAsync(Realm, userId, ClientId);

            return availableClientRoles;
        }


        [HttpPost, Route("AddClientRoleMappingsToUser")]
        public async Task<IEnumerable<Role>> AddClientRoleMappingsToUser(string userId)
        {
            var availableClientRoles = await _client.GetAvailableClientRoleMappingsForUserAsync(Realm,
                userId, ClientId);

            var addRoleToUser = availableClientRoles.ToList();
            await _client.AddClientRoleMappingsToUserAsync(Realm, userId, ClientId, addRoleToUser);

            return addRoleToUser;
        }
    }
}