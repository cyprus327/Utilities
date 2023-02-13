using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Utilities.ChessUtil {
    internal static class Chess {
        public static void PlayVsHuman() {
            InitializeBoard(out Piece[,] board);

            char currentPlayer = 'w';

			char winner;
            do {
				HandlePlayerMove(currentPlayer, board);
				
                Console.Clear();
                DrawBoard(board);

                currentPlayer = currentPlayer == 'w' ? 'b' : 'w';
            }
			while (!GameOver(board, out winner));

			Console.Clear();
			DrawBoard(board);
			Console.WriteLine($"\n{(winner == 'w' ? "White" : "Black")} wins.");
			Console.ReadKey(true);
        }

		public static void PlayVsAI() {
			InitializeBoard(out Piece[,] board);

			char currentPlayer = 'w';
			
			char winner;
			do {
				if (currentPlayer == 'w') {
					HandlePlayerMove(currentPlayer, board);

					Console.Clear();
					DrawBoard(board);
				}
				else {
					Console.WriteLine("\nAIs move...");
					HandleAIMove(currentPlayer, board);

					Console.Clear();
					DrawBoard(board);
				}

				currentPlayer = currentPlayer == 'w' ? 'b' : 'w';
			}
			while (!GameOver(board, out winner));

			Console.Clear();
			DrawBoard(board);
			Console.WriteLine($"\n{(winner == 'w' ? "White" : "Black")} wins.");
			Console.ReadKey(true);
		}

		private static void HandlePlayerMove(char player, Piece[,] board) {
			int row = 0, col = 0;

			while (true) {
				Console.Clear();
				DrawBoard(board, row, col);
				
				var key = Console.ReadKey(true);
				if (key.Key == ConsoleKey.LeftArrow) {
					if (col > 0) col--;
				}
				else if (key.Key == ConsoleKey.RightArrow) {
					if (col < 7) col++;
				}
				else if (key.Key == ConsoleKey.UpArrow) {
					if (row > 0) row--;
				}
				else if (key.Key == ConsoleKey.DownArrow) {
					if (row < 7) row++;
				}
				else if (key.Key == ConsoleKey.Enter) {
					if (board[row, col] == null || board[row, col].Symbol != player) continue;

					List<(int, int)> moves = new List<(int, int)>();
					for (int destRow = 0; destRow < 8; destRow++) {
						for (int destCol = 0; destCol < 8; destCol++) {
							if (!board[row, col].CanMove(destRow, destCol, board)) continue;
							moves.Add((destRow, destCol));
						}
					}

					if (moves.Count == 0) return;

					int index = 0;
					while (true) {
						Console.Clear();
						DrawBoard(board, row, col, moves[index].Item1, moves[index].Item2);

						key = Console.ReadKey(true);
						if (key.Key == ConsoleKey.LeftArrow) {
							index--;
							index = index < 0 ? moves.Count - 1 : index;
						}
						else if (key.Key == ConsoleKey.RightArrow) {
							index++;
							index = index >= moves.Count ? 0 : index;
						}
						else if (key.Key == ConsoleKey.Enter) {
							Piece.Move(row, col, moves[index].Item1, moves[index].Item2, board);
							return;
						}
						else if (key.Key == ConsoleKey.Escape) {
							break;
						}
					}
				}
				else if (key.Key == ConsoleKey.Escape) {
					return;
				}
			}
		}

		private static void HandleAIMove(char ai, Piece[,] board) {
			List<(int, int, int, int)> bestMoves = new List<(int, int, int, int)>();
			int bestValue = int.MinValue;

			for (int row = 0; row < 8; row++) {
				for (int col = 0; col < 8; col++) {
					if (board[row, col] == null || board[row, col].Symbol != ai) continue;

					for (int destRow = 0; destRow < 8; destRow++) {
						for (int destCol = 0; destCol < 8; destCol++) {
							if (!board[row, col].CanMove(destRow, destCol, board)) continue;

							Piece[,] newBoard = (Piece[,])board.Clone();
							newBoard[destRow, destCol] = newBoard[row, col];
							newBoard[row, col] = null;

							int value = KillerMoveMinimax(new Node(newBoard, ai), 3, int.MinValue, int.MaxValue, true);
							if (value > bestValue) {
								bestMoves.Clear();
								bestMoves.Add((row, col, destRow, destCol));
								bestValue = value;
							}
							else if (value == bestValue) {
								bestMoves.Add((row, col, destRow, destCol));
							}
						}
					}
				}
			}
			
			int rand = bestMoves.Count == 1 ? 0 : RandomNumberGenerator.GetInt32(bestMoves.Count - 1);
			(int, int, int, int) move = bestMoves[rand];

			Piece.Move(move.Item1, move.Item2, move.Item3, move.Item4, board);
		}

		private static int KillerMoveMinimax(Node node, int depth, int alpha, int beta, bool maximizingPlayer) {
			if (depth == 0) return node.BoardValue;

			List<Node> children = node.GenerateChildren();
			if (children.Count == 0) return node.BoardValue;

			int bestScore = maximizingPlayer ? int.MinValue : int.MaxValue;
			int killerScore = int.MinValue;
			int killerInd = -1;

			for (int i = 0; i < children.Count; i++) {
				int score;

				if (i == killerInd) {
					score = -KillerMoveMinimax(children[i], depth - 1, -beta, -alpha, !maximizingPlayer);
					if (score > killerScore && score < beta) {
						killerScore = score;
						killerInd = i;
					}
				}
				else {
					score = -KillerMoveMinimax(children[i], depth - 1, -beta, -alpha, !maximizingPlayer);
					if (score > killerScore) {
						killerScore = score;
						killerInd = i;
					}
				}

				if (maximizingPlayer) {
					bestScore = Math.Max(bestScore, score);
					alpha = Math.Max(alpha, bestScore);
					if (alpha >= beta) break;
				}
				else {
					bestScore = Math.Min(bestScore, score);
					beta = Math.Min(beta, bestScore);
					if (alpha >= beta) break;
				}
			}

			return bestScore;
		}

		private static bool GameOver(Piece[,] board, out char winner) {
			Piece whiteKing = null, blackKing = null;

			for (int row = 0; row < 8; row++) {
				for (int col = 0; col < 8; col++) {
					if (board[row, col] is King) {
						switch (board[row, col].Symbol) {
							case 'w': whiteKing = board[row, col]; break;
							case 'b': blackKing = board[row, col]; break;
						}
					}
				}
			}

			winner = ' ';
			if (whiteKing == null) {
				winner = 'b';
				return true;
			}
			if (blackKing == null) {
				winner = 'w';
				return true;
			}

			return false; 
		}
		
        private static void DrawBoard(Piece[,] board, int selectedRow = -1, int selectedCol = -1, int hRow = -1, int hCol = -1) {
			List<(int, int)> moves = null;
			if (selectedRow != -1 && selectedCol != -1) {
				moves = new List<(int, int)>() { (selectedRow, selectedCol) };
				for (int row = 0; row < 8; row++) {
					if (board[selectedRow, selectedCol] == null) break;
					
					for (int col = 0; col < 8; col++) {
						if (row != selectedRow || col != selectedCol) continue;
						
						for (int destRow = 0; destRow < 8; destRow++) {
							for (int destCol = 0; destCol < 8; destCol++) {
								if (!board[selectedRow, selectedCol].CanMove(destRow, destCol, board)) continue;
								moves.Add((destRow, destCol));
							}
						}
					}
				}
			}
			
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("   a b c d e f g h");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("  =---------------=");
            for (int row = 0; row < 8; row++) {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(8 - row);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(" |");
                for (int col = 0; col < 7; col++) {
					if (row == hRow && col == hCol) {
						Console.ForegroundColor = ConsoleColor.Green;
                        if (board[row, col] == null) Console.Write("- ");
                        else Console.Write($"{board[row, col].Name} ");
                    }
					else if (moves != null && moves.Contains((row, col))) {
						Console.ForegroundColor = ConsoleColor.Magenta;
						if (board[row, col] == null) Console.Write("- ");
						else Console.Write($"{board[row, col].Name} ");
					}
					else if (board[row, col] == null) {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("- ");
                    }
                    else {
                        Console.ForegroundColor = board[row, col].Symbol == 'w' ? ConsoleColor.White : ConsoleColor.DarkGray;
                        Console.Write($"{board[row, col].Name} ");
                    }
                }

				if (row == hRow && 7 == hCol) {
					Console.ForegroundColor = ConsoleColor.Green;
					if (board[row, 7] == null) Console.Write("-");
					else Console.Write(board[row, 7].Name);
				}
				else if (moves != null && moves.Contains((row, 7))) {
					Console.ForegroundColor = ConsoleColor.Magenta;
					if (board[row, 7] == null) Console.Write("-");
					else Console.Write(board[row, 7].Name);
				}
				else if (board[row, 7] == null) {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("-");
                }
                else {
                    Console.ForegroundColor = board[row, 7].Symbol == 'w' ? ConsoleColor.White : ConsoleColor.DarkGray;
                    Console.Write(board[row, 7].Name);
                }
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("|");
            }
            Console.WriteLine("  =---------------=");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void InitializeBoard(out Piece[,] board) {
            board = new Piece[8, 8];

            // black
            board[0, 0] = new Rook(0, 0, 'b');
            board[0, 1] = new Knight(0, 1, 'b');
            board[0, 2] = new Bishop(0, 2, 'b');
            board[0, 3] = new Queen(0, 3, 'b');
            board[0, 4] = new King(0, 4, 'b');
            board[0, 5] = new Bishop(0, 5, 'b');
            board[0, 6] = new Knight(0, 6, 'b');
            board[0, 7] = new Rook(0, 7, 'b');
            for (int i = 0; i < 8; i++) {
                board[1, i] = new Pawn(1, i, 'b');
            }

            // white
            board[7, 0] = new Rook(7, 0, 'w');
            board[7, 1] = new Knight(7, 1, 'w');
            board[7, 2] = new Bishop(7, 2, 'w');
            board[7, 3] = new Queen(7, 3, 'w');
            board[7, 4] = new King(7, 4, 'w');
            board[7, 5] = new Bishop(7, 5, 'w');
            board[7, 6] = new Knight(7, 6, 'w');
            board[7, 7] = new Rook(7, 7, 'w');
            for (int i = 0; i < 8; i++) {
                board[6, i] = new Pawn(6, i, 'w');
            }
        }
    }
}
