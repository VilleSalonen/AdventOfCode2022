using FluentAssertions;
using Sprache;

namespace Day2;

/*
--- Day 2: Rock Paper Scissors ---

The Elves begin to set up camp on the beach. To decide whose tent gets to be closest to the snack storage, a giant Rock Paper Scissors tournament is already in progress.

Rock Paper Scissors is a game between two players. Each game contains many rounds; in each round, the players each simultaneously choose one of Rock, Paper, or Scissors using a hand shape. Then, a winner for that round is selected: Rock defeats Scissors, Scissors defeats Paper, and Paper defeats Rock. If both players choose the same shape, the round instead ends in a draw.

Appreciative of your help yesterday, one Elf gives you an encrypted strategy guide (your puzzle input) that they say will be sure to help you win. "The first column is what your opponent is going to play: A for Rock, B for Paper, and C for Scissors. The second column--" Suddenly, the Elf is called away to help with someone's tent.

The second column, you reason, must be what you should play in response: X for Rock, Y for Paper, and Z for Scissors. Winning every time would be suspicious, so the responses must have been carefully chosen.

The winner of the whole tournament is the player with the highest score. Your total score is the sum of your scores for each round. The score for a single round is the score for the shape you selected (1 for Rock, 2 for Paper, and 3 for Scissors) plus the score for the outcome of the round (0 if you lost, 3 if the round was a draw, and 6 if you won).

Since you can't be sure if the Elf is trying to help you or trick you, you should calculate the score you would get if you were to follow the strategy guide.

For example, suppose you were given the following strategy guide:

A Y
B X
C Z

This strategy guide predicts and recommends the following:

In the first round, your opponent will choose Rock (A), and you should choose Paper (Y). This ends in a win for you with a score of 8 (2 because you chose Paper + 6 because you won).
In the second round, your opponent will choose Paper (B), and you should choose Rock (X). This ends in a loss for you with a score of 1 (1 + 0).
The third round is a draw with both players choosing Scissors, giving you a score of 3 + 3 = 6.
In this example, if you were to follow the strategy guide, you would get a total score of 15 (8 + 1 + 6).

What would your total score be if everything goes exactly according to your strategy guide?
*/

public enum RPS
{
    Rock = 1,
    Paper = 2,
    Scissors = 3
}

public enum RPSResult
{
    Win = 6,
    Draw = 3,
    Loss = 0
}

public readonly record struct Game(RPS OpponentPlay, RPS MyPlay)
{
    public RPSResult Result => (OpponentPlay, MyPlay) switch
    {
        { OpponentPlay: RPS.Rock, MyPlay: RPS.Rock } => RPSResult.Draw,
        { OpponentPlay: RPS.Rock, MyPlay: RPS.Paper } => RPSResult.Win,
        { OpponentPlay: RPS.Rock, MyPlay: RPS.Scissors } => RPSResult.Loss,

        { OpponentPlay: RPS.Paper, MyPlay: RPS.Rock } => RPSResult.Loss,
        { OpponentPlay: RPS.Paper, MyPlay: RPS.Paper } => RPSResult.Draw,
        { OpponentPlay: RPS.Paper, MyPlay: RPS.Scissors } => RPSResult.Win,

        { OpponentPlay: RPS.Scissors, MyPlay: RPS.Rock } => RPSResult.Win,
        { OpponentPlay: RPS.Scissors, MyPlay: RPS.Paper } => RPSResult.Loss,
        { OpponentPlay: RPS.Scissors, MyPlay: RPS.Scissors } => RPSResult.Draw,
    };

    public int NumericResult => (int)Result + (int)MyPlay;
}

public static class RockPaperScissors
{
    public static Parser<RPS> OpponentRock = from _ in Parse.Char('A') select RPS.Rock;
    public static Parser<RPS> OpponentPaper = from _ in Parse.Char('B') select RPS.Paper;
    public static Parser<RPS> OpponentScissors = from _ in Parse.Char('C') select RPS.Scissors;
    public static Parser<RPS> OpponentPlay = OpponentRock.Or(OpponentPaper).Or(OpponentScissors);

    public static Parser<RPS> MyRock = from _ in Parse.Char('X') select RPS.Rock;
    public static Parser<RPS> MyPaper = from _ in Parse.Char('Y') select RPS.Paper;
    public static Parser<RPS> MyScissors = from _ in Parse.Char('Z') select RPS.Scissors;
    public static Parser<RPS> MyPlay = MyRock.Or(MyPaper).Or(MyScissors);

    public static Parser<int> Game =
        from opponentPlay in OpponentPlay
        from _ in Parse.WhiteSpace
        from myPlay in MyPlay
        select new Game(opponentPlay, myPlay).NumericResult;
}

public class RockPaperScissorsTests
{
    [TestCase("A")]
    [TestCase("B")]
    [TestCase("C")]
    public void OpponentPlayTests(string input)
    {
        RockPaperScissors.OpponentPlay.Parse(input);
    }

    [TestCase("X")]
    [TestCase("Y")]
    [TestCase("Z")]
    public void MyPlayTests(string input)
    {
        RockPaperScissors.MyPlay.Parse(input);
    }

    [TestCase("A Y", 8)]
    [TestCase("B X", 1)]
    [TestCase("C Z", 6)]
    public void GameTests(string input, int expected)
    {
        RockPaperScissors
            .Game.Parse(input)
            .Should().Be(expected);
    }
}
