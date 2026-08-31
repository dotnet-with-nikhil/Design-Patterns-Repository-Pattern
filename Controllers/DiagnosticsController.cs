using DotNet8_Enterprise_CRUD.Services;
using Microsoft.AspNetCore.Mvc;
namespace DotNet8_Enterprise_CRUD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiagnosticsController(ITransientService t1, ITransientService t2, IScopedService s1, IScopedService s2, ISingletonService g1, ISingletonService g2) : ControllerBase
{
    [HttpGet("lifetimes")]
    public IActionResult Get() => Ok(
        new { transient1 = t1.Id, transient2 = t2.Id, scoped1 = s1.Id, scoped2 = s2.Id, singleton1 = g1.Id, singleton2 = g2.Id }
        );
}
