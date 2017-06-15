﻿// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog;
using Microsoft.Templates.Core.Test.Locations;
using Microsoft.Templates.Test.Artifacts;

using Xunit;

namespace Microsoft.Templates.Core.Test.PostActions.Catalog
{
    public class GenerateTestCertificatePostActionTest : IContextProvider
    {
        public string ProjectName { get; set; }
        public string OutputPath { get; set; }

        [Theory, MemberData("GetAllLanguages"), Trait("Type", "ProjectGeneration")]
        public void Execute_Ok(string language)
        {
            GenContext.Bootstrap(new UnitTestsTemplatesSource(), new FakeGenShell(), language);

            var projectName = "test";

            ProjectName = projectName;
            OutputPath = @".\TestData\tmp";

            GenContext.Current = this;

            Directory.CreateDirectory(GenContext.Current.OutputPath);
            File.Copy(Path.Combine(Environment.CurrentDirectory, "TestData\\TestProject\\Test.csproj"), Path.Combine(GenContext.Current.OutputPath, "Test.csproj"), true);

            var postAction = new GenerateTestCertificatePostAction("TestUser");

            postAction.Execute();

            var certFilePath = Path.Combine(GenContext.Current.OutputPath, $"{projectName}_TemporaryKey.pfx");

            Assert.True(File.Exists(certFilePath));

            File.Delete(certFilePath);
        }

        public static IEnumerable<object[]> GetAllLanguages()
        {
            yield return new object[] { "C#" };
            yield return new object[] { "VisualBasic" };
        }
    }
}
