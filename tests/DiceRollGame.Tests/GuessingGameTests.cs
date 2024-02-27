using DiceRollGame.Game;
using DiceRollGame.UserCommunication;
using Moq;

namespace DiceRollGame.Tests;

public class GuessingGameTests
{
    private Mock<IDice> _diceMock;
    private Mock<IUserCommunication> _userCommunicationMock;
    private GuessingGame _cut;

    [SetUp]
    public void Setup()
    {
        _diceMock = new Mock<IDice>();
        _userCommunicationMock = new Mock<IUserCommunication>();
        _cut = new GuessingGame(_diceMock.Object, _userCommunicationMock.Object);
    }

    [Test]
    public void Play_ShallReturnVictory_IfTheUserGuessesTheCorrectNumberOnTheFirstTry()
    {
        const int NumberOnDice = 3;
        _diceMock.Setup(mock => mock.Roll()).Returns(NumberOnDice);

        _userCommunicationMock
            .Setup(mock => mock.ReadInteger(It.IsAny<string>()))
            .Returns(NumberOnDice);

        var gameResult = _cut.Play();

        Assert.That(gameResult, Is.EqualTo(GameResult.Victory));
    }

    [Test]
    public void Play_ShallReturnVictory_IfTheUserGuessesTheCorrectNumberOnTheThirdTry()
    {
        SetupUserGuessingTheCorrectNumberOnTheThirdTry();

        var gameResult = _cut.Play();

        Assert.That(gameResult, Is.EqualTo(GameResult.Victory));
    }

    [Test]
    public void Play_ShallReturnLoss_IfTheUserNeverGuessesTheCorrectNumber()
    {
        const int NumberOnDice = 3;
        _diceMock.Setup(mock => mock.Roll()).Returns(NumberOnDice);
        const int UserNumber = 1;
        _userCommunicationMock
            .Setup(mock => mock.ReadInteger(It.IsAny<string>()))
            .Returns(UserNumber);

        var gameResult = _cut.Play();

        Assert.That(gameResult, Is.EqualTo(GameResult.Loss));
    }

    [Test]
    public void Play_ShallReturnLoss_IfTheUserGuessesTheCorrectNumberOnTheFourthTry()
    {
        const int NumberOnDice = 3;
        _diceMock.Setup(mock => mock.Roll()).Returns(NumberOnDice);

        _userCommunicationMock
            .SetupSequence(mock => mock.ReadInteger(It.IsAny<string>()))
            .Returns(1)
            .Returns(2)
            .Returns(4)
            .Returns(NumberOnDice);

        var gameResult = _cut.Play();

        Assert.That(gameResult, Is.EqualTo(GameResult.Loss));
    }

    [Test]
    public void Play_ShallShowWelcomeMessage()
    {
        var gameResult = _cut.Play();

        _userCommunicationMock.Verify(
            mock => mock.ShowMessage(
                "Dice rolled. Guess what number it shows in 3 tries."), Times.Once);
    }

    [Test]
    public void Play_ShallAskForNumberThreeTimes_IfTheUserGuessesTheCorrectNumberOnTheThirdTry()
    {
        SetupUserGuessingTheCorrectNumberOnTheThirdTry();

        var gameResult = _cut.Play();

        _userCommunicationMock.Verify(
            mock => mock.ReadInteger(
                "Enter a number:"),
            Times.Exactly(3));
    }

    [Test]
    public void Play_ShallShowWrongNumberTwice_IfTheUserGuessesTheCorrectNumberOnTheThirdTry()
    {
        SetupUserGuessingTheCorrectNumberOnTheThirdTry();

        var gameResult = _cut.Play();

        _userCommunicationMock.Verify(
            mock => mock.ShowMessage("Wrong guess. Try again."),
            Times.Exactly(2));
    }

    [TestCase(GameResult.Victory, "You win!")]
    [TestCase(GameResult.Loss, "You lose :(")]
    public void PrintResult_ShallShowTheCorrectMessage(GameResult gameResult, string expectedMessage)
    {
        _cut.PrintResult(gameResult);
        _userCommunicationMock.Verify(
            mock => mock.ShowMessage(expectedMessage));
    }

    private void SetupUserGuessingTheCorrectNumberOnTheThirdTry()
    {
        const int NumberOnDice = 3;
        _diceMock.Setup(mock => mock.Roll()).Returns(NumberOnDice);
        _userCommunicationMock
            .SetupSequence(mock => mock.ReadInteger(It.IsAny<string>()))
            .Returns(1)
            .Returns(2)
            .Returns(NumberOnDice);
    }
}