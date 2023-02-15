using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.MenuUtil {
    internal static class ASCIIGenerator {
        #region symbols
        readonly static string[] A = {
            "   AA     ",
            "  AAAA    ",
            " AA AAA   ",
            "AA   AAA  ",
            "AAAAAAAA  ",
            "AA   AAA  ",
            "AA  AAAAA "
        };

        readonly static string[] B = {
            "BBBBBBBB  ",
            "BB    BBB ",
            "BB    BBB ",
            "BBBBBBB   ",
            "BB    BBB ",
            "BB    BBB ",
            "BBBBBBBBB "
        };

        readonly static string[] C = {
            " CCCCCCC  ",
            "CC    CCC ",
            "CC    CCC ",
            "CC        ",
            "CC    CCC ",
            "CC    CCC ",
            " CCCCCCC  "
        };

        readonly static string[] D = {
            "DDDDDDD   ",
            "DD   DDD  ",
            "DD    DDD ",
            "DD    DDD ",
            "DD    DDD ",
            "DD   DDD  ",
            "DDDDDDD   "
        };

        readonly static string[] E = {
            "EEEEEEEEE ",
            "EE    EEE ",
            "EE        ",
            "EEEEEE    ",
            "EE        ",
            "EE    EEE ",
            "EEEEEEEEE "
        };

        readonly static string[] F = {
            "FFFFFFFFF ",
            "FF    FFF ",
            "FF        ",
            "FFFFFF    ",
            "FF        ",
            "FF        ",
            "FF        "
        };

        readonly static string[] G = {
            "  GGGGGG  ",
            " GG   GGG ",
            "GG        ",
            "GG        ",
            "GG   GGGG ",
            " GG    GG ",
            "  GGGGGG  "
        };

        readonly static string[] H = {
            "HH  HHHHH ",
            "HH   HHH  ",
            "HH   HHH  ",
            "HHHHHHHH  ",
            "HH   HHH  ",
            "HH   HHH  ",
            "HH  HHHHH "
        };

        readonly static string[] I = {
            "IIIIIIIII ",
            "   III    ",
            "   III    ",
            "   III    ",
            "   III    ",
            "   III    ",
            "IIIIIIIII "
        };

        readonly static string[] J = {
            "    JJJJJ ",
            "      JJJ ",
            "      JJJ ",
            "      JJJ ",
            "JJ    JJJ ",
            "JJ   JJJ  ",
            " JJJJJ    "
        };

        readonly static string[] K = {
            "KK    KKK ",
            "KK  KKK   ",
            "KK KKK    ",
            "KKKKK     ",
            "KKKKKK    ",
            "KK  KKK   ",
            "KK    KKK "
        };

        readonly static string[] L = {
            "LL        ",
            "LL        ",
            "LL        ",
            "LL        ",
            "LL        ",
            "LL    LLL ",
            "LLLLLLLLL "
        };

        readonly static string[] M = {
            " MM  MMM  ",
            "M M MM MM ",
            "M  MM  MM ",
            "M     MMM ",
            "MM    MMM ",
            "MM    MMM ",
            "MMM  MMMM "
        };

        readonly static string[] N = {
            "NNN   NNN ",
            "NNNN  NNN ",
            "NN NN NNN ",
            "NN NN NNN ",
            "NN NN NNN ",
            "NN  NNNNN ",
            "NN   NNNN "
        };

        readonly static string[] O = {
            "  OOOO    ",
            " OO  OOO  ",
            "OO    OOO ",
            "OO    OOO ",
            "OO    OOO ",
            " OO  OOO  ",
            "  OOOO    "
        };

        readonly static string[] P = {
            "PPPPPPP   ",
            "PPP  PPP  ",
            "PP    PPP ",
            "PPP  PPP  ",
            "PPPPPP    ",
            "PPP       ",
            "PP        "
        };

        readonly static string[] Q = {
            "   QQQ    ",
            " QQ  QQQ  ",
            "QQ    QQQ ",
            "QQ   QQQ  ",
            " QQ QQQ   ",
            "  QQQQ  Q ",
            "     QQQ  "
        };

        readonly static string[] R = {
            "RRRRRRRR  ",
            "RRR   RRR ",
            "RR   RRR  ",
            "RRRRRR    ",
            "RR  RRR   ",
            "RR   RRR  ",
            "RR    RRR "
        };

        readonly static string[] S = {
            "   SSSS   ",
            " SSS SSSS ",
            "SSS       ",
            "  SSSS    ",
            "     SSSS ",
            "SS  SSSS  ",
            " SSSSS    "
    };

        readonly static string[] T = {
            "TTTTTTTTT ",
            "TT TTT TT ",
            "   TTT    ",
            "   TTT    ",
            "   TTT    ",
            "   TTT    ",
            "  TTTTT   "
        };

        readonly static string[] U = {
            "UU    UUU ",
            "UU    UUU ",
            "UU    UUU ",
            "UU    UUU ",
            "UU    UUU ",
            " UU  UUU  ",
            "  UUUU    "
        };

        readonly static string[] V = {
            "VV    VVV ",
            "VV    VVV ",
            "VV    VVV ",
            " VV  VVV  ",
            "  VV VV   ",
            "   VVV    ",
            "    V     "
        };

        readonly static string[] W = {
            "WW    WWW ",
            "WW    WWW ",
            "WW    WWW ",
            "WW    WWW ",
            "WW WW WWW ",
            " WW W WW  ",
            "  WW WW   "
        };

        readonly static string[] X = {
            "XX    XXX ",
            " XX   XXX ",
            "  XX XXX  ",
            "   XXX    ",
            "  XX XXX  ",
            " XX   XXX ",
            "XX    XXX "
        };

        readonly static string[] Y = {
            "YY    YYY ",
            " YY  YYY  ",
            "  YYYYY   ",
            "   YYY    ",
            "   YYY    ",
            "   YYY    ",
            "   YYY    "
        };

        readonly static string[] Z = {
            "ZZZZZZZZZ ",
            "Z      ZZ ",
            "ZZ   ZZ   ",
            "   ZZ     ",
            " ZZ       ",
            "ZZ      Z ",
            "ZZZZZZZZZ "
        };

        readonly static string[] Period = {
            "          ",
            "          ",
            "          ",
            "          ",
            "          ",
            "###       ",
            "###       "
        };

        readonly static Dictionary<char, string[]> characters = new Dictionary<char, string[]>() {
            { 'A', A },
            { 'B', B },
            { 'C', C },
            { 'D', D },
            { 'E', E },
            { 'F', F },
            { 'G', G },
            { 'H', H },
            { 'I', I },
            { 'J', J },
            { 'K', K },
            { 'L', L },
            { 'M', M },
            { 'N', N },
            { 'O', O },
            { 'P', P },
            { 'Q', Q },
            { 'R', R },
            { 'S', S },
            { 'T', T },
            { 'U', U },
            { 'V', V },
            { 'W', W },
            { 'Y', Y },
            { 'X', X },
            { 'Z', Z },
            { '.', Period },
        };
        #endregion symbols

        public static string Generate(string text) {
            text = text.ToUpper();

            StringBuilder output = new StringBuilder(text.Length);

            for (int i = 0; i < 7; i++) {
                foreach (char c in text) {
                    if (characters.ContainsKey(c)) {
                        output.Append(characters[c][i].Select(c => c == ' ' ? ' ' : '#').ToArray());
                        output.Append(" ");
                    }
                    else {
                        output.Append(new string(' ', 10));
                    }
                }
                output.Append(Environment.NewLine);
            }

            return output.ToString();
        }
    }
}
