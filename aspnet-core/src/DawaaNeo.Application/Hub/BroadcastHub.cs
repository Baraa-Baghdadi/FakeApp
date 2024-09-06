using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace DawaaNeo.Hub
{
    // note: inherit from ITransientDependency for inject this class in another class......
    public class BroadcastHub : Hub<IHubClient> , ITransientDependency
    {
        private readonly ICurrentUser _currentUser;
        private readonly IHubContext<BroadcastHub> _context;
        private static readonly ConcurrentDictionary<Guid,List<string>> _userSessions
            = new ConcurrentDictionary<Guid,List<string>>();

        public BroadcastHub(ICurrentUser currentUser, IHubContext<BroadcastHub> context)
        {
            _currentUser = currentUser;
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            // maybe you should get token from header also (show signalR request from FE):
            //var token = Context?.GetHttpContext()?.Request.Query["access_token"].ToString().Replace("Bearer ","");
            //var handler = new JwtSecurityTokenHandler();
            //var jwt = handler.ReadJwtToken(token);

            // get Connection Id from token:
            var connectionId = Context?.ConnectionId;

            // get Tenant Id:
            var tenantId = _currentUser.TenantId;
            var id = _currentUser.Id;
            var userName = _currentUser.UserName;

            // add new connection Id:
            RegiserUserSessions(connectionId!, tenantId ?? Guid.Empty);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // note: maybe you should get token from header also (show signalR request from FE)......
            // note: maybe you need token for get userId from it.........
            //var token = Context?.GetHttpContext()?.Request.Query["access_token"].ToString().Replace("Bearer ", "");
            //var handler = new JwtSecurityTokenHandler();
            //var jwt = handler.ReadJwtToken(token);


            // get Connection Id from token:
            var connectionId = Context?.ConnectionId;

            // get Tenant Id:
            var tenantId = _currentUser.TenantId ?? Guid.Empty;

            // remove connection Id:
            RemoveUserSessions(connectionId!, tenantId);


            await base.OnDisconnectedAsync(exception);
        }

        private void RegiserUserSessions(string connectionId, Guid tenantId)
        {
            // add new Tenant & connection Id:
            if (!_userSessions.ContainsKey(tenantId))
            {
                _userSessions.TryAdd(tenantId, new List<string>() { connectionId });
            }
            // add new connection Id:
            else if (!_userSessions[tenantId].Contains(connectionId))
            {
                _userSessions[tenantId].Add(connectionId);
            }
        }

        private void RemoveUserSessions(string connectionId, Guid tenantId)
        {
            if (_userSessions.ContainsKey(tenantId))
            {
                var removeIndex = _userSessions[tenantId].FindIndex(x => !x.Contains(connectionId));

                if (removeIndex != -1)
                {
                    _userSessions[tenantId].RemoveAt(removeIndex);
                }
            }
        }

        public ConcurrentDictionary<Guid,List<string>> getAllConnectionsId()
        {
            return _userSessions;
        }
    }
}
