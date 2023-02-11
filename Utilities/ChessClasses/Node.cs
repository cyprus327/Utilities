using System.Collections.Generic;

namespace Utilities {
	internal class Node {
		public Node(Piece[,] board, char player) {
			Board = board;
			Player = player;
			BoardValue = EvaluateBoard();
		}
		
		public Piece[,] Board { get; init; }
		public char Player { get; init; }
		public int BoardValue { get; init; }

		public List<Node> GenerateChildren() {
		    List<Node> children = new List<Node>();
			
		    for (int row = 0; row < 8; row++) {
		        for (int col = 0; col < 8; col++) {
		            Piece piece = Board[row, col];
		            if (piece == null || piece.Symbol != Player) continue;
					
					for (int destRow = 0; destRow < 8; destRow++) {
						for (int destCol = 0; destCol < 8; destCol++) {
							if (!piece.CanMove(destRow, destCol, Board)) continue;
							
							Piece[,] newBoard = (Piece[,])Board.Clone();
							newBoard[destRow, destCol] = Board[row, col];
							newBoard[row, col] = null;
							children.Add(new Node(newBoard, Player == 'w' ? 'b' : 'w'));
						}
					}
		        }
		    }
		
		    return children;
		}

		private int EvaluateBoard() {
			int score = 0;

			void GetScore(Piece x) {
				if (x.Symbol == Player) {
					score += x.Value;
				}
				else {
					score -= x.Value;
				}
			}
			
			Piece.DoForAll(Board, x => GetScore(x));

			return score;
		}
	}
}