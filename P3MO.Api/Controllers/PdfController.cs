using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Playwright;

namespace P3MO.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PdfController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/Pdf/Books
        [HttpGet("Books")]
        public async Task<IActionResult> GenerateBookListPdf()
        {
            var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:3000";
            var pdfBytes = await GeneratePdfFromUrl($"{frontendUrl}/books");

            return File(pdfBytes, "application/pdf", $"books-list-{DateTime.UtcNow:yyyy-MM-dd}.pdf");
        }

        // GET: api/Pdf/Books/5
        [HttpGet("Books/{id}")]
        public async Task<IActionResult> GenerateBookDetailPdf(int id)
        {
            var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:3000";
            var pdfBytes = await GeneratePdfFromUrl($"{frontendUrl}/books/{id}");

            return File(pdfBytes, "application/pdf", $"book-detail-{id}-{DateTime.UtcNow:yyyy-MM-dd}.pdf");
        }

        private async Task<byte[]> GeneratePdfFromUrl(string url)
        {
            // Initialize Playwright
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });

            // Create a new page
            var page = await browser.NewPageAsync();

            // Navigate to the URL
            await page.GotoAsync(url, new PageGotoOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle,
                Timeout = 60000 // 60 seconds timeout
            });

            // Wait for charts to render (assuming they have a specific class)
            await page.WaitForSelectorAsync(".highcharts-container", new PageWaitForSelectorOptions
            {
                State = WaitForSelectorState.Attached,
                Timeout = 10000 // 10 seconds timeout
            });

            // Additional delay to ensure everything is fully rendered
            await page.WaitForTimeoutAsync(2000);

            // Generate PDF
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

            return pdfBytes;
        }
    }
}
