﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Generic.Abp.IdentityServer.ClaimTypes
{
    public class ClaimTypeController:IdentityServerController,IClaimTypeAppService
    {
        public IClaimTypeAppService ApplicationService { get; }
        public ClaimTypeController(IClaimTypeAppService claimTypeAppService)
        {
            ApplicationService = claimTypeAppService;
        }

        [HttpGet]
        public Task<ListResultDto<ClaimTypeDto>> GetListAsync()
        {
            return ApplicationService.GetListAsync();
        }

    }
}
