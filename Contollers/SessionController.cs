using PhotoTaggingApi.Models;
using PhotoTaggingApi.Services;
using Microsoft.AspNetCore.Mvc;
using SessionVariable;
using UserSessionVariable;
using System.Data;


namespace PhotoTaggingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SessionController : ControllerBase
{

    [HttpGet]
    public IEnumerable<string> GetSessionInfo()
    {
        var userName = HttpContext.Session.GetString(UserSessionKeyEnum.SessionKeyUsername.ToString());
        List<string> sessionInfo = new List<string>();


        DateTime UTCTime = DateTime.UtcNow;
        HttpContext.Session.SetString(SessionKeyEnum.SessionKeyUsername.ToString(), userName!);
        HttpContext.Session.SetString(SessionKeyEnum.SessionKeySessionDate.ToString(), UTCTime.ToString());
        HttpContext.Session.SetString(SessionKeyEnum.SessionKeySessionId.ToString(), Guid.NewGuid().ToString());

        var username = HttpContext.Session.GetString(SessionKeyEnum.SessionKeyUsername.ToString());
        var UtcTime = HttpContext.Session.GetString(SessionKeyEnum.SessionKeySessionDate.ToString());
        var sessionId = HttpContext.Session.GetString(SessionKeyEnum.SessionKeySessionId.ToString());

        sessionInfo.Add(username!);
        sessionInfo.Add(sessionId!);


        return sessionInfo;

    }
}