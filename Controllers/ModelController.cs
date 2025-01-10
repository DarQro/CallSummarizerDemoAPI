using CallSummarizerDemo.Services;
using CallSummarizerDemo.StaticDemoMocks;
using Microsoft.AspNetCore.Mvc;

namespace CallSummarizerDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class ModelController(ILogger<ModelController> logger, ISemanticKernelController semanticKernel, IHttpClientFactory httpClientFactory) : ControllerBase
{
    private readonly ILogger<ModelController> _logger = logger;
    private readonly ISemanticKernelController _sk = semanticKernel;
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("OllamaClient");

    [HttpPost("Summarize")]
    public async Task<IActionResult> Summarize([FromBody] string request)
    {
        // I should probably check the file type and make sure that it's an audio file
        // I could convert the audio file to a wav file if it's not already in that format
        // or i could require it be wav format and reject it?
        //if (text == null || text.Length == 0)
        //{
        //    return BadRequest("No file uploaded.");
        //}
        //using var stream = file.OpenReadStream();
        try
        {
            var response = await _sk.GetSummary(request);
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while summarizing transcript");
            return StatusCode(500, new { error = e.Message, details = e.ToString() });
        }

    }
}
