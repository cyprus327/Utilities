using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System.Drawing;

namespace Utilities.PongUtil {
    internal static class Pong {
        public static void PlayVsAI() {

        }

        public static void PlayVsHuman() {
            int width = 60;
            int height = 30;
            int ballX = width / 2;
            int ballY = height / 2;
            int ballSpeedX = 1;
            int ballSpeedY = 1;
            int player1Y = height / 2;
            int player2Y = height / 2;
            int player1Score = 0;
            int player2Score = 0;

            while (true) {
                var newState = UpdateGameState(
                    width, height, ballX, ballY, ballSpeedX, ballSpeedY,
                    player1Y, player2Y, player1Score, player2Score
                );

                if (newState == null) {
                    break;
                }

                (width, height, ballX, ballY, ballSpeedX, ballSpeedY,
                 player1Y, player2Y, player1Score, player2Score) = 
                 ((int, int, int, int, int, int, int, int, int, int))newState;
                

                DisplayGameState(
                    width, height, ballX, ballY, player1Y, player2Y,
                    player1Score, player2Score
                );

                Thread.Sleep(50);
            }
        }

        private static (int, int) UpdateBallPosition(
            int ballX, int ballY, int ballSpeedX, int ballSpeedY) {

            int newBallX = ballX + ballSpeedX;
            int newBallY = ballY + ballSpeedY;

            return (newBallX, newBallY);
        }

        private static int UpdatePlayerPosition(
            int playerY, bool moveUp, bool moveDown, int height) {

            if (moveUp && playerY > 1) {
                return playerY - 1;
            }

            if (moveDown && playerY < height - 2) {
                return playerY + 1;
            }

            return playerY;
        }

        private static int UpdatePlayer1Position(
            int player1Y, bool moveUp, bool moveDown, int height) {

            return UpdatePlayerPosition(player1Y, moveUp, moveDown, height);
        }

        private static int UpdatePlayer2Position(
            int player2Y, bool moveUp, bool moveDown, int height) {

            return UpdatePlayerPosition(player2Y, moveUp, moveDown, height);
        }

        private static (int, int) UpdateBallSpeed(
            int ballX, int ballY, 
            int ballSpeedX, int ballSpeedY, 
            int player1Y, int player2Y, 
            int width, int height) {

            if (ballX == 1 && ballY >= player1Y && ballY <= player1Y + 2) {
                return (-ballSpeedX, ballSpeedY);
            }

            if (ballX == width - 2 && ballY >= player2Y && ballY <= player2Y + 2) {
                return (-ballSpeedX, ballSpeedY);
            }

            if (ballX == 0) {
                return (1, 1);
            }

            if (ballX == width - 1) {
                return (-1, 1);
            }

            if (ballY == 0 || ballY == height - 1) {
                return (ballSpeedX, -ballSpeedY);
            }

            return (ballSpeedX, ballSpeedY);
        }

        private static (int, int, int, int, int, int, int, int, int, int)? UpdateGameState(
            int width, int height, 
            int ballX, int ballY, 
            int ballSpeedX, int ballSpeedY,
            int player1Y, int player2Y, 
            int player1Score, int player2Score) {

            var key = Console.ReadKey(true);

            var moveUp1 = key.Key == ConsoleKey.W;
            var moveDown1 = key.Key == ConsoleKey.S;
            var moveUp2 = key.Key == ConsoleKey.UpArrow;
            var moveDown2 = key.Key == ConsoleKey.DownArrow;

            int newPlayer1Y = UpdatePlayer1Position(player1Y, moveUp1, moveDown1, height);
            int newPlayer2Y = UpdatePlayer2Position(player2Y, moveUp2, moveDown2, height);

            var (newBallX, newBallY) = UpdateBallPosition(ballX, ballY, ballSpeedX, ballSpeedY);

            var (newBallSpeedX, newBallSpeedY) = UpdateBallSpeed(
                newBallX, newBallY, ballSpeedX, ballSpeedY, newPlayer1Y, newPlayer2Y, width, height
            );

            if (newBallX == ballX) {
                return null;
            }

            int newPlayer1Score = player1Score;
            int newPlayer2Score = player2Score;

            if (newBallX == 0) {
                newPlayer2Score++;
            }

            if (newBallX == width - 1) {
                newPlayer1Score++;
            }

            return (width, height, newBallX, newBallY, newBallSpeedX, newBallSpeedY,
                    newPlayer1Y, newPlayer2Y, newPlayer1Score, newPlayer2Score);
        }

        private static void DisplayGameState(
            int width, int height, 
            int ballX, int ballY, 
            int player1Y, int player2Y,
            int player1Score, int player2Score) {

            Console.Clear();

            Console.WriteLine($"Player 1 score: {player1Score}");
            Console.WriteLine($"Player 2 score: {player2Score}\n");

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (x == 0 || x == width - 1) {
                        Console.Write("|");
                    }
                    else if (x == ballX && y == ballY) {
                        Console.Write("O");
                    }
                    else if (x == 2 && y >= player1Y && y <= player1Y + 2) {
                        Console.Write("=");
                    }
                    else if (x == width - 3 && y >= player2Y && y <= player2Y + 2) {
                        Console.Write("=");
                    }
                    else {
                        Console.Write(" ");
                    }
                }

                Console.WriteLine();
            }
        }
    }
}
