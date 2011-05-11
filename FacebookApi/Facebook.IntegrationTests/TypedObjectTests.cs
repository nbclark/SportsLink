﻿namespace Facebook.Tests
{
    using Facebook.Graph;
    using Xunit;

    public class TypedObjectTests
    {
        private FacebookApp app;
        public TypedObjectTests()
        {
            app = new FacebookApp();
            app.MaxRetries = 0;
            //app.Session = new FacebookSession
            //{
            //    AccessToken = ConfigurationManager.AppSettings["AccessToken"],
            //};
        }

        [Fact]
        public void Get_User_Info_Typed()
        {
            var user = app.Get<User>("/totten");
            Assert.NotNull(user.FirstName);
        }
    }
}
