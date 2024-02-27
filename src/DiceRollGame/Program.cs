using DiceRollGame.Game;

var random = new Random();
var dice = new Dice(random);
var game = new GuessingGame(dice);

GameResult gameResult = game.Play();
GuessingGame.PrintResult(gameResult);

Console.ReadKey();