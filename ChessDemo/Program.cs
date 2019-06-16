using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessRules;

namespace ChessDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client();
            string fen = client.GetFenFromServer();
            Console.WriteLine(client.GameID);
            Chess chess = new Chess(fen);
            //"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
            //Console.WriteLine(NextMoves(6,chess));
            //Console.ReadKey();
            //return;
            while (true)
            {
                Console.WriteLine(chess.fen);
                Print(ChessToAscii(chess));
                foreach (string moves in chess.YieldValidMoves())
                    Console.Write(moves + " ");
                Console.WriteLine();
                string move = Console.ReadLine();
                if (move == "") break;
                if (move == "s")
                {
                    fen = client.GetFenFromServer();
                    chess = new Chess(fen);
                    continue;
                }

                if (!chess.IsValidMove(move))
                    continue;
                fen = client.SendMove(move);
                chess = new Chess(fen);
            }
        }
        static int NextMoves (int step, Chess chess)
        {
            if (step == 0) return 1;
            int count = 0;
            foreach (string moves in chess.YieldValidMoves())
                count += NextMoves(step - 1, chess.Move(moves));
            return count;
        }

        static string ChessToAscii(Chess chess)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("  +----------------+");
            for (int y = 7; y >= 0; y--)
            {
                sb.Append(y + 1);
                sb.Append(" | ");
                for (int x = 0; x < 8; x++)
                    sb.Append(chess.GetFigureAt(x, y) + " ");
                sb.AppendLine("|");
            }
            sb.AppendLine("  +----------------+");
            sb.AppendLine("    a b c d e f g h  ");
            if (chess.IsCheck) sb.AppendLine("Is check");
            if (chess.IsCheckmate) sb.AppendLine("Is checkmate");
            if (chess.IsStalemate) sb.AppendLine("Is stalemate");
            return sb.ToString();
        }

        static void Print (string text)
        {
            ConsoleColor old = Console.ForegroundColor;
            foreach (char x in text)
            {
                if (x >= 'a' && x <= 'z')
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (x >= 'A' && x <= 'Z')
                    Console.ForegroundColor = ConsoleColor.White;
                else
                    Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(x);
            }
            Console.ForegroundColor = old; 
        }
    }
}
