using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Utilities {
    internal static class Chess {
		public static void StepThroughMoves(bool pawnsEnabled = true) {
			InitializeBoard(out Piece[,] board);

			if (!pawnsEnabled) {
				for (int i = 0; i < 8; i++) {
					board[1, i] = null;
					board[6, i] = null;
				}
			}

			Console.CursorVisible = false;
			for (int row = 0; row < 8; row++) {
				for (int col = 0; col < 8; col++) {
					if (board[row, col] == null) continue;
					Piece piece = board[row, col];
					for (int destRow = 0; destRow < 8; destRow++) {
						for (int destCol = 0; destCol < 8; destCol++) {
							if (!piece.CanMove(destRow, destCol, board)) continue;
							//Piece[,] copyBoard = (Piece[,])board.Clone();
							//copyBoard[destRow, destCol] = copyBoard[row, col];
							//copyBoard[row, col] = null;
							Console.Clear();
							Console.Write(new string('\n', destRow));
							Console.Write(new string(' ', destCol));
							Console.Write(board[row, col].Name);
							//DrawBoard(copyBoard);
							System.Threading.Thread.Sleep(10);
						}
					}
				}
			}
			Console.CursorVisible = true;

			if (pawnsEnabled) {
				StepThroughMoves(false);
				return;
			}
			
			Console.WriteLine("\nPress any key to continue...");
			Console.ReadKey(true);
		}
		
        public static void PlayVsHuman() {
            InitializeBoard(out Piece[,] board);
			
			Console.Clear();
            DrawBoard(board);
            char currentPlayer = 'w';

			char winner = ' ';
            do {
                Console.WriteLine($"\n{(currentPlayer == 'w' ? "White" : "Black")}'s move:");
                Console.Write("> ");
                
				string move = Console.ReadLine() ?? "";
				if (move == "exit") break;
                
				if (!HandlePlayerMove(currentPlayer, move, board)) continue;
				
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

			Console.Clear();
			DrawBoard(board);
			
			char currentPlayer = 'w';
			
			char winner = ' ';
			do {
				if (currentPlayer == 'w') {
					Console.WriteLine("\nYour move. (White)");
					Console.Write("> ");

					string move = Console.ReadLine() ?? "";
					if (move == "exit") break;

					if (!HandlePlayerMove('w', move, board)) continue;

					Console.Clear();
					DrawBoard(board);
				}
				else {
					Console.WriteLine("\nAIs move...");
					HandleAIMove('b', board);

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

		private static bool HandlePlayerMove(char player, string move, Piece[,] board) {
			string[] moveCoords = move.Split(' ');
			if (moveCoords.Length != 2) {
				Console.WriteLine("Invalid move format. Please enter your move in the format 'e2 e4'.");
				return false;
			}
			if (!int.TryParse(moveCoords[0][1].ToString(), out int startRow) || 
				!int.TryParse(moveCoords[1][1].ToString(), out int destRow)) {
				Console.WriteLine("Invalid move format. Enter moves in the format 'e2 e4'.");
				return false;
			}
			startRow = 8 - startRow;
			destRow = 8 - destRow;
			int startCol = moveCoords[0][0] - 'a';
			int destCol = moveCoords[1][0] - 'a';

			if (!Piece.CoordsOnBoard(startRow, startCol) || !Piece.CoordsOnBoard(destRow, destCol)) {
				Console.WriteLine("Invalid move. Please enter a move that is within the chess board.");
				return false;
			}

			Piece piece = board[startRow, startCol];
			if (piece == null || piece.Symbol != player) {
				Console.WriteLine($"Invalid move. You can only move your own pieces.");
				return false;
			}
			if (!piece.CanMove(destRow, destCol, board)) {
				Console.WriteLine("Invalid move. That piece cannot move to that location.");
				return false;
			}

			Piece.Move(piece.Row, piece.Col, destRow, destCol, board);

			return true;
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

							int value = KillerMoveMinimax(new Node(newBoard, ai), 2, int.MinValue, int.MaxValue, true);
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

			/*for (int row = 0; row < 8; row++) {
				for (int col = 0; col < 8; col++) {
					Piece piece = board[row, col];
					if (piece == null || piece.Name != "K") continue;
		
					if (!Piece.PieceInCheck(piece, board)) continue;
		
					bool hasEscape = false;
					for (int destRow = 0; destRow < 8; destRow++) {
						for (int destCol = 0; destCol < 8; destCol++) {
							if (!piece.CanMove(destRow, destCol, board)) continue;
		
							Piece[,] newBoard = (Piece[,])board.Clone();
							newBoard[destRow, destCol] = piece;
							newBoard[row, col] = null;
		
							if (!Piece.PieceInCheck(piece, newBoard)) {
								hasEscape = true;
								break;
							}
						}
						if (hasEscape) break;
					}
					if (!hasEscape) {
						winner = piece.Symbol == 'w' ? 'b' : 'w';
						return true;
					}
				}
			} */
			return false; 
		}
		
        private static void DrawBoard(Piece[,] board, List<Piece> additionals = null) {
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
                    if (board[row, col] == null) {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("- ");
                    }
					else if (additionals != null && additionals.Contains(board[row, col])) {
						Console.ForegroundColor = ConsoleColor.Green;
						for (int destRow = 0; destRow < 8; destRow++) {
							for (int destCol = 0; destCol < 8; destCol++) {
								if (!board[row, col].CanMove(destRow, destCol, board)) continue;

                        		Console.Write($"{board[row, col].Name} ");
							}
						}
					}
                    else {
                        Console.ForegroundColor = board[row, col].Symbol == 'w' ? ConsoleColor.White : ConsoleColor.DarkGray;
                        Console.Write($"{board[row, col].Name} ");
                    }
                }

                if (board[row, 7] == null) {
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

            // white
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

            // black
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
