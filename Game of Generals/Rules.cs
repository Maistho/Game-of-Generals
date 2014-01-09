using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_of_Generals {
	class Rules {

		public static bool legalMove(Piece piece, int[] newMove) {
			if (Math.Abs(piece.Position[0] - newMove[0]) == 1) {
				if (Math.Abs(piece.Position[1] - newMove[1]) == 0) {
					return true;
				} else {
					return false;
				}

			} else if (Math.Abs(piece.Position[0] - newMove[0]) == 0) {
				if (Math.Abs(piece.Position[1] - newMove[1]) == 1) {
					return true;
				} else {
					return false;
				}
			} else {
				return false;
			}
		}

		public static bool legalPlacement(int player, int row) {
			if (player == 0) {
				if (row > 4) {
					return true;
				} else {
					return false;
				}
			} else if (player == 1) {
				if (row < 3) {
					return true;
				} else {
					return false;
				}
			} else {
				return false;
			}
		}

		public static int numberOfRanks() {
			return 14;
		}
		public static int numberOfPieces(int rank) {
			switch(rank){
				case 1:
					return 6;
				case 14:
					return 2;
				default:
					return 1;
			}

		}

        public static int stronger(Piece p1, Piece p2) {
            int diff = p1.getRank() - p2.getRank();
            if (diff == 0) {
                if (p1.getRank() == 0) {
                    return 3; //Both are flags
                } else {
                    return 0; //Equal
                }
            } else if (diff < 0) {
                if (p2.getRank() == 14) {
                    if (p1.getRank() == 1) {
                        return 1;
                    }
                }
                return 2;
            } else {
                if (p1.getRank() == 14) {
                    if (p2.getRank() == 1) {
                        return 2;
                    }
                }
                return 1;
            }

        }

	}
}
