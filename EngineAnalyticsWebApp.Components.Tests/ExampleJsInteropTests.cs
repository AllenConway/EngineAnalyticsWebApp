using Bunit;
using EngineAnalyticsWebApp.Components;
using Microsoft.JSInterop;

namespace EngineAnalyticsWebApp.Components.Tests
{
    public class ExampleJsInteropTests : BunitContext
    {
        [Fact]
        public async Task Prompt_InvokesShowPromptOnJsModule_ReturnsResult()
        {
            // Arrange
            JSInterop.Mode = JSRuntimeMode.Strict;
            var module = JSInterop.SetupModule("./_content/EngineAnalyticsWebApp.Components/exampleJsInterop.js");
            module.Setup<string>("showPrompt", "What is your name?").SetResult("Allen");
            var sut = new ExampleJsInterop(JSInterop.JSRuntime);

            // Act
            var result = await sut.Prompt("What is your name?");

            // Assert
            Assert.Equal("Allen", result);
        }

        [Fact]
        public async Task DisposeAsync_WhenModuleNotCreated_DoesNotThrow()
        {
            // Arrange
            var sut = new ExampleJsInterop(JSInterop.JSRuntime);

            // Act
            var exception = await Record.ExceptionAsync(async () => await sut.DisposeAsync());

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task DisposeAsync_WhenModuleCreated_DisposesModule()
        {
            // Arrange
            JSInterop.Mode = JSRuntimeMode.Strict;
            var module = JSInterop.SetupModule("./_content/EngineAnalyticsWebApp.Components/exampleJsInterop.js");
            module.Setup<string>("showPrompt", "Hi").SetResult("response");
            var sut = new ExampleJsInterop(JSInterop.JSRuntime);
            await sut.Prompt("Hi");

            // Act
            var exception = await Record.ExceptionAsync(async () => await sut.DisposeAsync());

            // Assert
            Assert.Null(exception);
        }
    }
}
