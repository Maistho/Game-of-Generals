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
	}
}
