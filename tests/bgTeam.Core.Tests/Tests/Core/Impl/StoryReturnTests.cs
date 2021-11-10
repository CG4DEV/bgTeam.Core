using System;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Core.Impl
{
    public class StoryReturnTests
    {
        [Fact]
        public void DependencyStoryFactory()
        {
            var (storyAccess, storyFactory, tStoryContext, story) = GetMocks();
            Assert.Throws<ArgumentNullException>("factory", () =>
            {
                new StoryReturn<ISomeContext>(storyAccess.Object, null, tStoryContext.Object);
            });
        }

        [Fact]
        public void Return()
        {
            var (storyAccess, storyFactory, tStoryContext, story) = GetMocks();
            var storyReturn = new StoryReturn<ISomeContext>(storyAccess.Object, storyFactory.Object, tStoryContext.Object);
            Assert.NotNull(storyReturn.ReturnAsync<IResult>().Result);
            storyAccess.Verify(x => x.CheckAccessAsync(It.IsAny<IStory<ISomeContext, IResult>>()));
        }

        [Fact]
        public async Task ReturnAsync()
        {
            var (storyAccess, storyFactory, tStoryContext, story) = GetMocks();
            var storyReturn = new StoryReturn<ISomeContext>(storyAccess.Object, storyFactory.Object, tStoryContext.Object);
            Assert.NotNull(await storyReturn.ReturnAsync<IResult>());
            storyAccess.Verify(x => x.CheckAccessAsync(It.IsAny<IStory<ISomeContext, IResult>>()));
        }

        private (
            Mock<IStoryAccess>,
            Mock<IStoryFactory>,
            Mock<ISomeContext>,
            Mock<IStory<ISomeContext, IResult>>)
            GetMocks()
        {
            var storyAccess = new Mock<IStoryAccess>();
            var storyFactory = new Mock<IStoryFactory>();
            var story = new Mock<IStory<ISomeContext, IResult>>();

            story.Setup(x => x.ExecuteAsync(It.IsAny<ISomeContext>()))
                .ReturnsAsync(new Mock<IResult>().Object);
            story.Setup(x => x.ExecuteAsync(It.IsAny<ISomeContext>()))
                .ReturnsAsync(new Mock<IResult>().Object);

            storyFactory.Setup(x => x.Create<ISomeContext, IResult>())
                .Returns(story.Object);

            var tStoryContext = new Mock<ISomeContext>();
            return (
                storyAccess,
                storyFactory,
                tStoryContext,
                story);
        }

        public interface ISomeContext
        {
        }

        public interface IResult
        {
        }
    }
}
