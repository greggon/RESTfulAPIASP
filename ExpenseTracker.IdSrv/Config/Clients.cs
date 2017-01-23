﻿using System.Collections.Generic;
using Thinktecture.IdentityServer.Core.Models;

namespace ExpenseTracker.IdSrv.Config
{
    public class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new[]
                   {
                       new Client
                       {
                           Enabled = true,
                           ClientName = "ExpenseTracker MVC Client (Hybrid Flow)",
                           ClientId = "mvc",
                           Flow = Flows.Hybrid,
                           RequireConsent = true,

                           RedirectUris = new List<string>
                                              {ExpenseTrackerConstants.ExpenseTrackerClient}
                       }
                   };
        }
    }
}