using Library.Framework.Test.HttpService.Entities;
using Library.HttpServices.Builders;
using Library.HttpServices.Builders.Extensions;
using Library.HttpServices.Builders.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Xunit;

namespace Library.Framework.Test.HttpService
{
    public class HttpRestBuilderTest
    {
        private readonly IHttpBuilder _httpRestBuilder = null;

        public HttpRestBuilderTest()
        {
            //_httpRestBuilder = new HttpRestBuilder("https://api-pub.bitfinex.com");
            _httpRestBuilder = new HttpRestBuilder("https://api-pub.bitfinex.com");
        }

        [Fact]
        public void Test_Http_Get_Request()
        {
            //var response = _httpRestBuilder.GetSync("v2/platform/status");
            var response = _httpRestBuilder.GetSync("v2/book/tBTCUSD/P0");

            //var result = response.ExtractArrayToObjectSync<Operative>();

            var result = response.ExtractArrayToListObjectSync<Book>();

            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }
    }
}
