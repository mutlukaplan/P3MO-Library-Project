using Microsoft.AspNetCore.Mvc;
using Microsoft.Playwright;

namespace P3MO.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PdfController> _logger;

        public PdfController(IConfiguration configuration, ILogger<PdfController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/Pdf/Books
        [HttpGet("Books")]
        public async Task<IActionResult> GenerateBookListPdf()
        {
            var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:3000";
            var pdfBytes = await GeneratePdfFromUrl($"{frontendUrl}/books", true); // true = has chart

            return File(pdfBytes, "application/pdf", $"books-list-{DateTime.UtcNow:yyyy-MM-dd}.pdf");
        }

        // GET: api/Pdf/Books/5
        [HttpGet("Books/{id}")]
        public async Task<IActionResult> GenerateBookDetailPdf(int id)
        {
            var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:3000";
            var pdfBytes = await GeneratePdfFromUrl($"{frontendUrl}/books/{id}", false); // false = no chart

            return File(pdfBytes, "application/pdf", $"book-detail-{id}-{DateTime.UtcNow:yyyy-MM-dd}.pdf");
        }

        private async Task<byte[]> GeneratePdfFromUrl(string url, bool hasChart)
        {
            _logger.LogInformation($"Generating PDF for URL: {url}, hasChart: {hasChart}");

            // Initialize Playwright
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });

            // Create a new page
            var page = await browser.NewPageAsync();

            try
            {
                // Navigate to the URL
                _logger.LogInformation($"Navigating to {url}");
                await page.GotoAsync(url, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle,
                    Timeout = 60000 // 60 seconds timeout
                });

                // If the page has a chart, wait for it to render
                if (hasChart)
                {
                    _logger.LogInformation("Waiting for chart to render");
                    await page.WaitForSelectorAsync(".highcharts-container", new PageWaitForSelectorOptions
                    {
                        State = WaitForSelectorState.Attached,
                        Timeout = 10000 // 10 seconds timeout
                    });
                }
                else
                {
                    // For pages without charts, just wait a moment for any JavaScript to complete
                    _logger.LogInformation("No chart to wait for, delaying to allow page to render");
                    await page.WaitForTimeoutAsync(2000);
                }

                // Additional delay to ensure everything is fully rendered
                await page.WaitForTimeoutAsync(1000);

                // Generate PDF
                _logger.LogInformation("Generating PDF");
                var pdfBytes = await page.PdfAsync(new PagePdfOptions
                {
                    Format = "A4",
                    PrintBackground = true,
                    Margin = new Margin
                    {
                        Top = "20px",
                        Bottom = "20px",
                        Left = "20px",
                        Right = "20px"
                    }
                });

                _logger.LogInformation($"PDF generated successfully, size: {pdfBytes.Length} bytes");
                return pdfBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating PDF");
                throw;
            }
        }
    }
}
