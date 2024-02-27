using DiceRollGame.Game;
using DiceRollGame.UserCommunication;

var random = new Random();
var dice = new Dice(random);
var userCommunication = new ConsoleUserCommunication();
var game = new GuessingGame(dice, userCommunication);

GameResult gameResult = game.Play();
game.PrintResult(gameResult);

Console.ReadKey();