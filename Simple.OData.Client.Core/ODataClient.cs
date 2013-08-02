using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using Simple.OData.Client.Extensions;

namespace Simple.OData.Client
{
    public partial class ODataClient
    {
        private readonly ODataClientSettings _settings;
        private readonly ISchema _schema;
        private readonly RequestBuilder _requestBuilder;
        private readonly RequestRunner _requestRunner;

        public ODataClient(string urlBase)
            : this(new ODataClientSettings { UrlBase = urlBase })
        {
        }

        public ODataClient(ODataClientSettings settings)
        {
            _settings = settings;
            _schema = Client.Schema.Get(_settings.UrlBase, _settings.Credentials);

            _requestBuilder = new CommandRequestBuilder(_settings.UrlBase, _settings.Credentials);
            _requestRunner = new CommandRequestRunner(_settings);
            _requestRunner.BeforeRequest = _settings.BeforeRequest;
            _requestRunner.AfterResponse = _settings.AfterResponse;
        }

        public ODataClient(ODataBatch batch)
        {
            _settings = batch.Settings;
            _schema = Client.Schema.Get(_settings.UrlBase, _settings.Credentials);

            _requestBuilder = batch.RequestBuilder;
            _requestRunner = batch.RequestRunner;
        }

        public ISchema Schema
        {
            get { return _schema; }
        }

        public string SchemaAsString
        {
            get { return SchemaProvider.FromUrl(_settings.UrlBase, _settings.Credentials).SchemaAsString; }
        }

        public static ISchema ParseSchemaString(string schemaString)
        {
            return SchemaProvider.FromMetadata(schemaString).Schema;
        }

        public static void SetPluralizer(IPluralizer pluralizer)
        {
            StringExtensions.SetPluralizer(pluralizer);
        }

        public IClientWithCommand For(string collectionName)
        {
            return new ODataClientWithCommand(this, _schema).For(collectionName);
        }

#if !PORTABLE_IOS
        public string FormatFilter(string collection, dynamic filterExpression)
        {
            if (filterExpression is FilterExpression)
            {
                var clientWithCommand = new ODataClientWithCommand(this, _schema);
                string filter = (filterExpression as FilterExpression)
                    .Format(clientWithCommand, collection);

                return clientWithCommand
                    .For(collection)
                    .Filter(filter).CommandText;
            }
            else
            {
                throw new InvalidOperationException("Unable to cast dynamic object to FilterExpression");
            }
        }
#endif

        public string FormatFilter<T>(string collection, Expression<Func<T, bool>> filterExpression)
        {
            var clientWithCommand = new ODataClientWithCommand(this, _schema);
            throw new NotImplementedException();
        }
    }
}