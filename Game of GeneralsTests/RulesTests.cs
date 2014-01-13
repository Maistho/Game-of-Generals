using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_of_Generals;
using NUnit.Framework;
using System.Collections.ObjectModel;
namespace Game_of_Generals.Tests {
	[TestFixture()]
	public class RulesTests {
		[Test()]
		public void canPlaceTest() {
			Assert.IsTrue(Rules.canPlace(0));
			Assert.IsTrue(Rules.canPlace(1));
			Assert.IsFalse(Rules.canPlace(2));
			Assert.IsFalse(Rules.canPlace(10));
		}

		[Test()]
		public void legalMoveTest() {
			var p = new Piece(0, 0, 0, 0);
			Assert.IsTrue(Rules.legalMove(p, new int[] { 0, 1 }, 5));
			Assert.IsTrue(Rules.legalMove(p, new int[] { 1, 0 }, 5));
			Assert.IsFalse(Rules.legalMove(p, new int[] { 1, 1 }, 5));
			Assert.IsFalse(Rules.legalMove(p, new int[] { -1, -1 }, 5));
			Assert.IsFalse(Rules.legalMove(p, new int[] { 0, 1 }, 1));
			Assert.IsFalse(Rules.legalMove(p, new int[] { 5, 1 }, 5));
		}

		[Test()]
		public void legalPlacementTest() {
			Assert.IsTrue(Rules.legalPlacement(0, 5));
			Assert.IsTrue(Rules.legalPlacement(1, 1));
			Assert.IsFalse(Rules.legalPlacement(0, 1));
			Assert.IsFalse(Rules.legalPlacement(1, 5));
		}

		[Test()]
		public void numberOfRanksTest() {
			Assert.AreEqual(Rules.numberOfRanks(), 14);
		}

		[Test()]
		public void numberOfPiecesTest() {
			Assert.AreEqual(Rules.numberOfPieces(0), 1);
			Assert.AreEqual(Rules.numberOfPieces(1), 6);
			Assert.AreEqual(Rules.numberOfPieces(2), 1);
			Assert.AreEqual(Rules.numberOfPieces(14), 2);
		}

		[Test()]
		public void victoryCheckTest() {
			var pieces = new ObservableCollection<Piece>();
			var flag1 = new Piece(0, 7, 0, 0);
			var flag2 = new Piece(0, 0, 0, 1);
			pieces.Add(flag1);
			pieces.Add(flag2);
			var q = from g in pieces
					where g.getRank() == 0
					select g;
			Assert.AreEqual(Rules.victoryCheck(q), 0);
			pieces.Remove(flag2);
			q = from g in pieces
					where g.getRank() == 0
					select g;
			Assert.AreEqual(Rules.victoryCheck(q), 1);
			pieces.Add(flag2);
			pieces.Remove(flag1);
			q = from g in pieces
					where g.getRank() == 0
					select g;
			Assert.AreEqual(Rules.victoryCheck(q), 2);
			pieces.Add(flag1);
			flag1.Y = 0;
			q = from g in pieces
				where g.getRank() == 0
				select g;
			Assert.AreEqual(Rules.victoryCheck(q), 0);
			Assert.AreEqual(Rules.victoryCheck(q), 1);
		}

		[Test()]
		public void strongerTest() {
			Assert.AreEqual(Rules.stronger(new Piece(0, 0, 5, 0), new Piece(0, 0, 5, 0)), 0);
			Assert.AreEqual(Rules.stronger(new Piece(0, 0, 10, 0), new Piece(0, 0, 5, 0)), 1);
			Assert.AreEqual(Rules.stronger(new Piece(0, 0, 1, 0), new Piece(0, 0, 14, 0)), 1);
			Assert.AreEqual(Rules.stronger(new Piece(0, 0, 4, 0), new Piece(0, 0, 8, 0)), 2);
			Assert.AreEqual(Rules.stronger(new Piece(0, 0, 0, 0), new Piece(0, 0, 0, 0)), 2);
			Assert.AreEqual(Rules.stronger(new Piece(0, 0, 14, 0), new Piece(0, 0, 1, 0)), 2);
		}
	}
}
