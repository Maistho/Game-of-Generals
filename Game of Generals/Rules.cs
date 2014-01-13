using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_of_Generals {
	class Rules {
        private static int turn = 0;
		private static bool P1Winning, P2Winning = false;
        public static void nextTurn() {
            ++turn;
        }
        public static bool canPlace() {
            return (turn < 2);
        }
        /*public static bool mayPass(Player player) {
            if (turn > 1 || player.onBoardPieces == 21) {
                return true;
            } else {
                //YOU SHALL NOT PASS
                return false;
            }
        }*/
		public static bool legalMove(Piece piece, int[] newMove) {
           if (turn > 1) {
                if (Math.Abs(piece.X - newMove[0]) == 1) {
                    if (Math.Abs(piece.Y - newMove[1]) == 0) {
                        return true;
                    } else {
                        return false;
                    }

                } else if (Math.Abs(piece.X - newMove[0]) == 0) {
                    if (Math.Abs(piece.Y - newMove[1]) == 1) {
                        return true;
                    } else {
                        return false;
                    }
                }
                return false;
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
			switch (rank) {
				case 1:
					return 6;
				case 14:
					return 2;
				default:
					return 1;
			}
		}
		public static int victoryCheck(IEnumerable<Piece> flags) {
			Piece flagP1 = flags.FirstOrDefault(f=> f.getPlayer() == 0);
			Piece flagP2 = flags.FirstOrDefault(f=> f.getPlayer() == 1);
			if (flagP1 == null) {
				return 2;
			} else if(flagP2 == null) {
				return 1;
			} else if (flagP1.Y == 0) {
				if (P1Winning == true) {
					return 1;
				} else {
					P1Winning = true;
				}
			} else if (flagP2.Y == 7) {
				if (P2Winning == true) {
					return 1;
				} else {
					P2Winning = true;
				}
			}
			return 0;
		}
		public static int stronger(Piece p1, Piece p2) {
			int diff = p1.getRank() - p2.getRank();
			if (diff == 0) {
				if (p1.getRank() == 0) {
					return 2; //Both are flags, challenging flag wins
				} else {
					return 0; //Equal non-flags, both loses
				}
			} else if (diff < 0) {
				if (p2.getRank() == 14 && p1.getRank() == 1) {
					return 1; //p2 is Spy and p1 is Private, p1 wins
				}
				return 2; //p2 wins
			} else {
				if (p1.getRank() == 14 && p2.getRank() == 1) {
					return 2; //p1 is Spy and p2 is Private, p2 wins
				}
				return 1; //p1 wins
			}
		}
	}
}
