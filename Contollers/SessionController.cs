using PhotoTaggingApi.Models;
using PhotoTaggingApi.Services;
using Microsoft.AspNetCore.Mvc;
using SessionVariable;
using System.Data;
using Microsoft.AspNetCore.Authorization;


namespace PhotoTaggingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SessionController : ControllerBase
{

    [HttpGet]
    public IEnumerable<string> GetSessionInfo()
    {

        List<string> sessionInfo = new List<string>();


        DateTime UTCTime = DateTime.UtcNow;

        HttpContext.Session.SetString(SessionKeyEnum.SessionKeySessionDate.ToString(), UTCTime.ToString());
        HttpContext.Session.SetString(SessionKeyEnum.SessionKeySessionId.ToString(), Guid.NewGuid().ToString());

        var UtcTime = HttpContext.Session.GetString(SessionKeyEnum.SessionKeySessionDate.ToString());
        var sessionId = HttpContext.Session.GetString(SessionKeyEnum.SessionKeySessionId.ToString());

        sessionInfo.Add(UtcTime!);
        sessionInfo.Add(sessionId!);


        return sessionInfo;

    }
}