﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtualAssistantSample.FunctionalTests.Configuration;
using VirtualAssistantSample.Models;
using VirtualAssistantSample.Tests;
using VirtualAssistantSample.Tests.Utterances;

namespace VirtualAssistantSample.FunctionalTests
{
    /// <summary>
    /// These tests validate scenarios when the SkillSample bot is connected to the VirtualAssistantSample.
    /// </summary>
    [TestClass]
    [TestCategory("FunctionalTests")]
    [TestCategory("SkillSample")]
    public class SkillSampleTests : BotTestBase
    {
        [TestMethod]
        public async Task Test_Sample_Utterance()
        {
            await Assert_Utterance_Triggers_SkillSample();
        }

        /// <summary>
        /// Assert that a connected SkillSample is triggered by a sample utterance and completes the VA dialog.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task Assert_Utterance_Triggers_SkillSample()
        {
            var profileState = new UserProfileState { Name = GeneralUtterances.Name };
            var namePromptVariations = AllResponsesTemplates.ExpandTemplate("NamePrompt");
            var haveNameMessageVariations = AllResponsesTemplates.ExpandTemplate("HaveNameMessage", profileState);

            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(2));

            var testBot = new TestBotClient(new EnvironmentBotTestConfiguration());

            await testBot.StartConversation(cancellationTokenSource.Token);
            await new GreetingTests().Assert_New_User_Greeting(cancellationTokenSource, testBot);
            await testBot.SendMessageAsync(GeneralUtterances.SkillSample, cancellationTokenSource.Token);
            await testBot.AssertReplyOneOf(namePromptVariations, cancellationTokenSource.Token);
            await testBot.SendMessageAsync(GeneralUtterances.Name, cancellationTokenSource.Token);
            await testBot.AssertReplyOneOf(haveNameMessageVariations, cancellationTokenSource.Token);
        }
    }
}