using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MessageSenderApp.Models;

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

    public async Task<IActionResult> Privacy()
    {
        await _queueService.SendMessageAsync("hello world");
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
