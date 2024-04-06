using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MessageSenderApp.Models;
using Azure.Storage.Queues.Models;

namespace MessageSenderApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly QueueService _queueService;

    public HomeController(ILogger<HomeController> logger, QueueService queueService)
    {
        _logger = logger;
        _queueService = queueService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage(string message)
    {
        SendReceipt receipt = await _queueService.SendMessageAsync(message);

        return Json(receipt);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
