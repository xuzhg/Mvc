// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using FormatterWebSite;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.TestHost;
using Microsoft.Framework.DependencyInjection;
using Xunit;

namespace Microsoft.AspNet.Mvc.FunctionalTests
{
    public class StreamOutputFormatterTest
    {
        private readonly Action<IApplicationBuilder> _app = new Startup().Configure;

        [Theory]
        [InlineData("SimpleMemoryStream", null)]
        [InlineData("MemoryStreamWithContentType", "text/html")]
        [InlineData("MemoryStreamWithContentTypeFromProduces", "text/plain")]
        [InlineData("MemoryStreamWithContentTypeFromProducesWithMultipleValues", "text/html")]
        [InlineData("MemoryStreamOverridesContentTypeWithProduces", "text/plain")]
        public async Task StreamOutputFormatter_ReturnsAppropriateContentAndContentType(string actionName, string contentType)
        {
            // Arrange
            var server = TestServer.Create(_app, AddServices);
            var client = server.CreateClient();

            // Act
            var response = await client.GetAsync("http://localhost/Stream/" + actionName);
            var body = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(contentType, response.Content.Headers.ContentType?.ToString());

            Assert.Equal("Sample text from a stream", body);
        }

        private static void AddServices(IServiceCollection services)
        {
            TestHelper.AddServices(services, nameof(FormatterWebSite));
        }
    }
}
