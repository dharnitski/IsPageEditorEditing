using FluentAssertions;
using NUnit.Framework;
using Sitecore;
using Sitecore.Collections;
using Sitecore.Configuration;
using Sitecore.FakeDb;
using Sitecore.FakeDb.Sites;
using Sitecore.Sites;


namespace IsPageEditorEditing
{
    [TestFixture]
    public class IsPageEditorEditingTests
    {
        [Test]
        public void SitecoreShipShouldBeConfigured()
        {
            using (var db = new Db()
            {
                new DbItem("home")
            })
            {
                var home = db.GetItem("/sitecore/content/home");

                home.Should().NotBeNull();
            }
        }

        [Test]
        public void TestIsPageEditorEditing()
        {
            var fakeSiteContext = new FakeSiteContext(
                new StringDictionary
                {
                    {"enableWebEdit", "true"},
                    {"masterDatabase", "master"},
                });

            using (new SiteContextSwitcher(fakeSiteContext))
            {
                //act
                Context.Site.SetDisplayMode(DisplayMode.Edit, DisplayModeDuration.Remember);

                //assert
                Context.PageMode.IsPageEditorEditing.Should().BeTrue();

                //Only works in Sitecore 8+
                Context.PageMode.IsExperienceEditorEditing.Should().BeTrue();
            }
        }

        [Test]
        public void TestIsPageEditorEditingWithExtraAssertions()
        {
            //this does not work for SiteManager.GetSite("shell")
            //var shellAttributes = new StringDictionary
            //    {
            //        {"name", "shell"},
            //        {"domain", "sitecore"}
            //    };

            //var shell = new FakeSiteContext(shellAttributes);
            //SiteContextFactory.Sites.Add(shell.SiteInfo);

            //Assert site configuration defined in app.config
            Factory.GetSite("shell").Should().NotBeNull();
            Factory.GetSite("shell").Domain.Name.Should().NotBeNullOrWhiteSpace();
            SiteManager.GetSite("shell").Should().NotBeNull();

            var fakeSiteContext = new FakeSiteContext(
                new StringDictionary
                {
                    {"name", "mywebsite"},
                    {"enableWebEdit", "true"},
                    {"database", "web"},
                    {"masterDatabase", "master"},
                });
            fakeSiteContext.Name.Should().Be("mywebsite");

            using (new SiteContextSwitcher(fakeSiteContext))
            {
                SiteManager.CanEnter("shell", Context.User).Should().BeTrue();

                //check prerequisites
                Context.Site.Name.Should().Be("mywebsite");
                Context.Site.EnableWebEdit.Should().BeTrue();
                Context.Site.Database.Name.Should().Be("web");
                Context.Site.MasterDatabase.Name.Should().Be("master");
                Settings.CommerceServer.StandaloneEdition.Should().BeFalse();
                Settings.WebEdit.Enabled.Should().BeTrue();

                //act
                Context.Site.SetDisplayMode(DisplayMode.Edit, DisplayModeDuration.Remember);

                //assert
                Context.Site.DisplayMode.Should().Be(DisplayMode.Edit);
                Context.PageMode.IsPageEditorEditing.Should().BeTrue();
            }
        }
    }
}

