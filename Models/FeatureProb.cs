namespace NBayes.Models
{
    public class FeatureProb
    {
        public string VarName { get; set; }
        public string LevelName { get; set; }
        public int LevelYCnt { get; set; }
        public double LevelYProb { get; set; }
        public double LevelYAdjProb { get; set; }
        public int LevelNCnt { get; set; }
        public double LevelNProb { get; set; }
        public double LevelNAdjProb { get; set; }


        public string LevelYAdjProbStr
        {
            get { return LevelYAdjProb.ToString("P2"); }
        }

        public string LevelYProbStr
        {
            get { return LevelYProb.ToString("P2"); }
        }

        public string LevelNProbStr
        {
            get { return LevelNProb.ToString("P2"); }
        }

        public string LevelNAdjProbStr
        {
            get { return LevelNAdjProb.ToString("P2"); } 
        }
    }
}