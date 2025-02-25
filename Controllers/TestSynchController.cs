using Microsoft.AspNetCore.Mvc;
using SynchApp.Data;

namespace SyncApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestSynchController : ControllerBase
{
    
    private readonly SqliteDbContext _sqliteDbContext;
    private readonly SqlServerDbContext _serverDbContext;


    public TestSynchController(SqliteDbContext sqliteDbContext, SqlServerDbContext serverDbContext)
    {
        _sqliteDbContext = sqliteDbContext;
        _serverDbContext = serverDbContext;
    }

    [HttpGet("sqlite")]
    public async Task<IActionResult> GetSqlite()
    {
        var products = _sqliteDbContext.Products.ToList();
        return Ok(products);
    }
    
    [HttpGet("sqlserver")]
    public async Task<IActionResult> GetSqlServer()
    {
        var products = _serverDbContext.Products.ToList();
        return Ok(products);
    }
}