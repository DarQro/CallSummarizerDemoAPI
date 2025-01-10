using CallSummarizerDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace CallSummarizerDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WhisperController : ControllerBase
    {
        private readonly ISpeechToTextService _stt;
        private readonly ILogger<WhisperController> _logger;

        public WhisperController(ISpeechToTextService stt, ILogger<WhisperController> logger)
        {
            _stt = stt;
            _logger = logger;
        }

        [HttpPost("Transcribe")]
        public async Task<IActionResult> TranscribeAudio(IFormFile file)
        {
            // I should probably check the file type and make sure that it's an audio file
            // I could convert the audio file to a wav file if it's not already in that format
            // or i could require it be wav format and reject it?
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            using var stream = file.OpenReadStream();
            var response = await _stt.TranscribeAudioAsync(stream);
            return Ok(response);
        }
    }
}
