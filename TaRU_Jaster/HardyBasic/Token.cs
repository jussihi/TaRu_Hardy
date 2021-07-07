
namespace TaRU_Jaster.HardyBasic
{
    public enum Token
    {
        Unknown,

        Identifier,
        Value,

        //Keywords
        Print,
        If,
        EndIf,
        Then,
        Else,
        For,
        To,
        Next,
        Goto,
        Input,
        Let,
        Gosub,
        Return,
        Rem,
        End,
        Assert,

        NewLine,
        Colon,
        Semicolon,
        Comma,
        Comment,

        Plus,
        Minus,
        Slash,
        Asterisk,
        Caret,
        Equal,
        Less,
        More,
        NotEqual,
        LessEqual,
        MoreEqual,
        Or,
        And,
        Not,

        LParen,
        RParen,

        EOF = -1   //End Of File
    }
}
