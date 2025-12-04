using System;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Scoping;
using System.Net.Mail;
using System.Net;

namespace UmbracoProject.Controllers
{
  [ApiController]
  [Route("api/mail")]
  public class SendMailController : ControllerBase
  {
    private readonly IContentService _contentService;
    private readonly IConfiguration _config;
    private readonly ICoreScopeProvider _scopeProvider;
    private readonly ILogger<SendMailController> _logger;
    public SendMailController(
        IContentService contentService,
        IConfiguration config,
        ICoreScopeProvider scopeProvider,
        ILogger<SendMailController> logger)
    {
      _contentService = contentService;
      _config = config;
      _scopeProvider = scopeProvider;
      _logger = logger;
    }

    [HttpGet("send-test-email")]
    public IActionResult SendEmail(
        [FromBody] JsonElement payload,
        [FromQuery(Name = "from")] string? from = null,
        [FromQuery(Name = "to")] string? to = null,
        [FromQuery(Name = "msg")] string? msg = "Hello Nick!"
        )
    {
      var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
      {
        Credentials = new NetworkCredential(_config["Smtp:Username"], _config["Smtp:Password"]),
        EnableSsl = true
      };

      try
      {
        client.Send(from!, to!, "Test Email From Demo", msg);
        System.Console.WriteLine("Email Sent!");
      }
      catch (Exception e)
      {
        return BadRequest(new { message = e.Message });
      }

      return Ok(new { message = "Email sent" });
    }
  }
}