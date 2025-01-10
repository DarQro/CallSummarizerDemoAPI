using Whisper.net;
using Whisper.net.Ggml;
using Microsoft.Extensions.Options;
using System.Text;

namespace CallSummarizerDemo.Services;

public class WhisperConfiguration
{
    public string ModelPath { get; set; } = string.Empty;
    public string ModelUrl { get; set; } = "https://huggingface.co/datasets/ggerganov/whisper.cpp/resolve/main/ggml-base.en.bin";
    public bool DownloadModelIfMissing { get; set; } = true;
}

public class WhisperCppService : ISpeechToTextService, IDisposable
{
    private readonly WhisperConfiguration _config;
    private readonly ILogger<WhisperCppService> _logger;
    private WhisperFactory? _whisperFactory;
    private bool _disposed;
    private readonly SemaphoreSlim _initLock = new(1, 1);

    public WhisperCppService(
        IOptions<WhisperConfiguration> config,
        ILogger<WhisperCppService> logger)
    {
        _config = config.Value;
        _logger = logger;
    }

    private async Task EnsureInitializedAsync()
    {
        if (_whisperFactory != null) return;

        await _initLock.WaitAsync();
        try
        {
            if (_whisperFactory != null) return;

            if (!File.Exists(_config.ModelPath) && _config.DownloadModelIfMissing)
            {
                _logger.LogInformation("Downloading whisper model from {Url}", _config.ModelUrl);
                await DownloadModelAsync();
            }

            _whisperFactory = WhisperFactory.FromPath(_config.ModelPath);

            _logger.LogInformation("Whisper factory initialized successfully");
        }
        finally
        {
            _initLock.Release();
        }
    }

    private async Task DownloadModelAsync()
    {
        using var client = new HttpClient();
        using var response = await client.GetAsync(_config.ModelUrl, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        var modelDir = Path.GetDirectoryName(_config.ModelPath);
        if (!string.IsNullOrEmpty(modelDir))
        {
            Directory.CreateDirectory(modelDir);
        }

        using var stream = await response.Content.ReadAsStreamAsync();
        using var fileStream = File.Create(_config.ModelPath);
        await stream.CopyToAsync(fileStream);
    }

    public async Task<string> TranscribeAudioAsync(Stream audioStream)
    {
        await EnsureInitializedAsync();

        if (_whisperFactory == null)
            throw new InvalidOperationException("Whisper factory not initialized");

        try
        {
            var builder = new StringBuilder();

            using var processor = _whisperFactory.CreateBuilder()
                .WithLanguage("en")
                .Build();

            var segments = processor.ProcessAsync(audioStream);
            await foreach (var segment in segments)
            {
                builder.AppendLine(segment.Text);
            }

            return builder.ToString().Trim();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during transcription");
            throw;
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _whisperFactory?.Dispose();
            _initLock.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}

public interface ISpeechToTextService
{
    Task<string> TranscribeAudioAsync(Stream stream);
}

public class SpeechToTextService : ISpeechToTextService
{
    public async Task<string> TranscribeAudioAsync(Stream stream)
    {
        return "Hello, World!";
    }
}
