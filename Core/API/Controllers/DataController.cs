using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class DataController : ControllerBase
{  
    private readonly ILogger<DataController> _logger;
    private readonly ConcurrentDictionary<string, double> _data; 

    public DataController(ILogger<DataController> logger, ConcurrentDictionary<string, double> data)
    {
        _logger = logger;
        _data = data;
    }

    [HttpPut(Name = "SetData")]
    public int SetData(string key, double value)
    {
        _data[key] = value;
        return _data.Count;
    }

    [HttpGet(Name = "List")]
    public IEnumerable<KeyValuePair<string, double>> Get()
    {
        // NOTE - not a thread safe snap shot
        return _data.Select(x => x).ToArray();
    }
}