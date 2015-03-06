﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.TestHost;
using Microsoft.Framework.DependencyInjection;
using RazorViewEngineOptionsWebsite;
using Xunit;

namespace Microsoft.AspNet.Mvc.FunctionalTests
{
    public class RazorViewEngineOptionsTest
    {
        private readonly Action<IApplicationBuilder> _app = new Startup().Configure;

        [Fact]
        public async Task RazorViewEngine_UsesFileProviderOnViewEngineOptionsToLocateViews()
        {
            // Arrange
            var expectedMessage = "Hello test-user, this is /RazorViewEngineOptions_Home";
            var server = TestServer.Create(_app, AddServices);
            var client = server.CreateClient();

            // Act
            var response = await client.GetStringAsync("http://localhost/RazorViewEngineOptions_Home?User=test-user");

            // Assert
            Assert.Equal(expectedMessage, response);
        }

        [Fact]
        public async Task RazorViewEngine_UsesFileProviderOnViewEngineOptionsToLocateAreaViews()
        {
            // Arrange
            var expectedMessage = "Hello admin-user, this is /Restricted/RazorViewEngineOptions_Admin/Login";
            var server = TestServer.Create(_app, AddServices);
            var client = server.CreateClient();
            var target = "http://localhost/Restricted/RazorViewEngineOptions_Admin/Login?AdminUser=admin-user";

            // Act
            var response = await client.GetStringAsync(target);

            // Assert
            Assert.Equal(expectedMessage, response);
        }

        private static void AddServices(IServiceCollection services)
        {
            TestHelper.AddServices(services, nameof(RazorViewEngineOptionsWebsite));
        }
    }
}