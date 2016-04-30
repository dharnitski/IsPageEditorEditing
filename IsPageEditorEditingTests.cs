using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Sitecore.FakeDb;


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

                Assert.IsNotNull(home);
            }
        }
    }
}
