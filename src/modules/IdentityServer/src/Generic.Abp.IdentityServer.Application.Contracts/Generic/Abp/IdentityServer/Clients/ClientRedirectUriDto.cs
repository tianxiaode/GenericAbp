﻿using System;

namespace Generic.Abp.IdentityServer.Clients;

[Serializable]
public class ClientRedirectUriDto
{
    public string RedirectUri { get; set; }
}