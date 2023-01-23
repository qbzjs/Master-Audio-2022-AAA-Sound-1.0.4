public struct Score
{
    public int score;
    public string explanation;

    public Score(int _score, string _explanation)
    {
        score = _score;
        explanation = _explanation;
    }
    
    public Score(int _score)
    {
        score = _score;
        explanation = _score.ToString();
    }
}