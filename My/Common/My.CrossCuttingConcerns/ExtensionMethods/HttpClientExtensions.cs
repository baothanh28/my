﻿using System;

using System.Text;

namespace My.CrossCuttingConcerns.ExtensionMethods;

public static class HttpClientExtensions
{
    public static void UseBasicAuthentication(this HttpClient client, string userName, string password)
    {
        var byteArray = Encoding.UTF8.GetBytes(userName + ":" + password);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
    }

    public static void UseBearerToken(this HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
