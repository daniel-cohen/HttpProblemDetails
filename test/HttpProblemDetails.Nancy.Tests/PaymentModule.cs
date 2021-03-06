﻿using System;
using Nancy;
using HttpProblemDetails.Tests;

namespace HttpProblemDetails.Nancy.Tests
{
    public class PaymentModule : NancyModule
    {
        public PaymentModule()
        {
            Get("/payment/{account}", async args =>
            {
                if (args.account == "12345")
                {
                    var problemDetail = new InsufficientCashProblem
                    {
                        Type = new Uri("https://example.com/probs/out-of-credit").ToString(),
                        Title = "You do not have enough credit.",
                        Status = (int)HttpStatusCode.Forbidden,
                        Detail = "Your current balance is 30, but that costs 50.",
                        Instance = new Uri("/account/12345/msgs/abc", UriKind.Relative).ToString()
                    };
                    throw new InsufficientCashException(problemDetail);
                }

                return await Response
                    .AsText("OK")
                    .WithStatusCode(HttpStatusCode.OK);
            });
        }
    }
}
