# Build diaries

13:33 Sunday, 8 March 2026

I thik that generally it is easier to run and deploy a system using something I already am familiar with, and then, when everything is ready move to Docker.

Therefore I will deploy my database on good old Windows Server. I think there is something magical about this legacy system. There are: control panel applets (cpl), there is WMC (Windows Management Console), you can run legacy paint, and there are services. This is nostalgic.

Let's deploy our secrets to environment variables on Windows Server, so we don't have them in source code we will push to git using this PowerShell script:

  [Environment]::SetEnvironmentVariable("JWT_SECRET", "aaaaaaaaaaaaabbbbbbbbbbbbbbbbbbbbbbbbbbb-ItIsLongEnoughsoJWTwontComplainxD", "Machine")
  
It is important that the key is at least 256 characters long, otherwise we will get complaint from JWT library when trying to log in.

