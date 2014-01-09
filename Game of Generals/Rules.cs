using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_of_Generals {
	class Rules {

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
