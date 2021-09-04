using System;
using System.Collections.Generic;
using PactNet;
using PactNet.Infrastructure.Outputters;
using Xunit;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using ContractTests.XUnitHelpers;

namespace ContractTests
{
    public class ProviderContractTests : IDisposable
    {
        private string _providerUri { get; }
        private IWebHost _webHost { get; }
        private ITestOutputHelper _output { get; }

        public ProviderContractTests(ITestOutputHelper output)
        {
            _output = output;
            _providerUri = "http://localhost:3001";

            _webHost = WebHost.CreateDefaultBuilder()
                .UseUrls(_providerUri)
                .UseStartup<TestStartup>()
                .Build();

            _webHost.Start();
        }

        [Fact]
        public void should_verify_pacts()
        {
            var verifierConfig = new PactVerifierConfig
            {
                Outputters = new List<IOutput>
                {
                    new XUnitOutputter(_output)
                },
                Verbose = true,
                PublishVerificationResults = true,
                ProviderVersion = $"1.0.{DateTime.Now.ToString("dd.MM.HH.mm")}"
            };

            new PactVerifier(verifierConfig)
                // .ProviderState($"{_providerUri}/provider-states")
                .ServiceProvider("product-service", _providerUri)
                .HonoursPactWith("price-service")
                // .PactUri(@"..\..\..\..\..\_pacts\price-service-product-service.json")
                .PactBroker("http://localhost:9292", new PactUriOptions("admin", "admin"))
                .Verify();

            new PactVerifier(verifierConfig)
                .ServiceProvider("product-service", _providerUri)
                .HonoursPactWith("search-service")
                .PactBroker("http://localhost:9292", new PactUriOptions("admin", "admin"))
                .Verify();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}