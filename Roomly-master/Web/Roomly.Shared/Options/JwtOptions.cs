﻿namespace Roomly.Shared.Options;

public class JwtOptions
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string[] Audiences { get; set; }
    public int ExpiresHours { get; set; }
}