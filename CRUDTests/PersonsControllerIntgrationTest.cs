using FluentAssertions;
using System;
using Fizzler;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

namespace CRUDTests
{
    public class PersonsControllerIntgrationTest: IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        public PersonsControllerIntgrationTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        #region Index
        [Fact]
        public async Task Index_ToReturnView()
        {

            // Acts
            HttpResponseMessage response = await _client.GetAsync("Persons/Index");

            // Assert 
            response.Should().BeSuccessful();

            string responseBody = await response.Content.ReadAsStringAsync();

            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(responseBody);

            var document = html.DocumentNode;

            var table = document.QuerySelectorAll("table.persons");
            table.Should().NotBeNull();

            var div = document.QuerySelectorAll(".box");

            div.Should().NotBeNull();

        }

        #endregion
    }
}
