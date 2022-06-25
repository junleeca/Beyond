namespace Beyond.Types;

public class JoinResult<TLeft, TRight>
{
    public TLeft Left { get; set; }
    public TRight Right { get; set; }
}