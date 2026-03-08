# Build diaries

13:33 Sunday, 8 March 2026

I thik that generally it is easier to run and deploy a system using something I already am familiar with, and then, when everything is ready move to Docker.

Therefore I will deploy my database on good old Windows Server. I think there is something magical about this legacy system. There are: control panel applets (cpl), there is WMC (Windows Management Console), you can run legacy paint, and there are services. This is nostalgic.

Let's deploy our secrets to environment variables on Windows Server, so we don't have them in source code we will push to git using this PowerShell script:

  [Environment]::SetEnvironmentVariable("JWT_SECRET", "aaaaaaaaaaaaabbbbbbbbbbbbbbbbbbbbbbbbbbb-ItIsLongEnoughsoJWTwontComplainxD", "Machine")
  
It is important that the key is at least 256 characters long, otherwise we will get complaint from JWT library when trying to log in.


17:18 Sunday, 8 March 2026

Note that we can easily test our project using MOQ. The reason is that we have DTO objects, so it is very easy to predict database structure for MOQ.

This is production-ready, follows clean architecture, respects JWT auth, and integrates neatly with your EF Core setup.


<!-- Magnus: Do you ever use builder pattern used commonly in very large projects?

Many junior developers expose public setters everywhere and create mutable chaos.
In modern .NET (especially EF Core), many teams use:
init;
instead of set.
Example:
public string Title { get; init; }
Which allows:
new CVProfile { Title = "Test CV" }
but only during object creation.

 -->