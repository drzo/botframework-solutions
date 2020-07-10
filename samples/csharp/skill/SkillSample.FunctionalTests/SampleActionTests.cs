﻿using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using SkillSample.FunctionalTests.Bot;
using SkillSample.FunctionalTests.Configuration;
using SkillSample.Tests;
using SkillSample.Tests.Utterances;

namespace SkillSample.FunctionalTests
{
    [TestClass]
    [TestCategory("FunctionalTests")]
    [TestCategory("SampleAction")]
    public class SampleActionTests : SkillTestBase
    {
        [TestMethod]
        public async Task Test_Action_SampleAction()
        {
            await Assert_Action_Triggers_SkillAction();
        }

        [TestMethod]
        public async Task Test_ActionWithInput_SampleAction()
        {
            await Assert_ActionWithInput_Triggers_SkillAction();
        }

        public async Task Assert_Action_Triggers_SkillAction()
        {
            var profileState = new { Name = SampleDialogUtterances.NamePromptResponse };
            var introTextVariations = AllResponsesTemplates.ExpandTemplate("IntroText");
            var namePromptTextVariations = AllResponsesTemplates.ExpandTemplate("NamePromptText");
            var haveNameMessageTextVariations = AllResponsesTemplates.ExpandTemplate("HaveNameMessageText", profileState);

            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(2));

            var testBot = new TestBotClient(new EnvironmentBotTestConfiguration());

            await testBot.StartConversation(cancellationTokenSource.Token);
            await testBot.SendEventAsync("startConversation", cancellationTokenSource.Token);
            await testBot.AssertReplyOneOf(introTextVariations, cancellationTokenSource.Token);
            await testBot.SendEventAsync(SampleDialogUtterances.EventName);
            await testBot.AssertReplyOneOf(namePromptTextVariations, cancellationTokenSource.Token);
            await testBot.SendMessageAsync(SampleDialogUtterances.NamePromptResponse);
            await testBot.AssertReplyOneOf(haveNameMessageTextVariations, cancellationTokenSource.Token);
        }

        public async Task Assert_ActionWithInput_Triggers_SkillAction()
        {
            var profileState = new { Name = SampleDialogUtterances.NamePromptResponse };
            var introTextVariations = AllResponsesTemplates.ExpandTemplate("IntroText");
            var haveNameMessageTextVariations = AllResponsesTemplates.ExpandTemplate("HaveNameMessageText", profileState);

            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(2));

            var testBot = new TestBotClient(new EnvironmentBotTestConfiguration());

            await testBot.StartConversation(cancellationTokenSource.Token);
            await testBot.SendEventAsync("startConversation", cancellationTokenSource.Token);
            await testBot.AssertReplyOneOf(introTextVariations, cancellationTokenSource.Token);
            await testBot.SendEventAsync(SampleDialogUtterances.EventName, JObject.FromObject(profileState));
            await testBot.AssertReplyOneOf(haveNameMessageTextVariations, cancellationTokenSource.Token);
        }
    }
}
