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
	}
}
