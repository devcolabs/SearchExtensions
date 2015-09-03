using System;
using System.Linq;
using NUnit.Framework;

namespace NinjaNye.SearchExtensions.Tests.Integration.Fluent.SearchTests
{
    [TestFixture]
    internal class ContainingTests : IDisposable
    {
        private readonly TestContext context = new TestContext();

        [Test]
        public void Containing_SearchWholeWordsOnly_ReturnsOnlyRecordsWithWholeWordMatches()
        {
            //Arrange

            //Act
            var result = this.context.TestModels.Search(x => x.StringOne)
                                                .Matching(SearchTypeEnum.WholeWords)
                                                .Containing("word");

            //Assert
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        public void Dispose()
        {
            this.context.Dispose();
        }        
    }

    [TestFixture]
    internal class ContainingAllTests : IDisposable
    {
        private readonly TestContext context = new TestContext();

        [Test]
        public void SearchContainingAll_OneTermSupplied_ReturnsOnlyRecordsContainingSearchTerm()
        {
            //Arrange
            const string searchTerm = "test";

            //Act
            var result = this.context.TestModels.Search(x => x.StringOne).ContainingAll(searchTerm);

            //Assert
            Assert.IsTrue(result.Any() && result.All(t => t.StringOne.IndexOf(searchTerm) >= 0));
        }

        [Test]
        public void SearchContainingAll_TwoTermsSupplied_ReturnsRecordsContainingBothSearchTerms()
        {
            //Arrange
            const string searchTerm1 = "test";
            const string searchTerm2 = "search";

            //Act
            var result = this.context.TestModels.Search(x => x.StringOne).ContainingAll(searchTerm1, searchTerm2);

            //Assert
            Assert.IsTrue(result.Any() && result.All(t => t.StringOne.IndexOf(searchTerm1) >= 0 && t.StringOne.IndexOf(searchTerm2) >= 0));
        }

        [Test]
        public void SearchContainingAll_TwoPropertiesAndTwoTermsSupplied_ReturnsDataWhereAllTermsAreMatched()
        {
            //Arrange
            const string searchTerm1 = "test";
            const string searchTerm2 = "search";
            const string searchTerm3 = "windmill";

            //Act
            var result = this.context.TestModels.Search(x => x.StringOne, x => x.StringTwo)
                             .ContainingAll(searchTerm1, searchTerm2, searchTerm3).ToList();

            //Assert
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(t => (t.StringOne.Contains(searchTerm1) || t.StringTwo.Contains(searchTerm1))
                                          && (t.StringOne.Contains(searchTerm2) || t.StringTwo.Contains(searchTerm2))
                                          && (t.StringOne.Contains(searchTerm3) || t.StringTwo.Contains(searchTerm3))
                              ));
        }

        [Test]
        public void ContainingAll_SearchAgainstOneProperty_DoesNotThrowAnException()
        {
            //Arrange
            
            //Act

            //Assert
            Assert.DoesNotThrow(() => this.context.TestModels.Search(x => x.StringOne).ContainingAll(x => x.StringTwo));
        }

        [Test]
        public void ContainingAll_SearchAgainstOneProperty_DoesNotReturnNull()
        {
            //Arrange
            
            //Act
            var result = this.context.TestModels.Search(x => x.StringOne).ContainingAll(x => x.StringTwo);

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void ContainingAll_SearchAgainstOneProperty_AllREsultsHaveAStringTwoInStringOne()
        {
            //Arrange
            
            //Act
            var result = this.context.TestModels.Search(x => x.StringOne).ContainingAll(x => x.StringTwo).ToList();

            //Assert
            Assert.IsTrue(result.Any(), "No records returned");
            Assert.IsTrue(result.All(x => x.StringOne.Contains(x.StringTwo)));
        }

        [Test]
        public void ContainingAll_SearchAgainstTwoProperties_AllRsultsHaveBothProperties()
        {
            //Arrange
            
            //Act
            var result = this.context.TestModels.Search(x => x.StringOne)
                                                .ContainingAll(x => x.StringTwo, x => x.StringThree)
                                                .ToList();

            //Assert
            Assert.IsTrue(result.Any(), "No records returned");
            Assert.IsTrue(result.All(x => x.StringOne.Contains(x.StringTwo) && x.StringOne.Contains(x.StringThree)));
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}