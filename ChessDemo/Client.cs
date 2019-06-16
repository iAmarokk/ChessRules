using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChessDemo
{
    class Client
    {
        public const string adress = "http://chess4players.info/api/Chess/";
        WebClient web;

        public int GameID { get; private set; }

        public Client ()
        {
            web = new WebClient();
        }

        public string GetFenFromServer ()
        {
            string json = web.DownloadString(adress);
            GameID = GetIdFromJSON(json);
            string fen = GetFenFromJSON(json);
            return fen;
        }

        int GetIdFromJSON (string json)
        {
            int x = json.IndexOf("\"ID\"");
            int y = json.IndexOf(":", x) + 1;
            int z = json.IndexOf(",", y);
            string id = json.Substring(y, z - y);
            return Convert.ToInt32(id);
        }

        public string SendMove (string move)
        {
            string json = web.DownloadString(adress + GameID.ToString() + "/" + move);
            string fen = GetFenFromJSON(json);
            return fen;
        }

        string GetFenFromJSON (string json)
        {
            int x = json.IndexOf("\"FEN\"");
            int y = json.IndexOf(":\"", x) + 2;
            int z = json.IndexOf("\"", y);
            string fen = json.Substring(y, z - y);
            return fen;
        }
    }
}
