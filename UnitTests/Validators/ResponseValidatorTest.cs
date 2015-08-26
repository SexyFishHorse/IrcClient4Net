namespace SexyFishHorse.Irc.Client.UnitTests.Validators
{
    using System;
    using Ploeh.AutoFixture;
    using SexyFishHorse.Irc.Client.Models;
    using SexyFishHorse.Irc.Client.Validators;
    using Should;
    using Xunit;

    public class ResponseValidatorTest
    {
        [Fact]
        public void ValidateCommand_MessageIsNull_ThrowValidationException()
        {
            var fixture = new Fixture();
            var instance = new ResponseValidator();

            Assert.Throws<ResponseValidationException>(() => instance.ValidateCommand(null, fixture.Create<string>()));
        }

        [Theory]
        [InlineData(" ")]
        [InlineData((string)null)]
        [InlineData("")]
        public void ValidateCommand_ExpectedCommandIsInvalid_ThrowArgumentException(string expectedCommand)
        {
            var fixture = new Fixture();
            var instance = new ResponseValidator();

            var exception = Assert.Throws<ArgumentException>(
                () => instance.ValidateCommand(fixture.Create<IrcMessage>(), expectedCommand));

            exception.ParamName.ShouldEqual("expectedCommand");
        }

        [Fact]
        public void ValidateCommand_CommandAndMessageDoesntMatch_ThrowValidationException()
        {
            var fixture = new Fixture();
            var instance = new ResponseValidator();

            var message = fixture.Create<IrcMessage>();
            message.Command = "001";

            const string ExpectedCommand = "002";

            var exception =
                Assert.Throws<ResponseValidationException>(() => instance.ValidateCommand(message, ExpectedCommand));

            exception.IrcMessage.ShouldEqual(message);
            exception.ExpectedCommand.ShouldEqual(ExpectedCommand);
        }

        [Fact]
        public void ValidateCommand_CommandAndMessageMatch_DoNothing()
        {
            var fixture = new Fixture();
            var instance = new ResponseValidator();

            var message = fixture.Create<IrcMessage>();
            message.Command = "001";

            const string ExpectedCommand = "001";

            instance.ValidateCommand(message, ExpectedCommand);
        }
    }
}
