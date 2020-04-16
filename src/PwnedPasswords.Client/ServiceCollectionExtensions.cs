﻿using PwnedPasswords.Client;
using System;
using System.Net.Http;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for registering the <see cref="IPwnedPasswordsClient"/> with the <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        private static readonly Action<HttpClient> DefaultConfigureClientFunction = client =>
        {
            client.BaseAddress = new Uri("https://api.pwnedpasswords.com");
            client.DefaultRequestHeaders.Add("User-Agent", nameof(PwnedPasswordsClient));
        };

        /// <summary>
        /// Adds the <see cref="IHttpClientFactory"/> and related services to the <see cref="IServiceCollection"/>
        /// and configures a binding between the <see cref="IPwnedPasswordsClient"/> and a named <see cref="HttpClient"/>
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="name">The logical name of the <see cref="HttpClient"/> to configure.</param>
        /// <param name="configureClient">A delegate that is used to configure the <see cref="HttpClient"/> used by <see cref="IPwnedPasswordsClient"/></param>
        /// <returns>An <see cref="IHttpClientBuilder"/>that can be used to configure the client.</returns>
        /// <remarks>
        ///  <see cref="HttpClient"/> instances that apply the provided configuration can
        ///  be retrieved using <see cref="IHttpClientFactory.CreateClient(System.String)"/>
        ///  and providing the matching name.
        ///  <see cref="IPwnedPasswordsClient"/> instances constructed with the appropriate <see cref="HttpClient"/>
        ///  can be retrieved from <see cref="System.IServiceProvider.GetService(System.Type)"/> (and related
        ///  methods) by providing <see cref="IPwnedPasswordsClient"/> as the service type.
        /// </remarks>
        public static IHttpClientBuilder AddPwnedPasswordHttpClient(this IServiceCollection services, string name, Action<HttpClient> configureClient)
        {
            return services.AddPwnedPasswordHttpClient(name, configureClient, options => { });
        }

        /// <summary>
        /// Adds the <see cref="IHttpClientFactory"/> and related services to the <see cref="IServiceCollection"/>
        /// and configures a binding between the <see cref="IPwnedPasswordsClient"/> and a named <see cref="HttpClient"/>
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="name">The logical name of the <see cref="HttpClient"/> to configure.</param>
        /// <param name="configureClient">A delegate that is used to configure the <see cref="HttpClient"/> used by <see cref="IPwnedPasswordsClient"/></param>
        /// <param name="configureOptions">A delegate that is used to configure the <see cref="PwnedPasswordsClientOptions"/></param>
        /// <returns>An <see cref="IHttpClientBuilder"/>that can be used to configure the client.</returns>
        /// <remarks>
        ///  <see cref="HttpClient"/> instances that apply the provided configuration can
        ///  be retrieved using <see cref="IHttpClientFactory.CreateClient(System.String)"/>
        ///  and providing the matching name.
        ///  <see cref="IPwnedPasswordsClient"/> instances constructed with the appropriate <see cref="HttpClient"/>
        ///  can be retrieved from <see cref="System.IServiceProvider.GetService(System.Type)"/> (and related
        ///  methods) by providing <see cref="IPwnedPasswordsClient"/> as the service type.
        /// </remarks>
        public static IHttpClientBuilder AddPwnedPasswordHttpClient(this IServiceCollection services, string name,
            Action<HttpClient> configureClient, Action<PwnedPasswordsClientOptions> configureOptions)
        {
            services.Configure(configureOptions);
            return services
                .AddHttpClient<IPwnedPasswordsClient, PwnedPasswordsClient>(name, configureClient)
                .ConfigureHttpClient((provider, client) =>
                {
                    var options = provider.GetRequiredService<IOptions<PwnedPasswordsClientOptions>>().Value;
                    if (options.AddPadding)
                    {
                        client.DefaultRequestHeaders.Add("Add-Padding", "true");
                    }
                });

        }

        /// <summary>
        /// Adds the <see cref="IHttpClientFactory"/> and related services to the <see cref="IServiceCollection"/>
        /// and configures a binding between the <see cref="IPwnedPasswordsClient"/> and an <see cref="HttpClient"/>
        /// named <see cref="PwnedPasswordsClient.DefaultName"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configureClient">A delegate that is used to configure the <see cref="HttpClient"/> used by <see cref="IPwnedPasswordsClient"/></param>
        /// <returns>An <see cref="IHttpClientBuilder"/>that can be used to configure the client.</returns>
        /// <remarks>
        ///  <see cref="HttpClient"/> instances that apply the provided configuration can
        ///  be retrieved using <see cref="IHttpClientFactory.CreateClient(System.String)"/>
        ///  and providing the matching name.
        ///  <see cref="IPwnedPasswordsClient"/> instances constructed with the appropriate <see cref="HttpClient"/>
        ///  can be retrieved from <see cref="System.IServiceProvider.GetService(System.Type)"/> (and related
        ///  methods) by providing <see cref="IPwnedPasswordsClient"/> as the service type.
        /// </remarks>
        public static IHttpClientBuilder AddPwnedPasswordHttpClient(this IServiceCollection services, Action<HttpClient> configureClient)
        {
            return services.AddPwnedPasswordHttpClient(PwnedPasswordsClient.DefaultName, configureClient, options => { });
        }

        /// <summary>
        /// Adds the <see cref="IHttpClientFactory"/> and related services to the <see cref="IServiceCollection"/>
        /// and configures a binding between the <see cref="IPwnedPasswordsClient"/> and an <see cref="HttpClient"/>
        /// named <see cref="PwnedPasswordsClient.DefaultName"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configureClient">A delegate that is used to configure the <see cref="HttpClient"/> used by <see cref="IPwnedPasswordsClient"/></param>
        /// <param name="configureOptions">A delegate that is used to configure the <see cref="PwnedPasswordsClientOptions"/></param>
        /// <returns>An <see cref="IHttpClientBuilder"/>that can be used to configure the client.</returns>
        /// <remarks>
        ///  <see cref="HttpClient"/> instances that apply the provided configuration can
        ///  be retrieved using <see cref="IHttpClientFactory.CreateClient(System.String)"/>
        ///  and providing the matching name.
        ///  <see cref="IPwnedPasswordsClient"/> instances constructed with the appropriate <see cref="HttpClient"/>
        ///  can be retrieved from <see cref="System.IServiceProvider.GetService(System.Type)"/> (and related
        ///  methods) by providing <see cref="IPwnedPasswordsClient"/> as the service type.
        /// </remarks>
        public static IHttpClientBuilder AddPwnedPasswordHttpClient(this IServiceCollection services, 
            Action<HttpClient> configureClient, Action<PwnedPasswordsClientOptions> configureOptions)
        {
            return services.AddPwnedPasswordHttpClient(PwnedPasswordsClient.DefaultName, configureClient, configureOptions);
        }

        /// <summary>
        /// Adds the <see cref="IHttpClientFactory"/> and related services to the <see cref="IServiceCollection"/>
        /// and configures a binding between the <see cref="IPwnedPasswordsClient"/> and an <see cref="HttpClient"/>
        /// named <see cref="PwnedPasswordsClient.DefaultName"/> to use the public HaveIBeenPwned API 
        /// at "https://api.pwnedpasswords.com"
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>An <see cref="IHttpClientBuilder"/>that can be used to configure the client.</returns>
        /// <remarks>
        ///  <see cref="HttpClient"/> instances that apply the provided configuration can
        ///  be retrieved using <see cref="IHttpClientFactory.CreateClient(System.String)"/>
        ///  and providing the matching name.
        ///  <see cref="IPwnedPasswordsClient"/> instances constructed with the appropriate <see cref="HttpClient"/>
        ///  can be retrieved from <see cref="System.IServiceProvider.GetService(System.Type)"/> (and related
        ///  methods) by providing <see cref="IPwnedPasswordsClient"/> as the service type.
        /// </remarks>
        public static IHttpClientBuilder AddPwnedPasswordHttpClient(this IServiceCollection services)
        {
            return services.AddPwnedPasswordHttpClient(PwnedPasswordsClient.DefaultName, DefaultConfigureClientFunction);
        }

        /// <summary>
        /// Adds the <see cref="IHttpClientFactory"/> and related services to the <see cref="IServiceCollection"/>
        /// and configures a binding between the <see cref="IPwnedPasswordsClient"/> and an <see cref="HttpClient"/>
        /// named <see cref="PwnedPasswordsClient.DefaultName"/> to use the public HaveIBeenPwned API 
        /// at "https://api.pwnedpasswords.com"
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="minimumFrequencyToConsiderPwned">The minimum frequency to consider a password to be Pwned.  For example, setting this to 20 means only PwnedPasswords seen 20 times or more are considered pwned.</param>
        /// <returns>An <see cref="IHttpClientBuilder"/>that can be used to configure the client.</returns>
        /// <remarks>
        ///  <see cref="HttpClient"/> instances that apply the provided configuration can
        ///  be retrieved using <see cref="IHttpClientFactory.CreateClient(System.String)"/>
        ///  and providing the matching name.
        ///  <see cref="IPwnedPasswordsClient"/> instances constructed with the appropriate <see cref="HttpClient"/>
        ///  can be retrieved from <see cref="System.IServiceProvider.GetService(System.Type)"/> (and related
        ///  methods) by providing <see cref="IPwnedPasswordsClient"/> as the service type.
        /// </remarks>
        public static IHttpClientBuilder AddPwnedPasswordHttpClient(this IServiceCollection services, int minimumFrequencyToConsiderPwned)
        {
            return services.AddPwnedPasswordHttpClient(DefaultConfigureClientFunction,
                opts => opts.MinimumFrequencyToConsiderPwned = minimumFrequencyToConsiderPwned);
        }

        /// <summary>
        /// Adds the <see cref="IHttpClientFactory"/> and related services to the <see cref="IServiceCollection"/>
        /// and configures a binding between the <see cref="IPwnedPasswordsClient"/> and an <see cref="HttpClient"/>
        /// named <see cref="PwnedPasswordsClient.DefaultName"/> to use the public HaveIBeenPwned API 
        /// at "https://api.pwnedpasswords.com"
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="minimumFrequencyToConsiderPwned">The minimum frequency to consider a password to be Pwned.  For example, setting this to 20 means only PwnedPasswords seen 20 times or more are considered pwned.</param>
        /// <param name="addPadding">If true, requests for the API to add padding to the response to increase privacy.</param>
        /// <returns>An <see cref="IHttpClientBuilder"/>that can be used to configure the client.</returns>
        /// <remarks>
        ///  <see cref="HttpClient"/> instances that apply the provided configuration can
        ///  be retrieved using <see cref="IHttpClientFactory.CreateClient(System.String)"/>
        ///  and providing the matching name.
        ///  <see cref="IPwnedPasswordsClient"/> instances constructed with the appropriate <see cref="HttpClient"/>
        ///  can be retrieved from <see cref="System.IServiceProvider.GetService(System.Type)"/> (and related
        ///  methods) by providing <see cref="IPwnedPasswordsClient"/> as the service type.
        /// </remarks>
        public static IHttpClientBuilder AddPwnedPasswordHttpClient(
            this IServiceCollection services, 
            int minimumFrequencyToConsiderPwned,
            bool addPadding)
        {
            return services.AddPwnedPasswordHttpClient(DefaultConfigureClientFunction,
                opts =>
                {
                    opts.MinimumFrequencyToConsiderPwned = minimumFrequencyToConsiderPwned;
                    opts.AddPadding = addPadding;
                });
        }
    }
}
