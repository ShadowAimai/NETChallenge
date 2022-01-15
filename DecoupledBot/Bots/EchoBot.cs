// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.15.0

using DecoupledBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DecoupledBot.Bots
{
    public class EchoBot : ActivityHandler
    {
        static readonly HttpClient client = new HttpClient();

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {

            string output_message = "";

            try
            {
                HttpResponseMessage response = await client.GetAsync(string.Format("https://stooq.com/q/l/?s={0}&f=sd2t2ohlcv&h&e=csv", turnContext.Activity.Text));
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                var list = Utils.CSVConverter.GetValues(responseBody);

                foreach (var item in list)
                {

                    if (item.Close != "N/D")
                    {
                        output_message = String.Format("{0} quote is {1}  per share", item.Symbol, item.Close);
                    }

                }

            }
            catch (Exception ex)
            {
                output_message = "";
            }

            var replyText = $"{output_message}";
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
