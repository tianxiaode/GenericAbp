using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace QuickTemplate.Controllers;

public class ExportController
{
    protected readonly ILogger<ExportController> Logger;

    public ExportController(ILogger<ExportController> logger)
    {
        Logger = logger;
    }

    [HttpGet]
    [Route("/api/users/export")]
    public virtual Task<ExportDto> ExportAsync(ExportInputDto input)
    {
        Logger.LogDebug($"提交参数：{System.Text.Json.JsonSerializer.Serialize(input)}");
        var result = new ExportDto();
        if (input == null)
        {
            throw new AbpException("error");
        }

        if (input.IsAsync)
        {
            result.IsAsync = true;
            return Task.FromResult(result);
        }

        result.File = "https://localhost:44320/global.js";
        return Task.FromResult(result);
    }
}