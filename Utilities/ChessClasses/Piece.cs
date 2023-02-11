using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities {
    internal abstract class Piece {
        public int Col { get; set; }
        public int Row { get; set; }
        public char Symbol { get; init; }
        public string Name { get; init; } = string.Empty;
		public int Value { get; init; }

        public abstract bool CanMove(int destRow, int destCol, Piece[,] board);
		
		public static void Move(int row, int col, int destRow, int destCol, Piece[,] board) {
			board[destRow, destCol] = board[row, col];
			board[row, col] = null;
			board[destRow, destCol].Row = destRow;
			board[destRow, destCol].Col = destCol;
		}
		
        public static bool CoordsOnBoard(int row, int col) {
            if (row < 0 || row >= 8 ||
                col < 0 || col >= 8) {
                return false;
            }
            return true;
        }

        public static bool PieceInCheck(Piece piece, Piece[,] board) {
            for (int row = 0; row < 8; row++) {
                for (int col = 0; col < 8; col++) {
                    Piece attackingPiece = board[row, col];
                    if (attackingPiece == null || attackingPiece.Symbol == piece.Symbol) continue;

                    if (attackingPiece.CanMove(piece.Row, piece.Col, board)) {
                        return true;
                    }
                }
            }
            return false;
        }

		public static void DoForAll<Piece>(Piece[,] board, Action<Piece> body) {
			for (int row = 0; row < 8; row++) {
				for (int col = 0; col < 8; col++) {
					if (board[row, col] == null) continue;
					body(board[row, col]);
				}
			}
		}
    }

    internal class King : Piece {
        public King(int row, int col, char symbol) {
            Col = col;
            Row = row;
            Symbol = symbol;
			Value = int.MaxValue;
            Name = "K";
        }

        public override bool CanMove(int destRow, int destCol, Piece[,] board) {
			if (!CoordsOnBoard(destRow, destCol)) return false; 
			if (destRow == Row && destCol == Col) return false;
			if (board[destRow, destCol] != null && board[destRow, destCol].Symbol == Symbol) return false;

            int rowDiff = Math.Abs(destRow - Row);
            int colDiff = Math.Abs(destCol - Col);

            if (rowDiff > 1 || colDiff > 1) return false;
            
            Piece destinationPiece = board[destRow, destCol];
            if (destinationPiece != null) {
                if (destinationPiece.Symbol == Symbol) {
                    return false;
                }
            }

            return true;
        }
    }

    internal class Queen : Piece {
        public Queen(int row, int col, char symbol) {
            Col = col;
            Row = row;
            Symbol = symbol;
			Value = 9;
            Name = "Q";
        }

        public override bool CanMove(int destRow, int destCol, Piece[,] board) {
            if (!CoordsOnBoard(destRow, destCol)) return false;
			if (board[destRow, destCol] != null && board[destRow, destCol].Symbol == Symbol) return false;
			if (destRow == Row && destCol == Col) return false;
			
            int rowDiff = Math.Abs(destRow - Row);
            int colDiff = Math.Abs(destCol - Col);

            // queen can move horizontally, vertically, or diagonally
            if (rowDiff == 0 || colDiff == 0 || rowDiff == colDiff) {
                // check if there's any pieces blocking the path from the current position to the destination
                if (rowDiff == 0) {
                    int step = Math.Sign(destCol - Col);
                    for (int i = Col + step; i != destCol; i += step) {
                        if (board[Row, i] != null) return false;
                    }
                }
                else if (colDiff == 0) {
                    int step = Math.Sign(destRow - Row);
                    for (int i = Row + step; i != destRow; i += step) {
                        if (board[i, Col] != null) return false;
                    }
                }
                else {
                    int rowStep = Math.Sign(destRow - Row);
                    int colStep = Math.Sign(destCol - Col);
                    for (int i = Row + rowStep, j = Col + colStep; i != destRow; i += rowStep, j += colStep) {
                        if (board[i, j] != null) return false;
                    }
                }

                return true;
            }

            return false;
        }
    }

    internal class Rook : Piece {
        public Rook(int row, int col, char symbol) {
            Row = row;
            Col = col;
            Symbol = symbol;
			Value = 5;
            Name = "R";
        }

        public override bool CanMove(int destRow, int destCol, Piece[,] board) {
            if (!CoordsOnBoard(destRow, destCol)) return false;
			if (destRow == Row && destCol == Col) return false;
			if (board[destRow, destCol] != null && board[destRow, destCol].Symbol == Symbol) return false;

            // Rooks can move horizontally or vertically
            if (destRow == Row || destCol == Col) {
                // check if path to destination is clear
                if (destRow == Row) {
                    int direction = Math.Sign(destCol - Col);
                    for (int i = Col + direction; i != destCol; i += direction) {
                        if (board[Row, i] != null) return false;
                    }
                }
                else {
                    int direction = Math.Sign(destRow - Row);
                    for (int i = Row + direction; i != destRow; i += direction) {
                        if (board[i, Col] != null) return false;
                    }
                }

                return true;
            }

            return false;
        }
    }

    internal class Bishop : Piece {
        public Bishop(int row, int col, char symbol) {
            Row = row;
            Col = col;
            Symbol = symbol;
			Value = 3;
            Name = "B";
        }

        public override bool CanMove(int destRow, int destCol, Piece[,] board) {
            if (!CoordsOnBoard(destRow, destCol)) return false;
			if (destRow == Row && destCol == Col) return false;
			if (board[destRow, destCol] != null && board[destRow, destCol].Symbol == Symbol) return false;

            // bishops can only move on a diagonal
            int rowDiff = Math.Abs(destRow - Row);
            int colDiff = Math.Abs(destCol - Col);
            if (rowDiff != colDiff) return false;

            // check if the path to the destination is clear
            int rowIncrement = rowDiff == 0 ? 0 : (destRow - Row) / rowDiff;
            int colIncrement = colDiff == 0 ? 0 : (destCol - Col) / colDiff;
            for (int i = 1; i < rowDiff; i++) {
                int nextRow = Row + (i * rowIncrement);
                int nextCol = Col + (i * colIncrement);
                if (board[nextRow, nextCol] != null) return false;
            }

            return true;
        }
    }

    internal class Knight : Piece {
        public Knight(int row, int col, char symbol) {
            Row = row;
            Col = col;
            Symbol = symbol;
			Value = 3;
            Name = "N";
        }

        public override bool CanMove(int destRow, int destCol, Piece[,] board) {
            if (!CoordsOnBoard(destRow, destCol)) return false;
			if (destRow == Row && destCol == Col) return false;
			if (board[destRow, destCol] != null && board[destRow, destCol].Symbol == Symbol) return false;

            int rowDiff = Math.Abs(destRow - Row);
            int colDiff = Math.Abs(destCol - Col);
            if ((rowDiff == 2 && colDiff == 1) || (rowDiff == 1 && colDiff == 2)) {
                return true;
            }

            return false;
        }
    }

    internal class Pawn : Piece {
        public Pawn(int row, int col, char symbol) {
            Row = row;
            Col = col;
            Symbol = symbol;
			Value = 1;
            Name = "P";
        }

        public override bool CanMove(int destRow, int destCol, Piece[,] board) {
            if (!CoordsOnBoard(destRow, destCol)) return false;
			if (destRow == Row && destCol == Col) return false;
			if (board[destRow, destCol] != null && board[destRow, destCol].Symbol == Symbol) return false;

            int rowDirection = Symbol == 'w' ? -1 : 1;
            int moveDistance = Math.Abs(destRow - Row);

            if (destCol == Col && board[destRow, destCol] == null) {
				if (Math.Sign(destRow - Row) != rowDirection) return false;
				
                if (moveDistance == 1) {
                    return true;
                }
				else if (moveDistance == 2 && Row == (Symbol == 'w' ? 6 : 1)) {
					int row = Symbol == 'w' ? 6 : 1;
					if (board[(row == 6 ? 5 : 2), destCol] == null) {
						return true;
					}
				}
            }
            else if (Math.Abs(destCol - Col) == 1 && moveDistance == 1) {
                if (board[destRow, destCol] != null && board[destRow, destCol].Symbol != Symbol) {
                    return true;
                }
            }

            return false;
        }
    }
}